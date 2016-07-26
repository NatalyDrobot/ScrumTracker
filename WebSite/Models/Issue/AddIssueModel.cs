using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTO.Entities;
using Resources;

namespace WebSite.Models.Issue
{
    public class AddIssueModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ValidationNameRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ValidationNameLength", MinimumLength = 6)]
        [Display(ResourceType = typeof(Resource), Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Description")]
        public string Description { get; set; }

        public Guid? ProjectId { get; set; }
        public Guid? SprintId { get; set; }
        public Guid? IssueTypeId { get; set; }
        public Guid? ParentIssueId { get; set; }
    }
}