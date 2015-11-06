using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Interface.User
{
    public interface IAuthorizeStrategy
    {
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="RoleID">权限ID</param>
        /// <returns></returns>
        bool Authorize(Guid UserID, int RoleID);

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        bool AddRole(string RoleName);

        /// <summary>
        /// 获得权限ID
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        int GetRoleID(string RoleName);

        /// <summary>
        /// 获取权限名称
        /// </summary>
        /// <param name="RoleIDList"></param>
        /// <returns></returns>
        string[] GetRoleName(int[] RoleIDList);

        /// <summary>
        /// 获取用户的权限
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        int[] GetRole(Guid UserID);

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        bool DelRole(int RoleID);

        /// <summary>
        /// 删除用户的权限
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        bool DelUserRole(Guid UserID, int RoleID);
    }
}
