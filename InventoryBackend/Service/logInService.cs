using InventoryBackend.Context;
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
        public async Task<string> IsLoggedIn(userAccounts passingCredentials)
        {
            //async Task<ActionResult<IEnumerable<userAccounts>>>
            //await _userAccountContext.userAccounts.ToListAsync()
            try
            {
                List<userAccounts> result = await _userAccountContext.userAccounts.Where(p => p.userName == passingCredentials.userName).ToListAsync();
                if (result.IsNullOrEmpty())
                {
                    return "false";
                }
                else
                {
                    foreach (var item in result)
                    {
                        if (item.password == passingCredentials.password)
                        {
                            string token = provider.Create(item);
                            Debug.WriteLine(token);//HACK TEST
                            return token;
                        }
                        else
                        {
                            return "false";
                        }
                    }
                    return "false";
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); //TODO TEST
                throw new Exception("Error occured when login (service) : " + ex.ToString());
            }
        }
    }
}
