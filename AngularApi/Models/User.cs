using System.ComponentModel.DataAnnotations;

namespace AngularApi.Models
{
    public class User
    {

        [Key]
        public int Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public string  Phone { get; set; }
        public string Confirmpsw { get; set; }

        public string  Token { get; set; }
        public string? ResetPasswordToken {  get; set; }
        public DateTime ResetPasswordExpiry { get; set; }


    }
}
