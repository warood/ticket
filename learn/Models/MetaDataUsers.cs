using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace learn.Models
{

    [MetadataType(typeof(UsersMetaData))]
    public partial class Users
    {
        //for add new properties
        [Display(Name = "confirm Email")]
        [Required]
        [Compare("Email", ErrorMessage ="Email is not match")]
        public string ConfEmail { get; set; }


        [Display(Name = "confirm Password")]
        [Required]
        [Compare("Password", ErrorMessage = "Password is not match")]
        public string ConfPassword { get; set; }
    }

    public class UsersMetaData
    {
        //for edit properties
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

       [DataType(DataType.EmailAddress)]
       [RegularExpression(@"\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b", ErrorMessage ="This is not Email Format")]
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(15 , MinimumLength = 7 , ErrorMessage ="The password should be between 7-25 characters")]
        [RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W]).{8,64})", ErrorMessage = "The Password is not strong ")]
        public string Password { get; set; }
        public int Status { get; set; }
    }
}