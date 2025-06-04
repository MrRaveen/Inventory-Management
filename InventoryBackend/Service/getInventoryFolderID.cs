using InventoryBackend.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace InventoryBackend.Service
{
    public class getInventoryFolderID
    {
        private string Database;
        private string containerDB;
        private readonly Microsoft.Azure.Cosmos.Container container;
        private IConfiguration configurationLocal;
        public getInventoryFolderID(CosmosClient client, IConfiguration configuration)
        {
            configurationLocal = configuration;
            Database = configuration["azurecosmos:database"];
            containerDB = configuration["azurecosmos:container1"];
            container = client.GetContainer(Database, containerDB);//creates the container
        }
        public async Task<List<inventory>> getProcess(int folderID)
        {
            try
            {
                List<inventory> outputInventory = new List<inventory>();
                using (FeedIterator<inventory> setIterator = container.GetItemLinqQueryable<inventory>().Where(b => b.folderID == folderID).ToFeedIterator())
                {
                    while (setIterator.HasMoreResults)
                    {
                        foreach (var inventoryTemp in await setIterator.ReadNextAsync())
                        {
                            outputInventory.Add(inventoryTemp);
                        }
                    }
                }
                return outputInventory;
            }
            catch(Exception ex)
            {
                throw new Exception("Error occured when retriving data (service) : " + ex.ToString());
            }
        }
    }
}
