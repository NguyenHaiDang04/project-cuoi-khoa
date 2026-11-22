using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace NovaCenter.DAL
{
    internal static class Db
    {
        public static string ConnectionString => Environment.GetEnvironmentVariable("NOVA_CONNECTION")
            ?? ConfigurationManager.ConnectionStrings["NovaCenter"].ConnectionString;

        public static SqlConnection Open()
        {
            var connection = new SqlConnection(ConnectionString);
            try { connection.Open(); return connection; }
            catch { connection.Dispose(); throw; }
        }

        public static SqlCommand Command(SqlConnection connection, SqlTransaction transaction,
            string sql, params SqlParameter[] parameters)
        {
            var command = new SqlCommand(sql, connection, transaction);
            command.Parameters.AddRange(parameters);
            return command;
        }

        public static DataTable Read(string sql, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = Command(connection, null, sql, parameters))
            using (var adapter = new SqlDataAdapter(command))
            {
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public static int Execute(string sql, params SqlParameter[] parameters)
        {
            using (var connection = Open())
            using (var command = Command(connection, null, sql, parameters))
                return command.ExecuteNonQuery();
        }

        public static T Transaction<T>(Func<SqlConnection, SqlTransaction, T> action)
        {
            using (var connection = Open())
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    T result = action(connection, transaction);
                    transaction.Commit();
                    return result;
                }
                catch
                {
                    if (transaction.Connection != null) transaction.Rollback();
                    throw;
                }
            }
        }

        public static SqlParameter P(string name, object value) =>
            new SqlParameter(name, value ?? DBNull.Value);

        public static SqlParameter Money(string name, decimal value) =>
            new SqlParameter(name, SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = value };

        public static SqlParameter Score(string name, decimal value) =>
            new SqlParameter(name, SqlDbType.Decimal) { Precision = 4, Scale = 2, Value = value };

        public static int Identity(string sql, params SqlParameter[] parameters)
        {
            using (var connection = Open())
            using (var command = Command(connection, null, sql, parameters))
                return (int)command.ExecuteScalar();
        }
    }
}
