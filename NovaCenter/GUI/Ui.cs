using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NovaCenter.GUI
{
    internal static class Ui
    {
        public static void Run(Label message, Action action)
        {
            try
            {
                Notice(message,"");
                action();
            }
            catch (ArgumentException ex) { Notice(message,ex.Message,true); }
            catch (InvalidOperationException ex) { Notice(message,ex.Message,true); }
            catch (UnauthorizedAccessException ex) { Notice(message,ex.Message,true); }
            catch (IOException ex) { Notice(message,"Không ghi được tệp. " + ex.Message,true); }
            catch (SqlException ex)
            {
                string text;
                switch (ex.Number)
                {
                    case 2601:
                    case 2627: text = "Mã hoặc dữ liệu đã tồn tại. Vui lòng kiểm tra lại."; break;
                    case 547: text = "Bản ghi đang có dữ liệu liên quan hoặc không thỏa điều kiện."; break;
                    default: text = "Không thao tác được với SQL Server (" + ex.Number + "). Kiểm tra kết nối rồi thử lại."; break;
                }
                Notice(message,text,true);
            }
        }

        public static void Notice(Label label, string text, bool error = false)
        {
            label.Text = text;
            label.ForeColor = error ? Color.Firebrick : Color.DarkGreen;
        }

        public static bool Required(ErrorProvider errors, params TextBox[] fields)
        {
            errors.Clear();
            bool valid = true;
            foreach (TextBox field in fields)
            {
                if (!string.IsNullOrWhiteSpace(field.Text)) continue;
                errors.SetError(field,"Không được để trống.");
                valid = false;
            }
            return valid;
        }

        public static DataRow Row(DataGridView grid) => (grid.CurrentRow?.DataBoundItem as DataRowView)?.Row;
        public static int Id(ComboBox combo) => combo.SelectedValue is int ? (int)combo.SelectedValue : 0;

        public static void Bind(ComboBox combo, DataTable table, string display, bool all = false)
        {
            int previous = Id(combo);
            if (all)
            {
                DataRow blank = table.NewRow();
                blank["Id"] = 0;
                blank[display] = "Tất cả";
                table.Rows.InsertAt(blank,0);
            }
            combo.DataSource = null;
            combo.ValueMember = "Id";
            combo.DisplayMember = display;
            combo.DataSource = table;
            foreach (DataRow row in table.Rows)
                if ((int)row["Id"] == previous) { combo.SelectedValue = previous; break; }
        }

        public static void Grid(DataGridView grid, DataTable table, params string[] headers)
        {
            grid.DataSource = table;
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                DataGridViewColumn c = grid.Columns[i];
                if (i < headers.Length) c.HeaderText = headers[i];
                if (c.Name == "Id" || c.Name.EndsWith("Id")) c.Visible = false;
                if (c.ValueType == typeof(DateTime)) c.DefaultCellStyle.Format = "dd/MM/yyyy";
                if (c.ValueType == typeof(decimal)) c.DefaultCellStyle.Format = "N2";
            }
        }

        public static bool ConfirmDelete(IWin32Window owner, int id)
        {
            if (id <= 0) throw new ArgumentException("Chọn một dòng trước.");
            return MessageBox.Show(owner,"Xóa bản ghi đang chọn?","Xác nhận",
                MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}
