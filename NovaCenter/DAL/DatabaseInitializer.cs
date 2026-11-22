using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NovaCenter.DAL
{
    internal static class DatabaseInitializer
    {
        public static Task EnsureAsync() => Task.Run(() => Ensure());

        public static void Ensure()
        {
            var settings = new SqlConnectionStringBuilder(Db.ConnectionString);
            string name = settings.InitialCatalog;
            if (!Regex.IsMatch(name, @"^[A-Za-z][A-Za-z0-9_]{0,90}$"))
                throw new InvalidOperationException("Tên CSDL chỉ được chứa chữ, số và dấu gạch dưới.");
            settings.InitialCatalog = "master";
            using (var connection = new SqlConnection(settings.ConnectionString))
            {
                connection.Open();
                using (var command = Db.Command(connection, null,
                    "SELECT DB_ID(@name)", Db.P("@name", name)))
                {
                    if (command.ExecuteScalar() == DBNull.Value)
                    {
                        using (var create = new SqlCommand("CREATE DATABASE [" + name + "]", connection))
                        {
                            create.CommandTimeout = 60;
                            try { create.ExecuteNonQuery(); }
                            catch (SqlException ex) when (ex.Number == 1801) { }
                        }
                    }
                }
            }

            Db.Transaction((connection, transaction) =>
            {
                using (var gate = Db.Command(connection, transaction, @"
                    DECLARE @r int;
                    EXEC @r=sys.sp_getapplock @Resource=N'NovaSchema',@LockMode='Exclusive',
                        @LockOwner='Transaction',@LockTimeout=30000;
                    IF @r<0 THROW 50001,'Cannot lock Nova schema',1;"))
                    gate.ExecuteNonQuery();
                string schema;
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NovaCenter.Schema.sql"))
                using (var reader = new StreamReader(stream))
                    schema = reader.ReadToEnd();
                using (var command = Db.Command(connection, transaction, schema))
                    command.ExecuteNonQuery();
                return true;
            });
        }
    }
}
