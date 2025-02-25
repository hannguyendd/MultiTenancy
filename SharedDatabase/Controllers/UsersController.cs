using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedDatabase.Contracts;
using SharedDatabase.Infrastructure;

namespace SharedDatabase.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(SharedDatabaseContext context, UserManager<AppUser> userManager) : ControllerBase
{
    private readonly SharedDatabaseContext _context = context;
    private readonly UserManager<AppUser> _userManager = userManager;

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest body)
    {
        var user = new AppUser
        {
            Email = body.Email,
            FullName = body.FullName,
            UserName = body.Email
        };

        var result = await _userManager.CreateAsync(user, body.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(user.Adapt<UserResponse>());
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> List()
    {
        var users = await _context.Users.ToListAsync();

        return users.Adapt<List<UserResponse>>();
    }

}