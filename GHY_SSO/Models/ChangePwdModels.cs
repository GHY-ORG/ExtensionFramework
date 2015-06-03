using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GHY_SSO.Models
{
    public class ChangePwdModels
    {
        [Display(Name="新密码")]
        [Required(ErrorMessage="必填")]
        [MinLength(6, ErrorMessage = "密码不少于6位")]
        [MaxLength(20, ErrorMessage = "密码不超过20位")]
        public string Password { set; get; }

        [Display(Name = "确认密码")]
        [Required(ErrorMessage = "必填")]
        public string EnsurePassword { set; get; }
    }
}