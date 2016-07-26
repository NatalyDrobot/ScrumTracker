using DTO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class DashBoardView
    {

        public string ProjectName { get; set; }

        public string SprintName { get; set; }

        public Guid ProjectId { get; set; }      

        public Guid SprintId { get; set; }

        public int StoryOpen { get; set; }

        public int TaskInProgress { get; set; }

        public int StoryDone { get; set; }

        public DateTime EndDate { get; set; }

        public IEnumerable<IssueDto> IssueList { get; set; }
              
    }

    public class BurnDownChartView
    {
        public IEnumerable<string> SprintDates { get; set; }
        public IEnumerable<double> IdealTasks { get; set; }
        public IEnumerable<int> ActualTasks { get; set; }
    }
}