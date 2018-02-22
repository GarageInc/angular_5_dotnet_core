using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace depot {

    [Route ("api/v1/account")]
    public class AccountController : Controller {

        [HttpPost ("authenticate")]
        public async Task Token ([FromBody] LoginModel model) {
            var user = GetIdentity (model);

            if (user == null) {
                Response.StatusCode = 400;
                await Response.WriteAsync ("Invalid username or password.");
                return;
            }

            var result = await _userManager.CreateAsync (user, model.Password);

            if (result.Succeeded) {
                await _signInManager.SignInAsync (user, false);
            }

            // создаем JWT-токен
            var token = this.GenerateJwtToken (user);

            // сериализация ответа
            Response.ContentType = "application/json";
            await Response.WriteAsync (JsonConvert.SerializeObject (new {
                access_token = token,
                    username = user.Email
            }, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private User GetIdentity (LoginModel model) {
            var hashed = CalculateMD5Hash (model.Password);
            User person = this._context.Users
                .Include (u => u.Role)
                .FirstOrDefault (x => (x.UserName == model.Username || x.Email == model.Username) && (x.Password == hashed || x.Password == model.Password));
            // User person = people.FirstOrDefault (x => x.Login == username && x.Password == password);
            // если пользователя не найдено
            return person;
        }

        private object GenerateJwtToken (User user) {
            var claims = new List<Claim> {
                new Claim (ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim (ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name),
                new Claim (JwtRegisteredClaimNames.Sub, user.Email),
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString ())
            };

            var key = AuthOptions.GetSymmetricSecurityKey ();
            var creds = new SigningCredentials (key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.Add (TimeSpan.FromMinutes (AuthOptions.LIFETIME));

            var token = new JwtSecurityToken (
                AuthOptions.ISSUER,
                AuthOptions.ISSUER,
                claims,
                expires : expires,
                signingCredentials : creds
            );

            return new JwtSecurityTokenHandler ().WriteToken (token);
        }
        public static string CalculateMD5Hash (string input)

        {

            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create ();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes (input);

            byte[] hash = md5.ComputeHash (inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder ();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append (hash[i].ToString ("X2"));

            }

            return sb.ToString ();

        }
        private List<User> people = new List<User> {
            new User {
            UserName = "admin@gmail.com", Password = CalculateMD5Hash ("12345"), Role = new Role {
            Name = "admin"
            }
            },
            new User {
            UserName = "qwerty", Password = CalculateMD5Hash ("55555"), Role = new Role () {
            Name = "user"
            }
            }
        };
        private void DatabaseInitialize () {
            if (!_context.Roles.Any ()) {
                string adminRoleName = "admin";
                string userRoleName = "user";

                string adminEmail = "admin@mail.ru";
                string adminLogin = "admin";
                string adminPassword = "123456";

                // добавляем роли
                Role adminRole = new Role { Name = adminRoleName };
                Role userRole = new Role { Name = userRoleName };

                _context.Roles.Add (userRole);
                _context.Roles.Add (adminRole);

                // добавляем администратора
                _context.Users.Add (new User { Email = adminEmail, UserName = adminLogin, Password = CalculateMD5Hash (adminPassword), Role = adminRole });

                _context.SaveChanges ();
            }
        }

        private DepotContext _context;

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController (DepotContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager) {
            this._context = context;

            _userManager = userManager;
            _signInManager = signInManager;
            DatabaseInitialize ();
        }
    }

}