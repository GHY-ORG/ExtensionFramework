using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hub.Interface.User;
using Hub.Models;
using System.ComponentModel.Composition;
using System.Security.Cryptography;

namespace User
{
    [Export(typeof(IAuthorizeStrategy))] 
    public class AuthorizeStrategy : IAuthorizeStrategy
    {
        public bool Authorize(Guid UserID, int RoleID)
        {
            using (var db = new ExtensionFrameworkContext())
            {
                User_Role user_role = new User_Role();
                user_role.UserID = UserID;
                user_role.RoleID = RoleID;
                user_role.Status = 1;
                db.User_Role.Add(user_role);
                return db.SaveChanges() > 0;
            }
        }

        public bool AddRole(string RoleName)
        {
            using (var db = new ExtensionFrameworkContext())
            {
                Role role = new Role();
                role.RoleName = RoleName;
                role.Status = 1;
                db.Roles.Add(role);
                return db.SaveChanges() > 0;
            }
        }

        public int GetRoleID(string RoleName)
        {
            using (var db = new ExtensionFrameworkContext())
            {
                var result = (from o in db.Roles
                              where o.RoleName.Equals(RoleName) && o.Status > 0
                              select o.RoleID).FirstOrDefault();
                return result;
            }
        }

        public string[] GetRoleName(int[] RoleIDList)
        {
            string[] rolenameList = new string[RoleIDList.Length];
            using (var db = new ExtensionFrameworkContext())
            {
                for (int i = 0; i < RoleIDList.Length; i++)
                {
                    var result = (from o in db.Roles
                                  where o.RoleID == RoleIDList[i] && o.Status > 0
                                  select o).First();
                    rolenameList[i] = result.RoleName;
                }
                return rolenameList;
            }
        }

        public int[] GetRole(Guid userid)
        {
            using (var db = new ExtensionFrameworkContext())
            {
                var result = (from o in db.User_Role
                              where o.UserID.Equals(userid) && o.Status > 0
                              select o.RoleID);
                if (result.Count() > 0) return result.ToArray<int>();
                return null;
            }
        }

        public bool DelRole(int RoleID)
        {
            using (var db = new ExtensionFrameworkContext())
            {
                var role = db.Roles.Where(x => x.RoleID == RoleID && x.Status > 0).First();
                if (role != null)
                {
                    role.Status = -1;
                }
                return db.SaveChanges() > 0;
            }
        }

        public bool DelUserRole(Guid UserID, int RoleID)
        {
            using (var db = new ExtensionFrameworkContext())
            {
                var list = db.User_Role.Where(x => x.UserID.Equals(UserID) && x.RoleID == RoleID && x.Status > 0);
                foreach (var item in list)
                {
                    item.Status = -1;
                }
                return db.SaveChanges() > 0;
            }
        }
    }
}
