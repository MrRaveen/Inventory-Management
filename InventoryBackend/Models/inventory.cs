using Newtonsoft.Json;

namespace InventoryBackend.Models
{
    public class inventory
    {
        public string id {  get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int amount { get; set; }
        public int folderID { get; set; }
    }
}
