using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBackend.Contract.Inventory
{
    public class updateinventoryRequest
    {
        private string inventoryID;
        private string nameInventory;
        private string descriptionInventory;
        private int amount;
        private int folderID;

        public string InventoryID { get => inventoryID; set => inventoryID = value; }
        public string NameInventory { get => nameInventory; set => nameInventory = value; }
        public string DescriptionInventory { get => descriptionInventory; set => descriptionInventory = value; }
        public int Amount { get => amount; set => amount = value; }
        public int FolderID { get => folderID; set => folderID = value; }
    }
}
