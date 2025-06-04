using InventoryBackend.Context;

namespace InventoryBackend.Service
{
    public class folderRemoveService
    {
        foldersContext foldersContext;
        public folderRemoveService(foldersContext foldersContext)
        {
            this.foldersContext = foldersContext;
        }
        public async Task<string> removeFolderProcess(int folderID)
        {
            try
            {
                var checkExistance = await foldersContext.folders.FindAsync(folderID);
                if (checkExistance != null)
                {
                    foldersContext.Remove(checkExistance);
                    await foldersContext.SaveChangesAsync();
                    return "removed";
                }
                else
                {
                    return "not removed";
                }
            }catch (Exception ex)
            {
                throw new Exception("Error occured when removing the folder (service) : " + ex.ToString());
            }
        }
    }
}
