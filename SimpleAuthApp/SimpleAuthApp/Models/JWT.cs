using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAuthApp.Models
{
    public class JWT
    {
        public string Token { get; set; }
        public long Expires { get; set; }
    }
}
