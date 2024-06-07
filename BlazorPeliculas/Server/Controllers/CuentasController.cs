using BlazorPeliculas.Shared.DTO;
using BlazorPeliculas.Shared.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        //A través de UserManager podemos crear usuarios
        public CuentasController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("crear")]
        public async Task<ActionResult<UserTokenDTO>> CreateUser([FromBody] LogInDTO model)
        {
            var usuario = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var resultado = await userManager.CreateAsync(usuario, model.Password);

            if (resultado.Succeeded)
            {
                return await BuildToken(model);
            }
            else
            {
                return BadRequest(resultado.Errors.First());
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserTokenDTO>> Login([FromBody] LogInDTO model)
        {
            var resultado = await signInManager.PasswordSignInAsync(model.Email,
                model.Password, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                var roles = await userManager.GetRolesAsync(user);

                if (roles.Contains("admin"))
                {
                    await signInManager.SignOutAsync();
                    return BadRequest("Usuario no encontrado");
                }
                return await BuildToken(model);
            }
            else
            {
                return BadRequest("Usuario y/o contraseña incorrecto");
            }
        }

        [HttpPost("adminlogin")]
        public async Task<ActionResult<UserTokenDTO>> AdminLogin([FromBody] LogInDTO model)
        {
            var resultado = await signInManager.PasswordSignInAsync(model.Email,
                model.Password, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                var roles = await userManager.GetRolesAsync(user);

                if (!roles.Contains("admin"))
                {
                    await signInManager.SignOutAsync();
                    return BadRequest("Sólo los administradores pueden iniciar sesión a través de este portal.");
                }

                return await BuildToken(model);
            }
            else
            {
                return BadRequest("Usuario y/o contraseña incorrecto");
            }
        }

        //Renovación de token
        [HttpGet("renovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserTokenDTO>> Renovar()
        {
            var userInfo = new LogInDTO()
            {
                Email = HttpContext.User.Identity.Name
            };
            return await BuildToken(userInfo);
        }

        //Este método permite crear un json web token a partir de lo que sea
        private async Task<UserTokenDTO> BuildToken(LogInDTO userInfo)
        {
            // aquí no podemos colocar informacíón sensible
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userInfo.Email),
            };

            var usuario = await userManager.FindByEmailAsync(userInfo.Email);
            var roles = await userManager.GetRolesAsync(usuario);

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtkey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(72);

            var token = new JwtSecurityToken(

                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
                );

            return new UserTokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
            };
        }
    }
}
