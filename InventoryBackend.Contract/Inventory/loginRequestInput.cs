using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBackend.Contract.Inventory
{
    public class loginRequestInput
    {
        private string userName;
        private string password;

        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }

        public loginRequestInput(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public loginRequestInput()
        {
        }
    }
}
