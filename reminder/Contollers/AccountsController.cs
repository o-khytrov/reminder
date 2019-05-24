using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using reminder.Data;
using reminder.Entities;
using reminder.Helpers;
using reminder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace reminder.Contollers
{
    
    [Route("api/account")]
    public class AccountsController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public AccountsController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;

            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var result = new RegistrationResult
                {
                    Success = false,
                    Messages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray()
                };
                return Ok(result);

            }

            var userIdentity = new ApplicationUser
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber

            };
            try
            {
                var result = await _userManager.CreateAsync(userIdentity, model.Password);

                if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {

                return Ok(new RegistrationResult { Success = false, Messages = new string[] { ex.Message } });
            }


            return Ok(new { Success = true });
        }

        public IActionResult Get()
        {
            var currentUser = Queryable.SingleOrDefault<ApplicationUser>(_unitOfWork.Users, u => u.UserName == _userManager.GetUserId(User));
            return Ok(new ApplicationUserDto
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                MiddleName = currentUser.MiddleName,
                ImageFile = currentUser.ImageFile
            });
        }

        [HttpPut]
        public IActionResult Update([FromBody]ApplicationUserDto model)
        {
            var currentUser = Queryable.SingleOrDefault<ApplicationUser>(_unitOfWork.Users, u => u.UserName == _userManager.GetUserId(User));
            currentUser.FirstName = model.FirstName;
            currentUser.LastName = model.LastName;
            currentUser.MiddleName = model.MiddleName;
            currentUser.ImageBinary = model.ImageBinary;
            currentUser.ImageFile = model.ImageFile;
            currentUser.Email = model.Email;
            _unitOfWork.CompleteAsync();
            return Ok(model);

        }

    }
}
      


