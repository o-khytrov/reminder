using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using reminder.Auth;
using reminder.Entities;
using reminder.Helpers;
using reminder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace reminder.Contollers
{
    [Route("api/auth")]
    public class BaseAuthController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly JwtIssuerOptions _jwtOptions;
        private ApplicationUser authorizedUser;

        public BaseAuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;


            CheckRoles();
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        private async Task CheckRoles()
        {
            var roleCheck = await _roleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //create the roles and seed them to the database    
                var roleResult = await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

        }
        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]CredentialsViewModel credentials)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
                if (identity == null)
                {
                    return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
                }
                var userRoles = await _userManager.GetRolesAsync(authorizedUser);
                var claims = new List<Claim>();
                foreach (var userRole in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                    var role = await _roleManager.FindByNameAsync(userRole);
                    if (role != null)
                    {
                        var roleClaims = await _roleManager.GetClaimsAsync(role);
                        foreach (Claim roleClaim in roleClaims)
                        {
                            claims.Add(roleClaim);
                        }
                    }
                }
                // Serialize and return the response
                var response = new
                {

                    id = identity.Claims.Single(c => c.Type == "id").Value,
                    auth_token = await _jwtFactory.GenerateEncodedToken(credentials.UserName, identity, claims),
                    expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                  
                };

                var json = JsonConvert.SerializeObject(response, _serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }

        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                // get the user to verifty
                var userToVerify = await _userManager.FindByNameAsync(userName);

                if (userToVerify != null)
                {
                    // check the credentials  
                    if (await _userManager.CheckPasswordAsync(userToVerify, password))
                    {
                        authorizedUser = userToVerify;
                        return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
                    }
                }
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        [Authorize]
        [HttpPost("password")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordViewModel model)
        {
            var userName = _userManager.GetUserId(User);

            var user = await _userManager.FindByNameAsync(userName);

            var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

            return Ok();
        }
    }
}
