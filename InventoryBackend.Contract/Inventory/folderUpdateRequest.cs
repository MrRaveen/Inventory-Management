using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBackend.Contract.Inventory
{
    public class folderUpdateRequest
    {
        public int folderID { get; set; }
        public string folderName { get; set; }
        public string descriptionFolder { get; set; }
        public int userID { get; set; }
    }
}
