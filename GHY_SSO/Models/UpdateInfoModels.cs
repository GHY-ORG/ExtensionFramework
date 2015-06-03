using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GHY_SSO.Models
{
    public class UpdateInfoModels
    {
        [Display(Name = "昵称")]
        [Required(ErrorMessage = "昵称必填")]
        [MinLength(3, ErrorMessage = "昵称长度不少于3位")]
        [MaxLength(12, ErrorMessage = "昵称长度不超过12位")]
        [RegularExpression("^[\u4E00-\u9FA5A-Za-z0-9_]+$", ErrorMessage = "昵称只能为汉字、英文、数字和下划线")]
        public string NickName { set; get; }

        [Display(Name = "手机号")]
        [Required(ErrorMessage = "手机号必填")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "请填写正确格式手机号")]
        [RegularExpression("[0-9]+")]
        [MinLength(6)]
        [MaxLength(13)]
        public string Tel { set; get; }

        [Display(Name = "性别")]
        [Required(ErrorMessage = "性别必填")]
        public int Sex { set; get; }

        [Display(Name = "真实姓名")]
        [MaxLength(20)]
        [RegularExpression("^[\u4E00-\u9FA5A-Za-z0-9_]+$", ErrorMessage = "真实姓名只能为汉字、英文、数字和下划线")]
        public string TrueName { set; get; }
    }
}