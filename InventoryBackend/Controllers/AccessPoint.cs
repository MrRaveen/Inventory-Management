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
        private createInventory createInventoryObj;
        public AccessPoint(userAccountContext userAccountContext, tokenProvider provider, foldersContext folderContext, createInventory createInventoryObj)
        {
            _userAccountContext = userAccountContext;
            this.provider = provider;
            _folderContext = folderContext;
            this.createInventoryObj = createInventoryObj;
        }

        [HttpPost("/logIn")]
        public async Task<string> logInProcess(loginRequestInput inputValue)
        {
            try
            {
                userAccounts credentials = new userAccounts();
                credentials.userName = inputValue.UserName;
                credentials.password = inputValue.Password;
                logInService service1 = new logInService(_userAccountContext,provider);
                string v = await service1.IsLoggedIn(credentials);
                if(v == "false")
                {
                    return "Invalid username or password";
                }
                else
                {
                    return v;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured in the controller : " + ex.ToString());
                throw new Exception("Error occured in the controller : " + ex.ToString());
            }
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
        //TODO FOLDER UPDATE
        //TODO FOLDER REMOVE
        //TODO FOLDER GET ALL

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
        public string updateByFolderID(int folderID)
        {
            return "";
        }
    }
}
