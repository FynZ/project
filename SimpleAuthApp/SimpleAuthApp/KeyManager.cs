using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace SimpleAuthApp
{
    public class KeyManager
    {
        public byte[] Key { get; }
        public string Algorithm { get; }

        public KeyManager()
        {
            Key = File.ReadAllBytes(@"C:\Users\Fynzie\Desktop\jwt-key");
            Algorithm = SecurityAlgorithms.RsaSha256;
        }
    }
}
