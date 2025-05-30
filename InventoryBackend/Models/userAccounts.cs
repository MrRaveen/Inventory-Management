using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryBackend.Models
{
    public class userAccounts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userID { get; set; }
        public string userName { get; set; }
        public string password { get; set; }

        [Column(TypeName = "date")]
        public DateTime createdDate { get; set; }
        public userAccounts()
        {
        }
    }
}
