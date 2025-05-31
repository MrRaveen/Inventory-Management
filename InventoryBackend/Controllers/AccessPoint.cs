using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.Data;
using InventoryBackend.Contract.Inventory;
using InventoryBackend.Context;
using InventoryBackend.Models;
using Microsoft.EntityFrameworkCore;
using InventoryBackend.Service;
using System.Diagnostics;

namespace InventoryBackend.Controllers
{
    [ApiController]
    public class AccessPoint : ControllerBase
    {
        private readonly userAccountContext _userAccountContext;
        public AccessPoint(userAccountContext userAccountContext)
        {
            _userAccountContext = userAccountContext;
        }

        [HttpPost("/logIn")]
        public async Task<bool> logInProcess(loginRequestInput inputValue)
        {
            try
            {
                userAccounts credentials = new userAccounts();
                credentials.userName = inputValue.UserName;
                credentials.password = inputValue.Password;
                logInService service1 = new logInService(_userAccountContext);
                bool v = await service1.IsLoggedIn(credentials);
                if (v == true)
                {
                    return true;
                }
                else
                {
                    return false;
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
                userAccounts convertedInfo = new userAccounts();
                convertedInfo.userName = createAccRequest.UserName;
                convertedInfo.password = createAccRequest.Password;

                createAccService createObj = new createAccService(_userAccountContext);
                userAccounts createdResult = await createObj.createProcessAsync(convertedInfo);

                if(createdResult == null)
                {
                    return "Account did not created. Try again";
                }
                else
                {
                    return "Account created";
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
    }
}
