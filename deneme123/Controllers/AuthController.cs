using deneme123.Data;
using deneme123.Dtos;
using deneme123.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace deneme123.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.Username) || string.IsNullOrEmpty(req.Password))
            {
                return BadRequest("Geçersiz veri girişi");
            }

            var user = new ApplicationUser
            {
                UserName = req.Username,
                Email = req.Email
            };
            var result = await _userManager.CreateAsync(user, req.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new { Message = "Kayıt başarılı" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest req)
        {
            var result = await _signInManager.PasswordSignInAsync(req.Username, req.Password, false, false);

            if (!result.Succeeded)
                return Unauthorized("Giriş başarısız");

            return Ok(new { Message = "Giriş başarılı" });
        }



        /*
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] AuthRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.Username) || string.IsNullOrEmpty(req.Password))
            {
                return BadRequest("Geçersiz veri girişi");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == req.Username);
            if (existingUser != null)
            {
                return BadRequest("Username zaten var.");
            }

            var user = new User
            {
                Username = req.Username,
                Password = req.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return Ok(new { Message = "Başarıyla kayıt olundu" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.Username) || string.IsNullOrEmpty(req.Password))
            {
                return BadRequest("Geçersiz veri girişi");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == req.Username && u.Password == req.Password);
            if (user == null)
            {
                return Unauthorized("Kullanıcı adı veya şifre yanlış.");
            }
            return Ok(new { Message = "Giriş başarılı", UserId = user.Id });
        }
        */

    }
}
