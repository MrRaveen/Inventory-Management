using InventoryBackend.Context;
using InventoryBackend.Models;
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
        public userAccounts createProcess(userAccounts newValues)
        {
            try
            {
                var dateTime = DateTime.Now;
                String shortDateValue = dateTime.ToShortDateString();
                DateTime shortDateValue1 = DateTime.Parse(shortDateValue); 
                newValues.createdDate = shortDateValue1;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());//TODO TEST
                throw new Exception("Error occured when creating the account (service) : " + ex.ToString());
            }
            return null;
        }
    }
}
