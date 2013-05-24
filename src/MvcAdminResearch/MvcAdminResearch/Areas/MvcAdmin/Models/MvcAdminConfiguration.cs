using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcAdminResearch.Areas.MvcAdmin.Models
{
    public class MvcAdminConfiguration
    {
        public DbContext Context { get; set; }
    }
}