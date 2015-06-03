using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Interface.User
{
    public interface IAccountStrategy
    {
        /// <summary>
        /// 学号是否已经注册
        /// </summary>
        /// <param name="stunumber"></param>
        /// <returns></returns>
        bool StuNumberRegistered(string stunumber);
        /// <summary>
        /// 邮箱是否已经注册
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool EmailRegistered(string email);
        /// <summary>
        /// 生成指定长度数字验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        string CreateCheckCode(int length);
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Guid CreateAccount(Hub.User user);
        /// <summary>
        /// 用户更新
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool UpdateAccount(Hub.User user);
        /// <summary>
        /// 用户删除
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        int DeleteAccount(Guid userID);
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Hub.User GetAccount(Guid userID);
        /// <summary>
        /// 验证昵称是否占用
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        bool NickNameRegistered(string nickname);
        /// <summary>
        /// 通过学号获取邮箱
        /// </summary>
        /// <param name="stunumber"></param>
        /// <returns></returns>
        string GetEmailByStuNum(string stunumber);
        /// <summary>
        /// 通过UserID获取昵称
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        string GetNickNameByUserID(Guid userid);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="stunumber"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool UpdatePassword(string stunumber, string password);
    }
}
