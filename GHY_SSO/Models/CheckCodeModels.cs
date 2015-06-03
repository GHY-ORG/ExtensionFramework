using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GHY_SSO.Models
{
    public class CheckCodeModels
    {
        public string Email { set; get; }
        public string CheckCode { set; get; }
        public DateTime CreateTime { set; get; }

        public bool ValidCheckCode(string email,string checkcode)
        {
            if (email.Equals(Email) && checkcode.Equals(CheckCode))
            {
                return true;
            }
            return false;
        }
    }
}