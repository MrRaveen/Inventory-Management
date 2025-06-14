using InventoryBackend.Context;
using InventoryBackend.Contract.Inventory;
using InventoryBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Threading.Tasks;

namespace InventoryBackend.Service
{
    public class logInService(userAccountContext _userAccountContext, tokenProvider provider)
    {
        public async Task<logInOutputResponse> IsLoggedIn(userAccounts passingCredentials)
        {
            //async Task<ActionResult<IEnumerable<userAccounts>>>
            //await _userAccountContext.userAccounts.ToListAsync()
            try
            {
                List<userAccounts> result = await _userAccountContext.userAccounts.Where(p => p.userName == passingCredentials.userName).ToListAsync();
                if (result.IsNullOrEmpty())
                {
                    return null;
                }
                else
                {
                    foreach (var item in result)
                    {
                        if (item.password == passingCredentials.password)
                        {
                            string token = provider.Create(item);
                            Debug.WriteLine(token);//HACK TEST
                            logInOutputResponse response1 = new logInOutputResponse();
                            response1.token = token;
                            response1.userID = item.userID;
                            Debug.WriteLine(response1.ToString());
                            return response1;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return null;
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); //TODO TEST
                throw new Exception("Error occured when login (service) : " + ex.ToString());
            }
        }
    }
}
