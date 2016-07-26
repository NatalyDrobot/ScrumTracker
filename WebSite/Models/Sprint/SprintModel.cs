using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebSite.Models.Issue;

namespace WebSite.Models.Sprint
{
    public class SprintModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string NameProject { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "ValidationNameRequired")]
        [Remote("CheckName", "Sprint", AdditionalFields = "Id, ProjectId", ErrorMessageResourceType = typeof(Resources.Resource), 
            ErrorMessageResourceName = "ValidationNameExists")]        
        [StringLength(30, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources.Resource), 
            ErrorMessageResourceName = "ValidationNameLength")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resources.Resource))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "ValidationDateRequired")]
        [Display(Name = "Begin", ResourceType = typeof(Resources.Resource))]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateBegin { get; set; } 

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "ValidationDateRequired")]
        [Display(Name = "End", ResourceType = typeof(Resources.Resource))]
        [Remote("CheckDate", "Sprint", AdditionalFields = "DateBegin, ProjectId, Id",
            ErrorMessageResourceType = typeof(Resources.Resource),ErrorMessageResourceName = "ValidationCheckDate")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateEnd { get; set; }

        public int State { get; set; }
        public string StateName { get; set; }

        public int CountAllIssues { get; set; }
        public int CountIssues { get; set; }
        public int CountTaskOpen { get; set; }
        public int CountTaskInProgess { get; set; }
        public int CountTaskFixed { get; set; }
        public int CountTaskVerified { get; set; }

    }
}