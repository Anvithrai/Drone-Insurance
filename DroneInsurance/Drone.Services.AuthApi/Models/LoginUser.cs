using System.ComponentModel.DataAnnotations;

namespace Drone.Services.AuthApi.Models
{
    public class LoginUser
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }    
    }
}
