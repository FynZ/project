using Newtonsoft.Json;

namespace Accounts.Services.HostedServices.Communication.Models
{
    public class Exchange
    {
        [JsonProperty("internal")]
        public bool Internal { get; set; }

        [JsonProperty("auto_delete")]
        public bool AutoDelete { get; set; }

        [JsonProperty("durable")]
        public bool Durable { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("vhost")]
        public string VHost { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
