using AuthenticationPlugin;
using Domain.Models.Entities.User;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Transactions;
using System.Net.Http.Json;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _repository;
        private ShopDbContext _context;
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        public UserController(IConfiguration configuration, IUserRepository repository, ShopDbContext context)
        {
            _repository = repository;
            _context = context;
            _configuration = configuration;
            _auth = new AuthService(configuration);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _repository.GetUsers();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}", Name = "Get")]
        public IActionResult GetUser(int id)
        {
            var user = _repository.GetUser(id);
            return new OkObjectResult(user);
        }

        [HttpPost]
        public IActionResult SignUp([FromForm] User user)
        {
            var userWithSameEmail = _context.Users.Where(u => u.Email == user.Email).SingleOrDefault();
            if (userWithSameEmail != null)
            {
                return BadRequest("User with same email already exists");
            }
            var userObj = new User()
            {
                FName = null,
                LName = null,
                Address = null,
                Phone = null,
                Email = user.Email,
                Role = "User",
                Password = SecurePasswordHasherHelper.Hash(user.Password),
                ConfirmPassword = SecurePasswordHasherHelper.Hash(user.Password),
            };
            _context.Users.Add(userObj);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut]
        [Authorize]
        public IActionResult CompleteProfile([FromForm] CompleteProfileModel completeProfile)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            var guid = Guid.NewGuid();
            var filepath = Path.Combine("wwwroot", guid + ".jpg");
            var userObj = new User()
            {
                FName = completeProfile.FName,
                LName = completeProfile.LName,
                Address = completeProfile.Address,
                Phone = completeProfile.Phone
            };

            if (userObj.Image != null)
            {
                var filestream = new FileStream(filepath, FileMode.Create);
                user.Image.CopyTo(filestream);
            }
            userObj.ImageUrl = filepath.Remove(0, 7);
            _context.SaveChanges();
            return Ok("Your Profile Updated.");
        }

        [HttpPut]
        public IActionResult ChangePassword([FromBody] ChangePass changePass)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            if (!SecurePasswordHasherHelper.Verify(changePass.OldPassword, user.Password))
            {
                return Unauthorized("Sorry you can't change the password");
            }
            user.Password = SecurePasswordHasherHelper.Hash(changePass.NewPassword);
            _context.SaveChanges();
            return Ok("Your password has been changed");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            _repository.DeleteUser(id);
            return new OkResult();
        }

        [HttpPost]
        public IActionResult Login([FromBody] Login objUser)
        {
            var userEmail = _context.Users.FirstOrDefault(u => u.Email == objUser.Email);
            if (userEmail == null)
            {
                return NotFound();
            }
            if (!SecurePasswordHasherHelper.Verify(objUser.Password, userEmail.Password))
            {
                return Unauthorized();
            }

            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Email, objUser.Email),
               new Claim(ClaimTypes.Email, objUser.Email),
               new Claim(ClaimTypes.Role,userEmail.Role)
            };
            var token = _auth.GenerateAccessToken(claims);
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_id = userEmail.ID
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangePhoneNumber([FromBody] ChangePhoneNum changePhoneNum)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }
            user.Phone = changePhoneNum.Phone;
            _context.SaveChanges();
            return Ok("Your Phone Number updated");
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult EditProfile(int id, [FromForm] ChangeProfile imageobj)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                var guid = Guid.NewGuid();
                var filepath = Path.Combine("wwwroot", guid + ".jpg");
                if (imageobj.Images != null)
                {
                    var filestream = new FileStream(filepath, FileMode.Create);
                    imageobj.Images.CopyTo(filestream);
                    user.ImageUrl = filepath.Remove(0, 7);
                }
                _context.SaveChanges();
                return Ok("Record updated successfully");
            }
        }
    }
}
