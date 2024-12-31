
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

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, GoalTemplate goalTemplate)
    {
        if (id != goalTemplate.Id)
        {
            return BadRequest();
        }

        _context.Entry(goalTemplate).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GoalTemplateExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<GoalTemplateResponse>> Create([FromBody] SaveGoalTemplateRequest body)
    {
        var template = body.Adapt<GoalTemplate>();
        _context.GoalTemplates.Add(template);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = template.Id }, template.Adapt<GoalTemplateResponse>());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGoalTemplate(Guid id)
    {
        var goalTemplate = await _context.GoalTemplates.FindAsync(id);
        if (goalTemplate == null)
        {
            return NotFound();
        }

        _context.GoalTemplates.Remove(goalTemplate);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool GoalTemplateExists(Guid id)
    {
        return _context.GoalTemplates.Any(e => e.Id == id);
    }
}