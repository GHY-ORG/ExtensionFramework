﻿using System;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Hub.Interface.User;
using System.Web;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Text;
using System.Drawing.Imaging;

namespace GHY_SSO.Controllers
{
    [Export]
    public class UserController : Controller
    {
        [Import]
        public Hub.Interface.User.IAuthenticationStrategy AuthentiationStrategy { set; get; }

        [Import]
        public Hub.Interface.User.IAccountStrategy AccountStrategy { set; get; }

        #region 视图显示
        public ActionResult Login()
        {
            string returnUrl = Request.QueryString["ReturnUrl"];
            ViewBag.ReturnUrl = returnUrl;
            HttpCookie cookie = Request.Cookies["ghy_sso_token"];
            if (cookie != null)
            {
                string token = cookie.Value;
                Hub.Models.Token token_record = AuthentiationStrategy.GetSessionByToken(token, AuthentiationStrategy.CreateCheckCode(Request.UserAgent, Request.UserHostAddress));
                if (token_record != null)
                {
                    Session["User"] = token_record.UserID;
                    //你懂的
                    if (returnUrl != null)
                    {
                        string field = returnUrl.Substring(7).Split('/')[0].ToLower();
                        if (field.Equals("ghy.cn") || field.Equals("ghy.swufe.edu.cn") || field.Equals("www.ghy.cn") || field.Equals("www.ghy.swufe.edu.cn"))
                        {
                            return Redirect(returnUrl + "?token=" + token_record.TokenCode);
                        } 
                    }
                    else
                    {
                        return Redirect(SiteConfig.SiteUrl+"/Home/Index");
                    }
                }
            }
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Forget()
        {
            return View();
        }
        public ActionResult ChangePwd()
        {
            if (Session["StuNumber"] != null)
                return View();
            else
                return Redirect(SiteConfig.SiteUrl+"/User/Forget");
        }
        #endregion

        #region 表单提交
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostLogin(Models.LoginModels model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "请输入有效的邮箱和密码");
                return View("Login", model);
            }
            else
            {
                string returnUrl = Request.QueryString["ReturnUrl"];
                Hub.Models.User user = AuthentiationStrategy.GetUser(model.Email, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "邮箱或密码错误");
                    return View("Login", model);
                }
                else
                {
                    Session["User"] = user.UserID;
                    string token = AuthentiationStrategy.CreateToken(model.Email, model.Password);

                    string token_checkcode = AuthentiationStrategy.CreateCheckCode(Request.UserAgent, Request.UserHostAddress);
                    AuthentiationStrategy.InsertTokenToDB(user.UserID, token, token_checkcode);
                    HttpCookie cookie = new HttpCookie("ghy_sso_token", token);
                    cookie.HttpOnly = true;
                    cookie.Expires = DateTime.Now + TimeSpan.FromDays(30);
                    Response.SetCookie(cookie);

                    //校验returnurl是否为安全地址！
                    if (returnUrl != null)
                    {
                        string field = returnUrl.Substring(7).Split('/')[0].ToLower();
                        if (field.Equals("firewood.ghy.cn") || field.Equals("ghy.cn") || field.Equals("ghy.swufe.edu.cn") || field.Equals("www.ghy.cn") || field.Equals("www.ghy.swufe.edu.cn"))
                        {
                            return Redirect(returnUrl + "?token=" + token);
                        }
                    }
                }
            }
            return Redirect(SiteConfig.SiteUrl+"/Home/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostRegister(Models.RegisterModels model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }
            else
            {
                //验证
                if (AccountStrategy.EmailRegistered(model.Email))
                {
                    ModelState.AddModelError("Email", "邮箱已经被注册");
                    return View("Register", model);
                }
                if (AccountStrategy.StuNumberRegistered(model.StuNumber))
                {
                    ModelState.AddModelError("StuNumber", "学号已经被注册");
                    return View("Register", model);
                }
                Models.CheckCodeModels checkcode = Session["CheckCode"] as Models.CheckCodeModels;
                if (checkcode == null || !checkcode.ValidCheckCode(model.Email, model.CheckCode))
                {
                    ModelState.AddModelError("CheckCode", "验证码错误");
                    return View("Register", model);
                }
                if (!model.Password.Equals(model.EnsurePassord))
                {
                    ModelState.AddModelError("EnsurePassord", "密码不一致");
                    return View("Register", model);
                }
                if (AccountStrategy.NickNameRegistered(model.NickName))
                {
                    ModelState.AddModelError("NickName", "昵称被占用咯~");
                    return View("Register", model);
                }
                /*if (AuthentiationStrategy.ValidStuNumber(model.StuNumber, model.StuPassword) == 0)
                {
                    ModelState.AddModelError("StuPassword", "学号认证失败");
                    return View("Register", model);
                }*/

                //图片不超过5M
                if (file == null && file.ContentLength > 1024 * 1024 * 5)
                {
                    ModelState.AddModelError("", "请上传规定大小的图片");
                    return View("Register", model);
                }

                //验证通过后
                Session.Remove("CheckCode");
                //新用户插入数据库 session赋值
                Hub.Models.User user = new Hub.Models.User
                {
                    UserID = Guid.NewGuid(),
                    Email = model.Email,
                    Password = model.Password,
                    NickName = model.NickName,
                    StuNumber = model.StuNumber
                };

                string absolutePath = SiteConfig.SitePath;
                string path = GetPath(user.UserID.ToString(), "User");
                string fullPath = absolutePath + path;
                string url = path + DateTime.Now.Ticks + ".png";
                using (var stream = file.InputStream)
                {
                    Image img = Image.FromStream(stream);
                    var bmp = ResizeImg(img);
                    if (!System.IO.Directory.Exists(fullPath))
                        System.IO.Directory.CreateDirectory(fullPath);
                    bmp.Save(absolutePath+url, ImageFormat.Png);
                }
                user.Avatar = url;

                Session["User"] = AccountStrategy.CreateAccount(user);
                //token create
                string token = AuthentiationStrategy.CreateToken(model.Email, model.Password);
                string token_checkcode = AuthentiationStrategy.CreateCheckCode(Request.UserAgent, Request.UserHostAddress);
                AuthentiationStrategy.InsertTokenToDB(user.UserID, token, token_checkcode);

                //token cookie
                HttpCookie cookie = new HttpCookie("ghy_sso_token", token);
                cookie.HttpOnly = true;
                cookie.Expires = DateTime.Now + TimeSpan.FromDays(30);
                Response.SetCookie(cookie);
            }
            return Redirect(SiteConfig.SiteUrl+"/Home/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostForget(Models.ForgetModels model)
        {
            if (!ModelState.IsValid)
            {
                return View("Forget", model);
            }
            if (!AccountStrategy.StuNumberRegistered(model.StuNumber))
            {
                return Content("<script>alert('该学号还未注册，请注册~');location.href='Register'</script>");
            }
            if (AuthentiationStrategy.ValidStuNumber(model.StuNumber, model.Password) == 0)
            {
                ModelState.AddModelError("", "学号认证失败");
                return View("Forget", model);
            }
            //验证成功后
            Session["StuNumber"] = model.StuNumber;
            ViewBag.Email = AccountStrategy.GetEmailByStuNum(model.StuNumber);
            return View("ChangePwd");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostChange(Models.ChangePwdModels model)
        {
            string stunumber = Session["StuNumber"].ToString();

            if (!ModelState.IsValid)
            {
                ViewBag.Email = AccountStrategy.GetEmailByStuNum(stunumber);
                return View("ChangePwd", model);
            }
            if (!model.Password.Equals(model.EnsurePassword))
            {
                ModelState.AddModelError("", "密码不一致");
                ViewBag.Email = AccountStrategy.GetEmailByStuNum(stunumber);
                return View("ChangePwd", model);
            }
            if (!AccountStrategy.UpdatePassword(stunumber, model.Password))
            {
                ModelState.AddModelError("", "重置密码异常");
                ViewBag.Email = AccountStrategy.GetEmailByStuNum(stunumber);
                return View("ChangePwd", model);
            }
            return Content("<script>alert('重置密码成功！请登录');window.location.href='Login'</script>");
        }

        [HttpPost]
        public JsonResult GetCheckCode(string email)
        {
            string emailStr = @"([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,5})+";
            Regex emailReg = new Regex(emailStr);
            if (!emailReg.IsMatch(email))
            {
                return Json(new { result = "输入的不是有效的邮件地址格式" });
            }
            if (Session["CheckCode"] != null)
            {
                Models.CheckCodeModels model = (Models.CheckCodeModels)Session["CheckCode"];
                TimeSpan ts = DateTime.Now - model.CreateTime;
                if (ts.TotalSeconds < 60)
                    return Json(new { result = "操作频繁，一分钟后再试" });
            }
            string checkcode = AccountStrategy.CreateCheckCode(6);
            Session["CheckCode"] = new Models.CheckCodeModels { CheckCode = checkcode, CreateTime = DateTime.Now, Email = email };
            //string content = "亲爱的Swufer：\n您的注册验证码为:" + checkcode + "\n--光华园网站--";
            string content = ReplaceText(checkcode);

            Hub.Helper.EmailHelper.SendEmailSSL(new string[] { email }, new string[] { "598260403@qq.com" }, "注册验证码", content, true, "ghyers@ghy.cn", "光华园网站");
            return Json(new { result = "已经将验证码发往邮箱" });
        }
        #endregion

        /// <summary>   
        ///替换邮件html模板中的字段值   
        /// </summary>   
        public string ReplaceText(string checkcode)  
        {  
            string path = string.Empty;

            path = System.Web.HttpContext.Current.Server.MapPath("..\\EmailTemplate\\index.html");   
            System.IO.StreamReader sr = new System.IO.StreamReader(path);  
            string str = sr.ReadToEnd().Replace("$CheckCode$", checkcode);
  
            return str;  
        }

        public ActionResult Exit()
        {
            if (Session["User"] != null)
            {
                Session.Clear();
            }
            if (Request.Cookies["ghy_sso_token"] != null)
            {
                if (!AuthentiationStrategy.DelToken(Request.Cookies["ghy_sso_token"].Value))
                {
                    return Content("<script>alert('token更新异常')</script>");
                }
                HttpCookie myCookie = new HttpCookie("ghy_sso_token");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }

            return Redirect(SiteConfig.SiteUrl+"/User/Login");
        }

        #region 私有成员
        private Bitmap ResizeImg(Image input)
        {
            if (input.Width > 1600 || input.Height > 1600)
            {
                if (input.Width > input.Height)
                {
                    return new Bitmap(input, 1600, input.Height * 1600 / input.Width);
                }
                else
                {
                    return new Bitmap(input, input.Width * 1600 / input.Height, 1600);
                }
            }
            return new Bitmap(input);
        }
        private string GetPath(string name, string foldername)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("GhySsoImages\\");
            sb.Append(foldername);
            sb.Append("\\");
            sb.Append(name);
            sb.Append("\\");
            return sb.ToString();
        }

        #endregion
    }
}