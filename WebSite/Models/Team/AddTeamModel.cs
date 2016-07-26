using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources;

namespace WebSite.Models.Team
{
    public class AddTeamModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ValidationNameRequired")]
        [Remote("CheckName", "Team", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ValidationNameExists" )]
        [StringLength(9, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TeamValidationLength", MinimumLength = 3)]
        [Display(ResourceType = typeof(Resource), Name = "Name")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Description is required field")]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "Description length must be min 3 characters")]
        [Display(ResourceType = typeof(Resource), Name = "Description")]
        public string Description { get; set; }
    }
}