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
using Microsoft.EntityFrameworkCore;

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

        [HttpPost]
        public IActionResult SignUp([FromForm] User user)
        {
            var userWithSameEmail = _context.Users.Where(u => u.Email == user.Email).SingleOrDefault();
            if (userWithSameEmail != null)
            {
                return BadRequest("User with same email already exists");
            }

            //Get Last ID In Table Records
            var id = _context.Users
                             .OrderByDescending(x => x.ID)
                             .Take(1)
                             .Select(x => x.ID)
                             .ToList()
                             .FirstOrDefault();

            string File = Convert.ToString(id+1);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + File;

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserImages/", File + ".jpg");

            var userObj = new User()
            {
                UserName = user.UserName,
                FName = user.FName,
                LName = user.LName,
                Address = user.Address,
                Phone = user.Phone,
                Email = user.Email,
                Role = "User",
                Password = SecurePasswordHasherHelper.Hash(user.Password),
                ConfirmPassword = SecurePasswordHasherHelper.Hash(user.Password),
            };

            user.Image.CopyTo(new FileStream(imagePath, FileMode.Create));

            userObj.ImageUrl = imagePath.Remove(0, 7);
            _context.Users.Add(userObj);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
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
            user.UpdateTime = DateTime.Now;
            _context.SaveChanges();
            return Ok("Your password has been changed");
        }

        [HttpPost]
        public IActionResult Login([FromBody] Login objUser)
        {
            var userEmail = _context.Users.FirstOrDefault(u => u.Email == objUser.Email);
            var email = objUser.Email;
            var remove = _context.Users.Where(u => u.Email == email).Select(r => r.IsRemoved).FirstOrDefault();
            if (remove == true)
            {
                return NotFound("User Removed By Admin.");
            }
            
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
            user.UpdateTime = DateTime.Now;
            _context.SaveChanges();
            return Ok("Your Phone Number updated");
        }

        [HttpPost]
        [Authorize]
        public IActionResult EditProfile([FromForm] ChangeProfile imageobj)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                //Get Last ID In Table Records
                var id = _context.Users.Where(e => e.Email == userEmail).Select(i => i.ID).FirstOrDefault();

                string File = Convert.ToString(id);

                string FileName = File;

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + File;

                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserImages/", File + ".jpg");

                imageobj.Images.CopyTo(new FileStream(imagePath, FileMode.Create));

                user.ImageUrl = imagePath.Remove(0, 7);

                user.UpdateTime = DateTime.Now;
                _context.SaveChanges();
                return Ok("Record updated successfully");
            }
        }
    }
}
