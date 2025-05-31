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

namespace InventoryBackend.Controllers
{
    [ApiController]
    public class AccessPoint : ControllerBase
    {
        private readonly userAccountContext _userAccountContext;
        private tokenProvider provider;
        public AccessPoint(userAccountContext userAccountContext, tokenProvider provider)
        {
            _userAccountContext = userAccountContext;
            this.provider = provider;
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
        [Authorize]
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

    }
}
