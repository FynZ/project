using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Models
{
    public class UserMonster
    {
        public int Id { get; set; }
        public int MonsterId { get; set; }
        public int Count { get; set; }
        public bool Search { get; set; }
        public bool Propose { get; set; }
        public int UserId { get; set; }
    }
}
