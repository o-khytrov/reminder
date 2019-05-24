using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reminder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reminder.Contollers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/notes")]
    [ApiController]

    public class NotesController : Controller
    {
        [HttpGet]
        public IActionResult GetNotes()
        {
            return Ok(new List<Note> { new Note { Title = "Make call", Palce = "London", DateTime = DateTime.Now.AddDays(1) } });
        }
    }
}
