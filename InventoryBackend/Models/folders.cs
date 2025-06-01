using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryBackend.Models
{
    public class folders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int folderID { get; set; }
        public string folderName { get; set; }
        public string descriptionFolder {  get; set; }
        public int userID { get; set; }
        [ForeignKey("userID")]
        public userAccounts userAccounts {  get; set; }

        public folders()
        {
        }
    }
}
