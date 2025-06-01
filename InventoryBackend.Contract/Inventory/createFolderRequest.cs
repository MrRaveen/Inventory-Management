using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBackend.Contract.Inventory
{
    public class createFolderRequest
    {
        private int userID;
        private string folderName;
        private string descriptionFolder;

        public int UserID { get => userID; set => userID = value; }
        public string FolderName { get => folderName; set => folderName = value; }
        public string DescriptionFolder { get => descriptionFolder; set => descriptionFolder = value; }

        public createFolderRequest(int userID, string folderName, string descriptionFolder)
        {
            UserID = userID;
            FolderName = folderName;
            DescriptionFolder = descriptionFolder;
        }

        public createFolderRequest()
        {
        }
    }
}
