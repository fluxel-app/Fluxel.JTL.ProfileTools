using JTL.Shared.Obfuscation;

namespace Fluxel.JTL.ProfileTools
{
    internal static class JtlObfuscatorExtensions
    {
        public static string Deobfuscate(this string str)
        {
            return DefaultObfuscator.Instance.Deobfuscate(str);
        }
        public static string Deobfuscate(this object obj)
        {
            return DefaultObfuscator.Instance.Deobfuscate(obj.ToString());
        }
        public static string Obfuscate(this string str)
        {
            return DefaultObfuscator.Instance.Obfuscate(str);
        }
    }
}