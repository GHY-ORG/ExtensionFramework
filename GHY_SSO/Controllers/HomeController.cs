using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;
using System.ComponentModel.Composition;

namespace GHY_SSO.Controllers
{
    [Export]
    public class HomeController : Controller
    {
        [Import]
        public Hub.Interface.User.IAuthenticationStrategy AuthentiationStrategy { set; get; }

        [Import]
        public Hub.Interface.User.IAccountStrategy AccountStrategy { set; get; }

        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                Hub.User user = AccountStrategy.GetAccount(new Guid(Session["User"].ToString()));
                ViewBag.StuNumber = user.StuNumber;
                ViewBag.Email = user.Email;
                ViewBag.NickName = user.NickName;
                ViewBag.Tel = user.Tel;
                ViewBag.Sex = user.Sex;
                ViewBag.TrueName = user.TrueName;
                return View();
            }
            else
                return Redirect("~/User/Login");
        }

        public ActionResult Application()
        {
            if (Session["User"] != null)
            {
                Hub.User user = AccountStrategy.GetAccount(new Guid(Session["User"].ToString()));
                HttpCookie cookie = Request.Cookies["ghy_sso_token"];
                ViewBag.NickName = user.NickName;
                ViewBag.Token = cookie.Value;
                return View();
            }
            else return Redirect("~/User/Login");
        }

        public ActionResult Message()
        {
            if (Session["User"] != null)
            {
                Hub.User user = AccountStrategy.GetAccount(new Guid(Session["User"].ToString()));
                ViewBag.NickName = user.NickName;
                return View();
            }
            else return Redirect("~/User/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostUpdateInfo(Models.UpdateInfoModels model)
        {
            Hub.User user = AccountStrategy.GetAccount(new Guid(Session["User"].ToString()));
            ViewBag.StuNumber = user.StuNumber;
            ViewBag.Email = user.Email;
            ViewBag.NickName = user.NickName;
            ViewBag.Tel = user.Tel;
            ViewBag.Sex = user.Sex;
            ViewBag.TrueName = user.TrueName;

            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            else
            {
                //从Session获取Guid
                Guid userid = new Guid(Session["User"].ToString());
                //原来的NickName
                string nickname = AccountStrategy.GetNickNameByUserID(userid);
                //如果更改了昵称并且被占用的话..
                if (!model.NickName.Equals(nickname) && AccountStrategy.NickNameRegistered(model.NickName))
                {
                    ModelState.AddModelError("NickName", "昵称被占用咯~");
                    return View("Index", model);
                }
                //更新用户信息
                Hub.User user2 = new Hub.User
                {
                    UserID = userid,
                    NickName = model.NickName,
                    Sex = model.Sex,
                    Tel = model.Tel,
                    TrueName = model.TrueName
                };
                if (!AccountStrategy.UpdateAccount(user2))
                {
                    return Content("<script>alert('完善信息异常！');window.location.href='Index'</script>");
                }
                return Content("<script>alert('完善信息成功！');window.location.href='Index'</script>");
            }
        }
    }
}