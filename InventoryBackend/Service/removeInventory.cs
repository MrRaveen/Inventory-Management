using InventoryBackend.Models;
using Microsoft.Azure.Cosmos;

namespace InventoryBackend.Service
{
    public class removeInventory
    {
        private string Database;
        private string containerDB;
        private readonly Microsoft.Azure.Cosmos.Container container;
        private IConfiguration configurationLocal;
        public removeInventory(CosmosClient client, IConfiguration configuration)
        {
            configurationLocal = configuration;
            Database = configuration["azurecosmos:database"];
            containerDB = configuration["azurecosmos:container1"];
            container = client.GetContainer(Database, containerDB);//creates the container
        }
        public async Task<string> removeProcess(string inventoryID)
        {
            try
            {
                string partitionKey = configurationLocal["azurecosmos:partitionKeyValue1"];
                var partition2 = new PartitionKey(partitionKey);
                ItemResponse<inventory> responseRemoved = await container.DeleteItemAsync<inventory>(inventoryID,partition2);
                if (responseRemoved != null)
                {
                    return "Removed";
                }
                else
                {
                    return "Data did not removed";
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error occured when removing inventory (service) : " + ex.ToString());
            }
        }
    }
}
