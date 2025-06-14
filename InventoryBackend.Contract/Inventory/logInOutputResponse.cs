using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBackend.Contract.Inventory
{
    public class logInOutputResponse
    {
        public string token {  get; set; }
        public int userID { get; set; }
    }
}
