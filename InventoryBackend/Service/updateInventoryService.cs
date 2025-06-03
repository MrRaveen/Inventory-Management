using InventoryBackend.Models;
using Microsoft.Azure.Cosmos;
using System.ComponentModel;

namespace InventoryBackend.Service
{
    public class updateInventoryService
    {
        private string Database;
        private string containerDB;
        private readonly Microsoft.Azure.Cosmos.Container container;
        private IConfiguration configurationLocal;
        public updateInventoryService(CosmosClient client, IConfiguration configuration)
        {
            configurationLocal = configuration;
            Database = configuration["azurecosmos:database"];
            containerDB = configuration["azurecosmos:container1"];
            container = client.GetContainer(Database, containerDB);//creates the container
        }
        public async Task<string> updateProcessAsync(inventory passingObj)
        {
            try
            {
                string partitionKey = configurationLocal["azurecosmos:partitionKeyValue1"];
                var partition2 = new PartitionKey(partitionKey);
                //check the item existance
                ItemResponse<inventory> response = await container.ReadItemAsync<inventory>(passingObj.id, partition2);

                //HACK INPLEMENT NULL
                inventory editObj = response.Resource;
                editObj.id = passingObj.id;
                editObj.name = passingObj.name;
                editObj.description = passingObj.description;
                editObj.amount = passingObj.amount;
                editObj.folderID = passingObj.folderID;
                ItemResponse<inventory> updateResponse = await container.ReplaceItemAsync<inventory>(editObj, editObj.id, partition2);
                if (updateResponse != null)
                {
                    return "Data updated";
                }
                else
                {
                    return "Data not updated";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when updating inventory (updateInventoryService) : " + ex.ToString());
            }
        }
    }
}
