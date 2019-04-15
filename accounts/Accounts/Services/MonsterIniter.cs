using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog;

namespace Accounts.Services
{
    public class MonsterIniter : IMonsterIniter
    {
        public void InitMonster(int userId)
        {
            using (var client = new HttpClient())
            using (HttpResponseMessage response = client.GetAsync($"hhttp://localhost:8080/monsters/init/{userId}").Result)
            using (HttpContent content = response.Content)
            {
                if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 300)
                {
                    Log.Information("Successfully inited monsters for user {@UserId}", userId);
                }
                else
                {
                    Log.Error("Failed to init monsters for user {@UserId}", userId);
                }
            }
        }
    }
}
