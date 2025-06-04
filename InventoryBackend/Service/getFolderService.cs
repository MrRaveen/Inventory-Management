using InventoryBackend.Context;
using InventoryBackend.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

namespace InventoryBackend.Service
{
    public class getFolderService
    {
        private foldersContext context;
        public getFolderService(foldersContext context)
        {
            this.context = context;
        }
        public async Task<List<folders>> getFolderProcess(int userID)
        {
            try
            {
                List<folders> outputData = await context.folders.Where(p => p.userID == userID).ToListAsync();
                return outputData;
            }catch (Exception ex)
            {
                throw new Exception("Error occured when ");
            }
        }
    }
}
