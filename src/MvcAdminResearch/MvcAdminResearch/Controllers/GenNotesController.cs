using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcAdminResearch.Models;
using MvcAdminResearch.Areas.MvcAdmin.Controllers;

namespace MvcAdminResearch.Controllers
{
    public class GenNotesController : GenericController<Note, NotesappContext>
    {
    }
}