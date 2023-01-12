using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Fluxel.JTL.ProfileTools
{
    public static class ClientRepository
    {

        public static IEnumerable<Client> GetClients(Profile profile)
        {
            using (var dt = SqlRepository.SelectFromProfile(profile, "SELECT * FROM eazybusiness.dbo.tMandant"))
            {
                return dt.Rows.Cast<DataRow>()
                        .Select(row => new Client
                        {
                            ClientId = (int)row["kMandant"],
                            Name = (string)row["cName"],
                            Database = (string)row["cDB"],
                        })
                        .ToList();
            }
        }
    }
}