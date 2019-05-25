using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reminder.Data;
using reminder.Entities;
using reminder.Models;
using System;
using System.Collections.Generic;

namespace reminder.Contollers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/notes")]
    [ApiController]
    public class NotesController : Controller
    {
        private readonly NotesRepository notesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NotesController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this.notesRepository = unitOfWork.Notes;
        }

        [HttpGet]
        public IActionResult GetNotes()
        {
            var notes = notesRepository.GetList();
            return Ok(new BaseApiResponse<IEnumerable<Note>> { Success = true, Data = notes, Message = ApiMessages.SuccessfullyFetched });
        }

        [HttpPost]
        public IActionResult Create([FromBody] Note note)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest();

            notesRepository.Add(note);
            _unitOfWork.Complete();

            return Ok(new BaseApiResponse<Note> { Success = true, Data = note, Message = ApiMessages.SuccessfullyCreated });
        }


        [HttpPut]
        public IActionResult Update([FromBody] Note note)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _unitOfWork.Notes.Update(note);
            _unitOfWork.Complete();

            return Ok(new BaseApiResponse<Note> { Success = true, Data = note, Message = ApiMessages.SuccessfullyUpdated });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var note = _unitOfWork.Notes.Get(id);
            if (note == null)
                return NotFound();

            _unitOfWork.Notes.Remove(note);

            _unitOfWork.Complete();
            return Ok(new BaseApiResponse<Note> { Success = true, Data = null, Message = ApiMessages.SuccessfullyDeleted });
        }
    }
}