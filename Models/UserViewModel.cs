using System.ComponentModel.DataAnnotations;

namespace UserDashboard.Models
{
    public class UserViewModel : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        [MinLength(2)]
        [DisplayAttribute(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [DisplayAttribute(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password does not match")]
        [CompareAttribute("Password")]
        [DisplayAttribute(Name = "Confirm Password")]
        public string PasswordC { get; set; }  
    }
}