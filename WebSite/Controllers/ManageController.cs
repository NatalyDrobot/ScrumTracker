using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Contracts.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Resources;
using WebSite.Controllers.Mappers;
using WebSite.Models;

namespace WebSite.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IManagerUser _managerUser;

        public ManageController(IManagerUser managerUser)
        {
            _managerUser = managerUser;
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            GetMessage(message);

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        private void GetMessage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess
                    ? Resource.ChangingPassword  
                    : message == ManageMessageId.SetPasswordSuccess
                        ? Resource.SetPassword 
                       : message == ManageMessageId.Error
                                ? Resource.ErrorProfile 
                               : message == ManageMessageId.DeleteUsers
                                        ? Resource.ErrorDeleteUser 
                                        : message == ManageMessageId.SaveChanges
                                            ? Resource.UpdateProfile 
                                            : "";
        }

       
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    UserManager.SendEmail (user.Id, "Your password has been changed",user.LastName + "  " +user.Firstname + 
                        " your new password is :    " + model.NewPassword);
                }
                return RedirectToAction("ProfileUser", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }


        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
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

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }
        
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            DeleteUsers,
            SaveChanges,
            Error
        }

#endregion

        [Authorize]
        [HttpGet]
        public ActionResult ProfileUser(ManageMessageId? message)
        {
            GetMessage(message);
            ProfileViewModel model = new ProfileViewModel();
            var user = UserManager.FindById(CurrentUserId);
            GetImageUser(model);
            model.Email = user.Email;
            model.FirstName = user.Firstname;
            model.LastName = user.LastName;
            model.Department = user.Department.Name;
            model.Culture = user.Culture;
            ViewBag.Cultures = new SelectList(new List<string>() {"ru", "en"});

            return View(model);
        }

        private void GetImageUser(ProfileViewModel model)
        {
            var user = UserManager.FindById(CurrentUserId);
            string image = "no-image.png";
            model.Avatar = user.Avatar ?? image;
        }

        [Authorize]
        [HttpPost]
        public ActionResult ProfileUser(ProfileViewModel model, HttpPostedFileBase image)
        {
            GetImageUser(model);
            if (image != null && !image.ContentType.Contains("image/"))
            {
                ModelState.AddModelError(string.Empty, Resource.UnsupportedFile);
                return View(model);
            }

            var user = UserManager.FindById(CurrentUserId);
            
            if (image!=null && user.Avatar != null && user.Avatar!= "no-image.png")
            {
                Util.FileUpload.DeleteFile(user.Avatar);
            }
            if (image != null)
            {
                user.Avatar = Util.FileUpload.UploadFile(image);
            }
            user.Firstname = model.FirstName;
            user.LastName = model.LastName;
            user.Culture = model.Culture;
            var result = UserManager.Update(user);
            if (result.Succeeded)
            {
                return RedirectToAction("ProfileUser", new { Message = ManageMessageId.SaveChanges });
            }
            return RedirectToAction("ProfileUser", new { Message = ManageMessageId.Error });
        }

        [Authorize (Roles = "Admin")]
        [HttpGet]
        public ActionResult ManageUsers()
        {
            var model = new ProfileViewModel();
            var currentUser = UserManager.FindById(CurrentUserId);
            var users = _managerUser.GetAllUsers().Where(u => u.Department!=null && u.Department.Id == currentUser.DepartmentId && u.IsActivate && u.Id!=CurrentUserId).OrderBy(u=>u.Email ).ToList();
            var models = users.ToProfileViewModel();
            return View(models);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = _managerUser.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var model = user.ToProfileViewModel();
            return View(model);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
           // GetMessage(message);
            try
            {
                var user= _managerUser.GetUserById(id);
                if (user.EmailConfirmed == false)
                {
                    _managerUser.RemoveUser(user);
                }
                else
                {
                    user.IsActivate = false;
                    user.EmailConfirmed = false;
                    user.SecurityStamp = Guid.NewGuid().ToString();
                    _managerUser.UpdateUser(user);
                }
                
                return RedirectToAction("ManageUsers");
            }
            catch
            {
                ModelState.AddModelError("",Resource.ErrorDeleteUser);
                return View();
            }
        }
       
       
        [HttpGet]
        public ActionResult FilterUsersByName(string email)
        {
            var currentUser = UserManager.FindById(CurrentUserId);
            var users = _managerUser.FindUsersByEmail(email).Where(u => u.Department?.Id == currentUser.DepartmentId).ToList();
            var models = users.ToProfileViewModel();
           
            return View("ManageUsers", models);

        }
        public ActionResult AutocompleteUsersName(string term)
        {
            var currentUser = UserManager.FindById(CurrentUserId);
            var users = _managerUser.GetAllUsers().Where(u => u.Department?.Id==currentUser.DepartmentId && u.Email.Contains(term))
                            .Select(u => new { value = u.Email }).Distinct();
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        
    }
}