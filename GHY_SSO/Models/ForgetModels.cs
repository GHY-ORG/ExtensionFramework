using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GHY_SSO.Models
{
    public class ForgetModels
    {
        [Display(Name="学号")]
        [Required(ErrorMessage="学号必填")]
        [RegularExpression("^[0-9]+$",ErrorMessage="学号格式错误")]
        public string StuNumber { set; get; }

        [Display(Name = "上网密码")]
        [Required(ErrorMessage="上网密码必填")]
        public string Password { set; get; }
    }
}