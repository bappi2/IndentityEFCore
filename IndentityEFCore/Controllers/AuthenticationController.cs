using IndentityEFCore.Models.Authentication.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Services;

namespace IndentityEFCore.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    
    public AuthenticationController(UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager, IConfiguration configuration,
        IEmailService emailService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _emailService = emailService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUser, string role)
    {
        // Check user exits 
        var existingUser = await _userManager.FindByNameAsync(registerUser.Username);
        if (existingUser != null)
        {
            return BadRequest("User already exists");
        }
        // Add the user in the database
        var user = new IdentityUser
        {
            UserName = registerUser.Username,
            Email = registerUser.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        
        if (await _roleManager.RoleExistsAsync(role))
        {
            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            // Add the user to the role
            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }
            return Ok(new { Message = "User registered successfully" });
        }
        else
        {
            return NotFound(role);
        }
    }

    [HttpGet("test-email")]
    public async Task<IActionResult> TestEmail()
    {
        var message = new Message(
            new string[] { "bappi@live.com" },
            "Test Email",
            "<h1>This is a test email</h1>"
        );
            
        _emailService.SendEmail(message);
                
        return Ok("Testing email");
    }
}