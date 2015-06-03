using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hub.Interface.User;
using System.ComponentModel.Composition;
using System.Security.Cryptography;

namespace User
{
    [Export(typeof(IAccountStrategy))]
    public class AccountStrategy : IAccountStrategy
    {
        /// <summary>
        /// 生成MD5|Hash串
        /// </summary>
        /// <param name="input">输入串</param>
        /// <returns>hash串</returns>
        public string CreateMD5HashCode(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
        public Guid CreateAccount(Hub.User user)
        {
            user.UserID = Guid.NewGuid();
            user.Password = CreateMD5HashCode(user.Password);
            user.RegisterTime = DateTime.Now;
            user.Status = 1;
            using (var db = new Hub.ExtensionFrameworkEntities2015())
            {
                db.User.Add(user);
                db.SaveChanges();
            }
            return user.UserID;
        }
        public bool StuNumberRegistered(string stunumber)
        {
            using (var db = new Hub.ExtensionFrameworkEntities2015())
            {
                var result = from o in db.User
                             where o.Status > 0 && o.StuNumber.Equals(stunumber)
                             select o;
                return result.Count() > 0 ? true : false;
            }
        }
        public bool EmailRegistered(string email)
        {
            using (var db = new Hub.ExtensionFrameworkEntities2015())
            {
                var result = from o in db.User
                             where o.Status > 0 && o.Email.Equals(email)
                             select o;
                return result.Count() > 0 ? true : false;
            }
        }
        public bool NickNameRegistered(string nickname)
        {
            using (var db = new Hub.ExtensionFrameworkEntities2015())
            {
                var result = from o in db.User
                             where o.Status > 0 && o.NickName.Equals(nickname)
                             select o;
                return result.Count() > 0 ? true : false;
            }
        }
        public bool UpdateAccount(Hub.User user)
        {
            using (var db = new Hub.ExtensionFrameworkEntities2015())
            {
                var getUser = db.User.Where(x => x.UserID.Equals(user.UserID) && x.Status > 0).First();
                if (getUser != null)
                {
                    getUser.NickName = user.NickName;
                    getUser.Sex = user.Sex;
                    //getUser.Avatar = user.Avatar;
                    getUser.Tel = user.Tel;
                    getUser.TrueName = user.TrueName;
                }
                return db.SaveChanges() > 0;
            }
        }
        public int DeleteAccount(Guid userID)
        {
            using (var db = new Hub.ExtensionFrameworkEntities2015())
            {
                var user = db.User.Where(x => x.UserID == userID && x.Status > 0).FirstOrDefault();
                user.Status = -1;
                return db.SaveChanges();
            }
        }
        public Hub.User GetAccount(Guid userID)
        {
            using (var db = new Hub.ExtensionFrameworkEntities2015())
            {
                var result = (from o in db.User
                              where o.UserID.Equals(userID) && o.Status > 0
                              select o).First();
                return result;
            }
        }
        public string CreateCheckCode(int length)
        {
            Random rand = new Random();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.Append(rand.Next(0, 9));
            }
            return builder.ToString();
        }
        public string GetEmailByStuNum(string stunumber)
        {
            using (var db = new Hub.ExtensionFrameworkEntities2015())
            {
                var result = (from o in db.User
                              where o.StuNumber.Equals(stunumber) && o.Status > 0
                              select o).First();
                return result.Email;
            }
        }
        public string GetNickNameByUserID(Guid userid)
        {
            using (var db = new Hub.ExtensionFrameworkEntities2015())
            {
                var result = (from o in db.User
                              where o.UserID.Equals(userid) && o.Status > 0
                              select o).First();
                return result.NickName;
            }
        }
        public bool UpdatePassword(string stunumber, string password)
        {
            using (var db = new Hub.ExtensionFrameworkEntities2015())
            {
                var getUser = db.User.Where(x => x.StuNumber.Equals(stunumber) && x.Status > 0).First();
                if (getUser != null)
                {
                    getUser.Password = CreateMD5HashCode(password);
                }
                return db.SaveChanges()>0;
            }
        }
    }
}
