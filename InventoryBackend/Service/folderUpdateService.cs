using InventoryBackend.Context;
using InventoryBackend.Models;
using Microsoft.Azure.Cosmos;

namespace InventoryBackend.Service
{
    public class folderUpdateService
    {
        foldersContext _folderContext;
        public folderUpdateService(foldersContext _folderContext)
        {
           this._folderContext = _folderContext;
           
        }
        public async Task<string> folderUpdateProcess(folders updateObj)
        {
            try
            {
                //check existance
                var checkItem = await _folderContext.folders.FindAsync(updateObj.folderID);
                //disposes the context (after await the HTTP request lifecycle ends) then context will dispose
                //use IServiceScopeFactory --> to create a new scope (of the context) for the next process
                if (checkItem != null)
                {
                    checkItem.folderName = updateObj.folderName;
                    checkItem.descriptionFolder = updateObj.descriptionFolder;
                    checkItem.userID = updateObj.userID;
                    await _folderContext.SaveChangesAsync();
                    return "updated";
                }
                else
                {
                    return "Folder did not found";
                }
            }catch(Exception ex)
            {
                throw new Exception("Error occured when updating folders (service) : " + ex.ToString());
            }
        }
    }
}
