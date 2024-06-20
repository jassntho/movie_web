using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Movie_Web.ModelsView
{
    public class RegisterVM
    {
        public int AccountId {  get; set; }
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage ="Vui long nhap ho ten")]
        public string? FullName { get; set; }
        [Required(ErrorMessage ="Vui long nhap Email")]
        [MaxLength(150)]
        [DataType(DataType.EmailAddress)]
        //[Remote(action: "ValidationEmail", controller: "Accounts")]
        public string? Email { get; set; }
        
      
        //[Remote(action: "ValidationPhone", controller: "Accounts")]
        public string? Password { get; set; }
        [MinLength(5, ErrorMessage = "Ban can dat mat khau it nhat 5 ky tu")]
        [Display(Name = "Nhap lai mat khau")]
        //[Compare("Password", ErrorMessage =("Vui long nhap mat khau giong nhau"))]
        public string? ConfirmPassword {  get; set; }

    }
}
