using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api1.Dtos.Account;
using api1.Models;
using api1.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace api1.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, TokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(s => s.UserName == loginDto.UserName.ToLower());
            if (user == null)
                return Unauthorized("Invalid Username!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return Unauthorized("Username not found and/or password incorrect");

            return Ok(
               new NewUserDto
               {
                   UserName = user.UserName,
                   Email = user.Email,
                   Token = _tokenService.CreateToken(user)
               }
            );

        }




        //Registering User
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var appUser = new AppUser
                {
                    //Requiring both Email and Username
                    UserName = registerDto.UserName,
                    Email = registerDto.Email
                };
                var existingUser = await _userManager.FindByEmailAsync(appUser.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Email is already in use." });
                }

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
                if (createdUser.Succeeded)
                {
                    //Any body who signs through register endpoint will be assigned as User
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

    }
}