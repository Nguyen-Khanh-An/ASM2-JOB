using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FPTJOB.Models
{
    public class Profile
    {
        public int Id { get; set; }       
        public string UserId { get; set; }        
        public string FullName { get; set; }       
        public string Address { get; set; }        
        public string Skill { get; set; }        
        public string Education { get; set; }
        public string MyFile { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [InverseProperty("Profile")]
        public virtual ICollection<ProJob>? ProJobs { get; set; }
    }
}
