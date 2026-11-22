using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace NovaCenter.BUS
{
    internal static class CsvExporter
    {
        public static void Save(DataTable table, string path)
        {
            using (var writer = new StreamWriter(path,false,new UTF8Encoding(true)))
            {
                writer.WriteLine(string.Join(",",table.Columns.Cast<DataColumn>().Select(c => Cell(c.ColumnName))));
                foreach (DataRow row in table.Rows)
                    writer.WriteLine(string.Join(",",row.ItemArray.Select(value =>
                    {
                        string text = value is DateTime date ? date.ToString("dd/MM/yyyy",CultureInfo.InvariantCulture)
                            : Convert.ToString(value,CultureInfo.InvariantCulture);
                        return Cell(text);
                    })));
            }
        }

        private static string Cell(string text)
        {
            // Avoid treating user-entered values as Excel formulas.
            if (text.Length > 0 && "=+-@\t\r\n".IndexOf(text[0]) >= 0) text = "'" + text;
            return "\"" + text.Replace("\"","\"\"") + "\"";
        }
    }
}
