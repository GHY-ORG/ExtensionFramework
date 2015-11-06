using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Interface.User
{
    public interface IAuthenticationStrategy
    {
        /// <summary>
        /// 验证学号
        /// </summary>
        /// <param name="stunumber">学号</param>
        /// <param name="password">上网密码</param>
        /// <returns>1成功 0失败</returns>
        int ValidStuNumber(string stunumber, string password);

        /// <summary>
        /// 生成MD5|Hash串
        /// </summary>
        /// <param name="input">输入串</param>
        /// <returns>hash串</returns>
        string CreateMD5HashCode(string input);

        /// <summary>
        /// 由TokenCode获取Token记录（包括UserID）
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="checkcode">CheckCode校验码</param>
        /// <returns></returns>
        Hub.Models.Token GetSessionByToken(string token, string checkcode);
        /// <summary>
        /// 利用邮箱密码获取用户对象
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Hub.Models.User GetUser(string email, string password);
        /// <summary>
        /// 由邮箱，密码生成token
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        /// <returns>返回TokenCode</returns>
        string CreateToken(string email, string password);
        /// <summary>
        /// 生成Token校验码
        /// </summary>
        /// <param name="agent">Request Agent</param>
        /// <param name="ip">用户IP</param>
        /// <returns></returns>
        string CreateCheckCode(string agent, string ip);
        /// <summary>
        /// 数据库中插入Token
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="tokenCode"></param>
        /// <param name="checkCode"></param>
        /// <returns></returns>
        int InsertTokenToDB(Guid userID, string tokenCode, string checkCode);
        /// <summary>
        /// 令token失效
        /// </summary>
        /// <param name="tokenCode"></param>
        /// <returns></returns>
        bool DelToken(string tokenCode);
    }
}
