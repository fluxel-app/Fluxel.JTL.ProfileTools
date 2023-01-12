using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System;
using System.Reflection;
using Microsoft.Win32;
using System.Linq;
using System.IO;

namespace Fluxel.JTL.ProfileTools
{
    internal class Program
    {
        public static string JtlPath { get; private set; }

        static void Main(string[] args)
        {
            PrepareJtlDllResolvement();

            var sb = new StringBuilder();

            foreach (var p in ProfileRepository.GetProfiles())
            {
                sb.AppendLine("######################################");
                sb.AppendLine("Name:" + p.Name);
                sb.AppendLine("SQL-Server:" + p.Server);
                sb.AppendLine("SQL-Benutzername:" + p.User);
                sb.AppendLine("SQL-Passwort:" + p.Password);
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());

            string file = null;
            while(string.IsNullOrWhiteSpace(file))
            {
                Console.Write("Speicherpfad oder [ENTER], um das Programm zu beenden: ");
                file = Console.ReadLine();
                if(string.IsNullOrEmpty(file))
                {
                    file = null;
                    break;
                }
            }

            if(file == null)
            {
                return;
            }

            File.WriteAllText(file, sb.ToString());
        }

        private static void PrepareJtlDllResolvement()
        {
            JtlPath = Path.GetDirectoryName(FindInstallLocation());
            AppDomain.CurrentDomain.AssemblyResolve += LoadJtlAssemblys;
        }

        private static Assembly LoadJtlAssemblys(object sender, ResolveEventArgs args)
        {
            var assemblyPath = Path.Combine(JtlPath ?? throw new InvalidOperationException(), $"{new AssemblyName(args.Name).Name}.dll");
            if (!File.Exists(assemblyPath))
                return null;
            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }

        private static string FindInstallLocation()
        {
            var cLocation = FindUninstallSubkey(@"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            return !string.IsNullOrEmpty(cLocation) ? cLocation : FindUninstallSubkey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
        }

        private static string FindUninstallSubkey(string baseKey)
        {
            var oKey = Registry.LocalMachine.OpenSubKey(baseKey);
            if (oKey == null)
                return null;
            return oKey.GetSubKeyNames()
                .Select(oKey.OpenSubKey)
                .Where(oSubKey => Equals("JTL-Wawi", oSubKey.GetValue("DisplayName")))
                .Select(oSubKey => Convert.ToString(oSubKey.GetValue("InstallLocation")))
                .FirstOrDefault();
        }
    }
}