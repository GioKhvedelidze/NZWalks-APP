using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenRepository _tokenRepo;

    public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepo)
    {
        _userManager = userManager;
        _tokenRepo = tokenRepo;
    }
    
    [HttpPost]
    [Route("Register")]
    [ValidateModel]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Username,
            
        };

       var identityResult =  await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

       if (identityResult.Succeeded)
       {
           // Add Roles
           if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
           {
              identityResult =  await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

              if (identityResult.Succeeded)
              {
                  return Ok("Registration Is successfully!");
              }
           }
           
       }
       
       return BadRequest();
    }

    [HttpPost]
    [Route("Login")]
    [ValidateModel]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);

        if (user != null)
        {
            var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (checkPasswordResult)
            {
                // Get roles
                var roles = await _userManager.GetRolesAsync(user);

                if (roles != null)
                {
                    // Create Token
                   var jwtToken = _tokenRepo.CreateJwtToken(user, roles.ToList());

                   var response = new LoginResponseDto
                   {
                       Token = jwtToken
                   };
                   
                   return Ok(response);
                }
            }
        }

        return BadRequest("Username or password incorrect");
    }
}