using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Data.Linq;
using System.Security.Cryptography;
using Hub.Models;
using Hub.Interface.User;
using User.cn.edu.swufe.news;
using System.Text;
using System.ComponentModel.Composition;

namespace User
{
    /// <summary>
    /// 用户认证相关
    /// </summary>
    [Export(typeof(IAuthenticationStrategy))]
    public class AuthenticationStrategy : IAuthenticationStrategy
    {
        /// <summary>
        /// 验证学号
        /// </summary>
        /// <param name="stunumber">学号</param>
        /// <param name="password">上网密码</param>
        /// <returns>1成功 0失败</returns>
        public int ValidStuNumber(string stunumber, string password)
        {
            using (getLdapGHYWSDL client = new getLdapGHYWSDL())
            {
                client.Credentials = new System.Net.NetworkCredential("ghy", "mys");
                if ("2000".Equals(client.chg_ldaps(stunumber, password, 1, " ", "ldap1")))
                    return 1;
                else return 0;
            }
        }

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

        /// <summary>
        /// 由TokenCode获取Token记录（包括UserID）
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="checkcode">CheckCode校验码</param>
        /// <returns></returns>
        public Token GetSessionByToken(string token, string checkcode)
        {
            using (var db = new ExtensionFrameworkContext())
            {
                var sessionList = from o in db.Tokens
                                  where o.Status == 1 && o.Expire > DateTime.Now && o.TokenCode.Equals(token) && o.CheckCode.Equals(checkcode)
                                  select o;
                return sessionList.Count() == 0 ? null : sessionList.ToList()[0];
            }
        }
        /// <summary>
        /// 利用邮箱密码获取用户对象
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public Hub.Models.User GetUser(string email, string password)
        {
            using (var db = new ExtensionFrameworkContext())
            {
                var encodePwd = CreateMD5HashCode(password);
                var userList = from o in db.Users
                               where o.Email.Equals(email) && o.Password.Equals(encodePwd)
                               select o;
                if (userList.Count() < 1) return null;
                else return userList.ToList()[0];
            }
        }
        /// <summary>
        /// 由邮箱，密码生成token
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        /// <returns>返回TokenCode</returns>
        public string CreateToken(string email, string password)
        {
            string timestamp = DateTime.Now.ToString();
            string bundle = email + timestamp + password;
            return CreateMD5HashCode(bundle);
        }
        /// <summary>
        /// 生成Token校验码
        /// </summary>
        /// <param name="agent">Request Agent</param>
        /// <param name="ip">用户IP</param>
        /// <returns></returns>
        public string CreateCheckCode(string agent, string ip)
        {
            string bundle = agent + ip;
            return CreateMD5HashCode(bundle);
        }
        /// <summary>
        /// 数据库中插入Token
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="tokenCode"></param>
        /// <param name="checkCode"></param>
        /// <returns></returns>
        public int InsertTokenToDB(Guid userID, string tokenCode, string checkCode)
        {
            Guid tokenID = Guid.NewGuid();
            Hub.Models.Token one = new Token
            {
                TokenID = Guid.NewGuid(),
                UserID = userID,
                TokenCode = tokenCode,
                CheckCode = checkCode,
                Expire = DateTime.Now + TimeSpan.FromDays(30),
                Status = 1
            };
            using (var db = new ExtensionFrameworkContext())
            {
                db.Tokens.Add(one);
                return db.SaveChanges();
            }
        }
        /// <summary>
        /// 令token失效
        /// </summary>
        /// <param name="tokenCode"></param>
        /// <returns></returns>
        public bool DelToken(string tokenCode)
        {
            using (var db = new ExtensionFrameworkContext())
            {
                var getToken = db.Tokens.Where(x => x.TokenCode.Equals(tokenCode) && x.Status > 0).First();
                if (getToken != null)
                {
                    getToken.Status = 0;
                }
                return db.SaveChanges() > 0;
            }
        }
    }
}
