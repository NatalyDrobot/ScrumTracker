using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Resources;

namespace WebSite.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (Resource), ErrorMessageResourceName = "ErrorLengthPassword", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resource), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resource), Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof (Resource), ErrorMessageResourceName = "ErrorComparePassword")]
        public string ConfirmPassword { get; set; }
    }
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resource), Name = "CurrentPassword")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (Resource), ErrorMessageResourceName = "ErrorLengthPassword", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resource), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resource), Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof (Resource), ErrorMessageResourceName = "ErrorComparePassword")]
        public string ConfirmPassword { get; set; }
    }
    
    public class ProfileViewModel
    {
       
        public string Id { get; set; }
        [Required]
        [Display(ResourceType = typeof (Resource), Name = "FirstName")]
        public string FirstName { get; set; }
        [Required]
        [Display(ResourceType = typeof (Resource), Name = "LastName")]
        public string LastName { get; set; }
        public string Email { get; set; }
        [Required]
        [Display(ResourceType = typeof (Resource), Name = "Department")]
        public string Department { get; set; }
        public string Avatar { get; set; }
        public bool EmailConfirmed { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "Language")]
        public string Culture { get; set; }
    }

}