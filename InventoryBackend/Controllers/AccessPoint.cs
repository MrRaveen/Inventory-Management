using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.Data;
using InventoryBackend.Contract.Inventory;
using InventoryBackend.Context;
using InventoryBackend.Models;
using Microsoft.EntityFrameworkCore;
using InventoryBackend.Service;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace InventoryBackend.Controllers
{
    [ApiController]
    public class AccessPoint : ControllerBase
    {
        private readonly userAccountContext _userAccountContext;
        private tokenProvider provider;
        private foldersContext _folderContext;
        private createInventory createInventoryObj;//create inventory
        private updateInventoryService updateInventoryObj;//update inventory
        private removeInventory removeInventoryObj;//remove inventory
        private getInventoryFolderID getInventoryFolderIDObj;//get inventory
        public AccessPoint(userAccountContext userAccountContext, tokenProvider provider, foldersContext folderContext, createInventory createInventoryObj, updateInventoryService updateInventoryObj, removeInventory removeInventoryObj, getInventoryFolderID getInventoryFolderIDObj)
        {
            _userAccountContext = userAccountContext;
            this.provider = provider;
            _folderContext = folderContext;
            this.createInventoryObj = createInventoryObj;
            this.removeInventoryObj = removeInventoryObj;
            this.getInventoryFolderIDObj = getInventoryFolderIDObj;
        
        }

        [HttpPost("/logIn")]
        public async Task<logInOutputResponse> logInProcess(loginRequestInput inputValue)
        {
            logInOutputResponse result = null;
            for(int count = 0;count < 4; count++)
            {
                try
                {
                    userAccounts credentials = new userAccounts();
                    credentials.userName = inputValue.UserName;
                    credentials.password = inputValue.Password;
                    logInService service1 = new logInService(_userAccountContext, provider);
                    logInOutputResponse v = await service1.IsLoggedIn(credentials);
                    if (v == null)
                    {
                        result = null;
                    }
                    else
                    {
                        result = v;
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured in the controller : " + ex.ToString());
                    //throw new Exception("Error occured in the controller : " + ex.ToString());
                    //result = ex.ToString();
                }
            }
            return result;
        }
        [HttpPost("/createAccount")]
        public async Task<string> createAccProcess(loginRequestInput createAccRequest)
        {
            try
            {
                if (createAccRequest.UserName.IsNullOrEmpty() || createAccRequest.Password.IsNullOrEmpty())
                {
                    return "Username or password is empty";
                }
                else
                {
                    userAccounts convertedInfo = new userAccounts();
                    convertedInfo.userName = createAccRequest.UserName;
                    convertedInfo.password = createAccRequest.Password;

                    createAccService createObj = new createAccService(_userAccountContext);
                    userAccounts createdResult = await createObj.createProcessAsync(convertedInfo);

                    if (createdResult == null)
                    {
                        return "Account did not created. Try again";
                    }
                    else
                    {
                        return "Account created";
                    }
                }
            }
            catch (ArgumentException e1)
            {
                Debug.WriteLine(e1.ToString());
                return e1.ToString();
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                return e.ToString();
            }
        }
        [HttpPost("/createFolder")]
        [Authorize]
        public async Task<string> createProcess(createFolderRequest folderRequest)
        {
            try
            {
                folders newFolder = new folders();
                newFolder.folderName = folderRequest.FolderName;
                newFolder.descriptionFolder = folderRequest.DescriptionFolder;
                newFolder.userID = folderRequest.UserID;
                createFolders createObj = new createFolders(_folderContext);
                string savedFolderResult = await createObj.createFolderProcess(newFolder);
                return savedFolderResult;
            }catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw new Exception("Error occured in the controller when saving folder : " + e.ToString());
            }
        }
        [HttpPut("/updateFolder")]
        [Authorize]
        public async Task<string> updateProcess(folderUpdateRequest updateRequest)
        {
            try
            {
                folders updateFolders = new folders();
                updateFolders.folderID = updateRequest.folderID;
                updateFolders.folderName = updateRequest.folderName;
                updateFolders.descriptionFolder = updateRequest.descriptionFolder;
                updateFolders.userID = updateRequest.userID;
                folderUpdateService folderUpdateObj = new folderUpdateService(_folderContext);
                string updateResult = await folderUpdateObj.folderUpdateProcess(updateFolders);
                return updateResult;
            }
            catch(Exception ex)
            {
                throw new Exception("Error occured when updating folder (controller) : " + ex.ToString());
            }
        }
        [HttpDelete("/removeFolder/{folderID}")]
        [Authorize]
        public async Task<string> removeFolderByID(int folderID)
        {
            try
            {
                folderRemoveService removeObj = new folderRemoveService(_folderContext);
                return await removeObj.removeFolderProcess(folderID);
            }catch(Exception e)
            {
                throw new Exception("Error occured when removing the folder (controller) : " + e.ToString());
            }
        }
        [HttpGet("/getFoldersByUser/{userID}")]
        [Authorize]
        public async Task<List<folders>> getFoldersByUserID(int userID)
        {
            try
            {
                getFolderService getObj = new getFolderService(_folderContext);
                return await getObj.getFolderProcess(userID);
            }catch(Exception e)
            {
                throw new Exception("Error occured when selecting folders (controller) : " + e.ToString());
            }
        }
        [HttpPost("/createInventory")]
        [Authorize]
        public async Task<string> createInventoryProcess(createInventoryRequest inventoryData)
        {
            try
            {
                if (inventoryData.NameInventory.IsNullOrEmpty() || inventoryData.DescriptionInventory.IsNullOrEmpty() || inventoryData.FolderID == 0 || inventoryData.Amount == 0)
                {
                    return "Some values are null";
                }
                else
                {
                    inventory newInventory = new inventory();
                    newInventory.name = inventoryData.NameInventory;
                    newInventory.description = inventoryData.DescriptionInventory;
                    newInventory.amount = inventoryData.Amount;
                    newInventory.folderID = inventoryData.FolderID;
                    newInventory.id = Guid.NewGuid().ToString();
                    Task<string> result = createInventoryObj.createProcessInventory(newInventory);
                    return await result;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error occured in the controller when saving inventory : " + ex.ToString());
            }
        }
        //update inventory by id
        [HttpPut("/updateInventory")]
        [Authorize]
        public async Task<string> updateByInventoryID(updateinventoryRequest request)
        {
            try
            {
                inventory updateObj = new inventory();
                updateObj.id = request.InventoryID;
                updateObj.name = request.NameInventory;
                updateObj.description = request.DescriptionInventory;
                updateObj.amount = request.Amount;
                updateObj.folderID = request.FolderID;
                string updateResult = await updateInventoryObj.updateProcessAsync(updateObj);
                return updateResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when updating data (controller) : " + ex.ToString());
            }
        }
        [HttpDelete("/removeInventory/{inventoryID}")]
        [Authorize]
        public async Task<string> removeInventoryByID(string inventoryID)
        {
            try
            {
                string result = await removeInventoryObj.removeProcess(inventoryID);
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception("Error occured when removing inventory (controller) : " + ex.ToString());
            }
        }
        //get all the inventory 
        [HttpGet("/getAllInventory/{folderID}")]
        [Authorize]
        public async Task<List<inventory>> getInventoryByFolderID(int folderID)
        {
            try
            {
                List<inventory> inventoryList = await getInventoryFolderIDObj.getProcess(folderID);
                return inventoryList;
            }catch(Exception ex)
            {
                throw new Exception("Error occured when getting inventory (controller) : " + ex.ToString());
            }
        }
    }
}
