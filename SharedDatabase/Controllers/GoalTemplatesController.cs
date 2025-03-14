
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedDatabase.Contracts;
using SharedDatabase.Infrastructure;

namespace SharedDatabase.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GoalTemplatesController(SharedDatabaseContext context) : ControllerBase
{
    private readonly SharedDatabaseContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GoalTemplateResponse>>> List()
    {
        var templates = await _context.GoalTemplates.ToListAsync();

        return templates.Adapt<List<GoalTemplateResponse>>();
    }

    [HttpGet("ignore-tenant")]
    public async Task<ActionResult<IEnumerable<GoalTemplateResponse>>> ListIgnoreTenant()
    {
        var templates = await _context.GoalTemplates.IgnoreQueryFilters().ToListAsync();

        return templates.Adapt<List<GoalTemplateResponse>>();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GoalTemplateResponse>> Get(Guid id)
    {
        var goalTemplate = await _context.GoalTemplates.FindAsync(id);

        if (goalTemplate == null)
        {
            return NotFound();
        }

        return goalTemplate.Adapt<GoalTemplateResponse>();
    }


    [HttpPost]
    public async Task<ActionResult<GoalTemplateResponse>> Create([FromBody] SaveGoalTemplateRequest body)
    {
        var template = body.Adapt<GoalTemplate>();
        _context.GoalTemplates.Add(template);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = template.Id }, template.Adapt<GoalTemplateResponse>());
    }
}