using BookManager.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BookManager.Controllers
{
    public class AccountController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        //Registration Post Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified, ActivationCode, ResetPasswordCode")] User user)
        {
            bool Status = false;
            string message = string.Empty;
            //Model Validation
            if (ModelState.IsValid)
            {
                //Email is already exist
                var isExist = IsEmailExist(user.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                //Generate Activation Code
                user.ActivationCode = Guid.NewGuid();
                //Password Hashing
                user.FirstName = user.FirstName;
                user.LastName = user.LastName;
                user.Username = user.FirstName + " " + user.LastName;
                user.Passcode = user.Password;
                user.Photo = "~/Uploads/" + "NoImage.png";
                user.RegDate = DateTime.Now.Date;
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                user.IsEmailVerified = false;
                //Save Data to Database
                db.Users.Add(user);
                db.SaveChanges();
                //Send Email to User
                SendVerificationLinkEmail(user.Email, user.ActivationCode.ToString());
                message = "Registration successfully done. Account activation link " +
                    "has been sent to your Email Address: " + user.Email;
                Status = true;
            }
            else
            {
                message = "Invalid Request!";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }
        // Verify Account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            var v = db.Users.Where(x => x.ActivationCode == new Guid(id)).FirstOrDefault();
            if (v != null)
            {
                v.IsEmailVerified = true;
                db.SaveChanges();
                Status = true;
                ViewBag.Message = "Your account has been successfully activated.";
            }
            else
            {
                ViewBag.Message = "Invalid Request";
            }
            ViewBag.Status = Status;

            return View();
        }
        //Login
        [HttpGet]
        public ActionResult Login() 
        {
            return View();
        }
        //Login post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl ="") 
        {
            string message = "";
            ViewBag.Message = message;
            var v = db.Users.Where(x=>x.Email == login.Email).FirstOrDefault();
            if(v!=null)
            {
                if(string.Compare(Crypto.Hash(login.Password),v.Password)==0)
                {
                    int timeout = login.RememberMe ? 43200 : 7200;
                    var ticket = new FormsAuthenticationTicket(login.Email, login.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly= true;
                    Response.Cookies.Add(cookie);

                    if(Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            else
            {
                message = "Invalid credential provided";
            }

            return View();
        }
        //Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Account");
        }

        [NonAction]
        public bool IsEmailExist(string email)
        {
            var v = db.Users.Where(a=>a.Email == email).FirstOrDefault();
            return v != null;
        }
        [NonAction]
        public void SendVerificationLinkEmail(string email, string activationCode, string emailFor = "VerifyAccount" )
        {
            var verifyUrl = "/Account/"+emailFor+"/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var fromEmail = new MailAddress("sssk08844@gmail.com", "BookManager");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "sk01627428269";
            string subject = "";
            string body = "";
            if(emailFor=="VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>Your 'BookManager' account is successfully created." +
                    "Please click on the below link to verify your account" +
                    "<br/><br/><br/><br/><a href = '" + link + "'>" + link + "</a>";
            }
            else if(emailFor=="ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi, <br/> We got request for resetting your password. <br/> " +
                    "Please click on the below link" + "<br/> <a href=" + link + ">Reset Here</a>";
            }
            
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl= true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials= false,
                Credentials = new NetworkCredential(fromEmail.Address, "fqjvvrgwxwafbdbf")
            };
            using(var message = new MailMessage(fromEmail,toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })
          smtp.Send(message);
        }
        [HttpGet]
        //Forgot Password
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            //Verify Email Address
            //Generate Reset Password Link
            //Send Email
            string message = string.Empty;
            bool status = false;
            var account = db.Users.Where(x=>x.Email==Email).FirstOrDefault();
            if (account!=null)
            {
                //Send Email for reset password
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                account.ResetPasswordCode= resetCode;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                status= true;
                message = "Password reset link has successfully been sent.";
            }
            else
            {
                message = "Account not found!";
            }
            ViewBag.Message = message;
            return View();
        }
        public ActionResult ResetPassword(string id)
        {
            //Verify password link
            //Find Account
            //Redirct to reset
            var user = db.Users.Where(x=>x.ResetPasswordCode==id).FirstOrDefault();
            if (user!=null)
            {
                ResetPasswordModel model= new ResetPasswordModel();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = string.Empty;
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(x=>x.ResetPasswordCode==model.ResetCode).FirstOrDefault();
                if (user!=null)
                {
                    user.Password = Crypto.Hash(model.NewPassword);
                    user.ResetPasswordCode = "";
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    message = "Password updated successfully.";
                }
                else
                {
                    message = "Error occurred!";
                }
            }
            else
            {
                message = "Error occurred!";
            }
            ViewBag.Message = message;

            return View(model);
        }
    }
}