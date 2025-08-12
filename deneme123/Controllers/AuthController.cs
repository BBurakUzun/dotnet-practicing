using deneme123.Data;
using deneme123.Dtos;
using deneme123.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using deneme123.Services;

namespace deneme123.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtTokenService jwtTokenService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._jwtTokenService = jwtTokenService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequest req, [FromServices] IValidator<RegisterRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
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
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var result = await _signInManager.PasswordSignInAsync(req.Username, req.Password, false, false);

            if (!result.Succeeded)
                return Unauthorized("Giriş başarısız");

            var user = await _userManager.FindByNameAsync(req.Username);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı");

            var token = await _jwtTokenService.GenerateTokenAsync(user);

            return Ok(new { Token = token});
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Çıkış başarılı" });
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
