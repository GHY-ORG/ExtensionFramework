using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GHY_SSO.Models
{
    public class LoginModels
    {
        [Display(Name="邮箱")]
        [Required(ErrorMessage="必填")]
        public string Email { set; get; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "必填")]
        public string Password { set; get; }
    }
}