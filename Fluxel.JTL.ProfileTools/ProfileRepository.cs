using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Win32;

namespace Fluxel.JTL.ProfileTools
{
    public static class ProfileRepository
    {
        private const string JtlKey = @"Software\jtl-software";
        private const string ProfileKey = @"Software\jtl-software\Profiles";

        public static IEnumerable<Profile> GetProfiles()
        {
            return GetProfileKeys().Select(GetProfile);
        }

        public static Profile GetProfile(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            var k1 = Registry.CurrentUser.OpenSubKey(ProfileKey + $"\\{key}");
            var profile = new Profile
            {
                Name = key
            };

            foreach (var valueName in k1?.GetValueNames() ?? throw new Exception())
            {
                if (valueName == "Key1")
                {
                    profile.Server = k1.GetValue(valueName).Deobfuscate();
                }
                if (valueName == "Key2")
                {
                    profile.User = k1.GetValue(valueName).Deobfuscate();
                }
                if (valueName == "Key3")
                {
                    profile.Password = k1.GetValue(valueName).Deobfuscate();
                }
            }

            return profile;
        }

        public static void SetProfile(string key, string server, string user, string pass)
        {
            var cu = Registry.CurrentUser;
            if (cu.OpenSubKey(JtlKey + @"\eazyBusiness", true) == null) cu.CreateSubKey(JtlKey + @"\eazyBusiness");
            if (cu.OpenSubKey(JtlKey + @"\MessageBoxes", true) == null) cu.CreateSubKey(JtlKey + @"\MessageBoxes");
            var reg = cu.OpenSubKey(ProfileKey + $"\\{key}", true) ?? cu.CreateSubKey(ProfileKey + $"\\{key}");

            reg.SetObfuscatedValue("Key1", server);
            reg.SetObfuscatedValue("Key2", user);
            reg.SetObfuscatedValue("Key3", pass);
            reg.SetObfuscatedValue("Key4", "Server: " + server);
            reg.SetObfuscatedValue("Key5", "admin");
            reg.SetObfuscatedValue("Key6", "eB-Standard");
            reg.SetObfuscatedValue("Key7", "0");
        }

        private static void SetObfuscatedValue(this RegistryKey reg, string key, string value)
        {
            reg.SetValue(key, value.Obfuscate());
        }

        public static IEnumerable<string> GetProfileKeys()
        {
            return Registry.CurrentUser.OpenSubKey(ProfileKey)?.GetSubKeyNames();
        }

    }
}