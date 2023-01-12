using System;
using System.Data.SqlClient;
using System.Data;

namespace Fluxel.JTL.ProfileTools
{
    internal static class SqlRepository
    {
        public static DataTable SelectFromProfile(Profile profile, string sql)
        {
            var dt = new DataTable();
            try
            {
                using (var conn = GetConnection(profile))
                {
                    using (var command = new SqlCommand(sql, conn)
                    {
                        CommandTimeout = 2,
                    })
                    {
                        using (var adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            return dt;
        }

        public static SqlConnection GetConnection(Profile profile, string database = null)
        {
            var csb = new SqlConnectionStringBuilder
            {
                DataSource = profile.Server,
                UserID = profile.User,
                Password = profile.Password,
                ConnectTimeout = 5,
                TrustServerCertificate = true,
            };
            if (database != null)
            {
                csb.InitialCatalog = database;
            }
            return new SqlConnection(csb.ConnectionString);
        }
    }
}