using System;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace WebSite.Models.Team
{
    public class EditTeamModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ValidationNameRequired")]
        [StringLength(9, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ValidationNameLength", MinimumLength = 3)]
        [Display(ResourceType = typeof(Resource), Name = "Name")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Description is required field")]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "Description length must be min 3 characters")]
        [Display(ResourceType = typeof(Resource), Name = "Description")]
        public string Description { get; set; }
    }
}