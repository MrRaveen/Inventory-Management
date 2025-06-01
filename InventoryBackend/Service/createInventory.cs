using InventoryBackend.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace InventoryBackend.Service
{
    public class createInventory
    {
        private string Database;
        private string containerDB;
        private readonly Microsoft.Azure.Cosmos.Container container;
        public createInventory(CosmosClient client, IConfiguration configuration)
        {
            //get the db information
            Database = configuration["azurecosmos:database"];
            containerDB = configuration["azurecosmos:container1"];
            container = client.GetContainer(Database,containerDB);//creates the container
        }
        public async Task<string> createProcessInventory(inventory newInventoryData)
        {
            try
            {
                var document = new
                {
                    id = newInventoryData.id,
                    name = newInventoryData.name,
                    description = newInventoryData.description,
                    amount = newInventoryData.amount,
                    folderID = newInventoryData.folderID,
                    inventory = new
                    {
                        Name = "valueSec1"
                    },
                };
                await container.CreateItemAsync(document);
                return "succeed";
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when creating inventory : " + ex.ToString());
            }
        }
    }
}
