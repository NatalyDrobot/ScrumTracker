using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebSite.Models.Project
{
    public class ProjectModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "ValidationNameRequired")]
        [Remote("CheckName", "Project", AdditionalFields = "Id", ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "ValidationNameExists")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "ValidationNameLength")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "ValidationKeyRequired")]
        [StringLength(10, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "ValidationKeyLength")]
        [Display(Name = "Key", ResourceType = typeof(Resources.Resource))]
        public string Code { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resources.Resource))]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Created", ResourceType = typeof(Resources.Resource))]
        public System.DateTime DateCreate { get; set; }

        [Required]
        public Guid? StateProjectsId { get; set; }
        public string NameStateProject { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "ErrorCreatePtojectTeamId")]
        //[Remote("CheckTeam", "Project", AdditionalFields = "TeamId", ErrorMessageResourceType = typeof(Resources.Resource),
        //    ErrorMessageResourceName = "ErrorCreatePtojectTeamId")]
        public Guid? TeamId { get; set; }

        public string NameTeamProject { get; set; }

        public int CountSprints { get; set; }
        public int CountIssues { get; set; }
        public int CountMembers { get; set; }
        public Guid DepartmentId { get; set; }
    }
}