using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcAdminResearch.Areas.MvcAdmin.Models
{
    public class ModelInfoDto
    {
        public string ControllerName { get; set; }
        public string DisplayName { get; set; }
        public int RecordsCount { get; set; }
    }
}