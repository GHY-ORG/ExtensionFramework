using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GHY_SSO.Models
{
    public class RegisterModels
    {
        [Display(Name="邮箱")]
        [Required(ErrorMessage="必填")]
        [EmailAddress(ErrorMessage="邮箱格式不正确")]
        public string Email { set; get; }

        [Display(Name="验证码")]
        [Required(ErrorMessage = "必填")]
        [RegularExpression("^[0-9]+$",ErrorMessage="验证码格式不正确")]
        [StringLength(6, ErrorMessage = "验证码格式不正确")]
        public string CheckCode { set; get; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "必填")]
        [MinLength(6, ErrorMessage = "密码不少于6位")]
        [MaxLength(20, ErrorMessage = "密码不超过20位")]
        public string Password { set; get; }

        [Display(Name = "确认密码")]
        [Required(ErrorMessage = "必填")]
        public string EnsurePassord { set; get; }

        [Display(Name = "昵称")]
        [Required(ErrorMessage = "必填")]
        [MinLength(3, ErrorMessage = "昵称长度不少于3位")]
        [MaxLength(12, ErrorMessage = "昵称长度不超过12位")]
        [RegularExpression("^[\u4E00-\u9FA5A-Za-z0-9_]+$", ErrorMessage = "昵称只能为汉字、英文、数字和下划线")]
        public string NickName { set; get; }

        [Display(Name = "学号")]
        [Required(ErrorMessage = "必填")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "学号格式不正确")]
        public string StuNumber { set; get; }

        [Display(Name = "上网密码")]
        [Required(ErrorMessage = "必填")]
        public string StuPassword { set; get; }
    }
}