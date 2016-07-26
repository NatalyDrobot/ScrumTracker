using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.Contracts.Interfaces;
using DTO.Entities;
using Microsoft.AspNet.Identity;
using WebSite.Util.Filters;

namespace WebSite.Controllers
{
    
    public class BaseController : Controller
    {
       
        protected string CurrentUserId
        {
            get
            {
                return System.Web.HttpContext.Current.User.Identity.GetUserId();                
            }
        }

        protected UserDto CurrentUser
        {
            get
            {
                if (Session["CurrentUser"] == null)
                {
                    var user = new ManagerUser().GetUserById(CurrentUserId);
                    Session["CurrentUser"] = user;
                    return user;
                }
                else
                {
                    return (UserDto) Session["CurrentUser"];
                }
            }
        }
    }
}