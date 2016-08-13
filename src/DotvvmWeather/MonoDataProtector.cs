using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Owin.Security.DataProtection;

namespace DotvvmWeather
{
    public class MonoDataProtector : IDataProtector
    {
        private const string PRIMARY_PURPOSE = "Microsoft.Owin.Security.IDataProtector";

        private readonly string appName;
        private readonly DataProtectionScope dataProtectionScope;
        private readonly string[] purposes;

        public MonoDataProtector(string appName, string[] purposes = null)
        {
            if (appName == null) { throw new ArgumentNullException("appName"); }
            if (purposes == null)
            {
                purposes = new[] { PRIMARY_PURPOSE };
            }

            this.appName = appName;
            this.purposes = purposes;
            this.dataProtectionScope = DataProtectionScope.CurrentUser;
        }

        public byte[] Protect(byte[] userData)
        {
            return ProtectedData.Protect(userData, this.GetEntropy(), dataProtectionScope);
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return ProtectedData.Unprotect(protectedData, this.GetEntropy(), dataProtectionScope);
        }

        private byte[] GetEntropy()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, sha256, CryptoStreamMode.Write))
                using (StreamWriter writer = new StreamWriter(cryptoStream))
                {
                    writer.Write(this.appName);
                    writer.Write(PRIMARY_PURPOSE);

                    foreach (string purpose in this.purposes)
                    {
                        writer.Write(purpose);
                    }
                }

                return sha256.Hash;
            }
        }
    }
}