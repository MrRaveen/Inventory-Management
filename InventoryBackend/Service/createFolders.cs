using InventoryBackend.Context;
using InventoryBackend.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace InventoryBackend.Service
{
    public class createFolders(foldersContext _folderContext)
    {
        public async Task<string> createFolderProcess(folders folderInput)
        {
            /*
             create only folder process
             */
            try
            {
                var result = _folderContext.folders.Add(folderInput);
                await _folderContext.SaveChangesAsync();
                return "Folder created";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());//HACK TEST
                return "Error occured when saving the folder : " + ex.ToString();
            }
        }
    }
}
