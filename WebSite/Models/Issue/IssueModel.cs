using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO.Entities;
using PagedList;

namespace WebSite.Models.Issue
{
    public class IssueModel
    {
        public IPagedList<IssueDto> Issues { get; set; }
        public Guid? SelectedProjectId { get; set; }
    }
}