using InventoryBackend.Context;
using InventoryBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace InventoryBackend.Service
{
    public class createAccService
    {
        private readonly userAccountContext _userAccountContext;
        public createAccService(userAccountContext userAccountContext)
        {
            _userAccountContext = userAccountContext;
        }
        public async Task<userAccounts> createProcessAsync(userAccounts newValues)
        {
            try
            {
                var dateTime = DateTime.Now;
                String shortDateValue = dateTime.ToShortDateString();
                Debug.WriteLine(shortDateValue.ToString());
                DateTime shortDateValue1 = DateTime.Parse(shortDateValue); 
                newValues.createdDate = shortDateValue1;
                //Check the existance
                List<userAccounts> checkResult = await _userAccountContext.userAccounts.Where(p => p.userName == newValues.userName).ToListAsync();
                if (checkResult.IsNullOrEmpty())
                {
                    var result4 = _userAccountContext.userAccounts.Add(newValues);
                    await _userAccountContext.SaveChangesAsync();
                    return newValues;
                }
                else
                {
                    throw new ArgumentException("The username is used. Try with another username");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());//TODO TEST
                throw new Exception("Error occured when creating the account (service) : " + ex.ToString());
            }
        }
    }
}
