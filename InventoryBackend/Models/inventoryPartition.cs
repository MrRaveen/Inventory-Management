using Newtonsoft.Json;

namespace InventoryBackend.Models
{
    public class inventoryPartition
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}
