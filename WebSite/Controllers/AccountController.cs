using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebSite.Models;
using DTO.Entities;
using BusinessLayer.Contracts.Interfaces;
using Resources;
using BusinessLayer;

namespace WebSite.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
      //  private readonly IManagerDepartament _departamentManager;


        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
           // _departamentManager = departamentManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.RegistrationInProgress = false;
            return View("Register");
        }

        [AllowAnonymous]
        [HttpPost]
        public string Login(string login, string loginPass)
        {

            var user = UserManager.Find(login, loginPass);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    return "";
                }
                else
                {
                    return "Email havn't been confirmed.";
                    // ModelState.AddModelError("", "Email havn't been confirmed.");
                }
            }
            else
            {
                return "Invalid login attempt.";
                // ModelState.AddModelError("", "Invalid login attempt.");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult SendInvite(AccauntMessageId? message)
        {
            GetMessage(message);
            return View();
        }
        public enum AccauntMessageId
        {
            SendInvite,
            SaveChanges,
            Error,
            ErrorAccount
        }
        private void GetMessage(AccauntMessageId? message)
        {
            ViewBag.Message =
                message == AccauntMessageId.SendInvite
                    ? Resource.InviteSentInform
                    : message == AccauntMessageId.SaveChanges
                        ? Resource.ChangingPassword
                        : message == AccauntMessageId.ErrorAccount
                        ? Resource.ErrorUserExist
                            : message == AccauntMessageId.Error
                                ? Resource.ErrorProfile
                                    : "";
        }

        private readonly string _emailConfirmation = "EmailConfirmation";
        [HttpPost]
        public ActionResult SendInvite(InviteViewModel model)
        {
            string title, body, actionName, code;
            var currentUser = UserManager.FindById(CurrentUserId);
            var user = UserManager.FindByEmail(model.Email);
            if (user == null)
            {
                user = new User
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Email,
                    IsActivate = true,
                    DepartmentId = currentUser.DepartmentId,
                    Avatar = "no-image.png",
                    Culture = "en"

                };
                var result = UserManager.Create(user);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "User");
                    code = UserManager.GenerateUserToken(_emailConfirmation, user.Id);
                    actionName = "ConfirmInvite";
                    title = "Your account has invated";
                    body = "Login:    " + model.Email + "   "
                           + "Please confirm your invite by clicking and than enter your data <a href=\"";
                }
                else
                {
                    ModelState.AddModelError("", Resource.InviteError);
                    return View(model);
                }

            }
            else if (user.DepartmentId != currentUser.Department.Id && user.EmailConfirmed == false && user.IsActivate == false)
            {
                user.DepartmentId = currentUser.Department.Id;
                user.IsActivate = true;
                UserManager.Update(user);
                code = UserManager.GenerateEmailConfirmationToken(user.Id);
                actionName = "ConfirmEmail";
                title = "Invite to the department";
                body = user.LastName + "  " + user.Firstname +
                       " You got the invitation in department :   " +
                       currentUser.Department.Name +
                       "<br> Please confirm your invite by clicking and than enter your data <a href=\"";

            }
            else if (user.EmailConfirmed == false && user.IsActivate == false)
            {
                user.IsActivate = true;
                UserManager.Update(user);
                code = UserManager.GenerateEmailConfirmationToken(user.Id);
                actionName = "ConfirmEmail";
                title = "Invite to the department";
                body = user.LastName + "  " + user.Firstname +
                      "  You got the invitation in department :   " +
                      currentUser.Department.Name +
                      "<br> Please confirm your invite by clicking and than enter your data <a href=\"";
            }
            else
            {
                ModelState.AddModelError("", Resource.ErrorUserExist);
                return View(model);
            }
            var callbackUrl = Url.Action(actionName, "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            UserManager.SendEmail(user.Id, title, body + callbackUrl +
                "\">here</a>");
            return RedirectToAction("SendInvite", new { Message = AccauntMessageId.SendInvite });
        }

        [AllowAnonymous]
        public ActionResult ConfirmInvite(string userId, string code)
        {
            var user = UserManager.FindById(userId);

            var isConfirmed = UserManager.VerifyUserToken(userId, _emailConfirmation, code);
            if (isConfirmed)
            {
                var model = new ConfirmInviteViewModel();
                model.Email = user.Email;
                return View(model);
            }
            return View("Error");//TODO
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ConfirmInvite(ConfirmInviteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByEmail(model.Email);
                user.Firstname = model.FirstName;
                user.LastName = model.LastName;
                user.EmailConfirmed = true;
                var result = UserManager.AddPassword(user.Id, model.Password);
                if (result.Succeeded)
                {
                    result = UserManager.Update(user);
                    if (result.Succeeded)
                    {
                        SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View(model);
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.RegistrationInProgress = false;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            ManagerDepartament mngr = new ManagerDepartament();
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        Firstname = model.FirstName,
                        LastName = model.LastName,
                        IsActivate = true,
                        Avatar = "no-image.png",
                        Culture = "en"
                    };
                    var departmentInDb = mngr.GetDepartmentByName(model.Department);


                    if (departmentInDb != null)
                    {
                        ModelState.AddModelError("DepartExist", "This department already exists");
                        ViewBag.RegistrationInProgress = true;
                        return View();
                    }
                    else
                    {
                        var depart = new DepartmentDto { Name = model.Department };
                        Guid id = depart.Id;
                        mngr.AddDepartment(depart);
                                                user.DepartmentId = depart.Id;
                    }
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await UserManager.AddToRoleAsync(user.Id, "Admin");
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId = user.Id, code = code},
                            protocol: Request.Url.Scheme);
                        await UserManager.SendEmailAsync(user.Id, "Confirm your account",
                            "Login:    " + model.Email + "   " + "Your password:   " + model.Password + "   "
                            + "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                        return View("DisplayEmail"); //TODO
                    }
                    AddErrors(result);
                }
                catch (Exception e)
                {
                    Response.Write(string.Format("{0}{1}", e.Message,e.InnerException));
                    
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.RegistrationInProgress = true;
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }


        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<int> ForgotPassword(string email)
        {
            ForgotPasswordViewModel model = new ForgotPasswordViewModel()
            {
                Email = email
            };

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return 0;
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return 1;
            }

            // If we got this far, something failed, redisplay form
            return 0;
        }

        
        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                UserManager.SendEmail(user.Id, "Your password has been changed", user.LastName + "  " + user.Firstname +
                       " your new password is :    " + model.Password);
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

      //
        // POST: /Account/LogOff
        //   [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session["CurrentUser"] = null;
            return RedirectToAction("Register");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        public ActionResult ErrorConfirmInvite() //TODO
        {
            throw new NotImplementedException();
        }
    }
}