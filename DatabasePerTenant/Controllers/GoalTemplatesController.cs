// using DatabasePerTenant.Infrastructure;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;

// namespace DatabasePerTenant.Controllers;

// [Route("api/[controller]")]
// [ApiController]
// public class GoalTemplatesController(ApplicationDbContext context) : ControllerBase
// {
//     private readonly ApplicationDbContext _context = context;

//     [HttpGet]
//     public async Task<ActionResult<IEnumerable<GoalTemplate>>> GetGoalTemplates()
//     {
//         return await _context.GoalTemplates.ToListAsync();
//     }

//     [HttpGet("{id}")]
//     public async Task<ActionResult<GoalTemplate>> GetGoalTemplate(int id)
//     {
//         var goalTemplate = await _context.GoalTemplates.FindAsync(id);

//         if (goalTemplate == null)
//         {
//             return NotFound();
//         }

//         return goalTemplate;
//     }

//     // PUT: api/GoalTemplates/5
//     [HttpPut("{id}")]
//     public async Task<IActionResult> PutGoalTemplate(int id, GoalTemplate goalTemplate)
//     {
//         if (id != goalTemplate.Id)
//         {
//             return BadRequest();
//         }

//         _context.Entry(goalTemplate).State = EntityState.Modified;

//         try
//         {
//             await _context.SaveChangesAsync();
//         }
//         catch (DbUpdateConcurrencyException)
//         {
//             if (!GoalTemplateExists(id))
//             {
//                 return NotFound();
//             }
//             else
//             {
//                 throw;
//             }
//         }

//         return NoContent();
//     }

//     // POST: api/GoalTemplates
//     [HttpPost]
//     public async Task<ActionResult<GoalTemplate>> PostGoalTemplate(GoalTemplate goalTemplate)
//     {
//         _context.GoalTemplates.Add(goalTemplate);
//         await _context.SaveChangesAsync();

//         return CreatedAtAction("GetGoalTemplate", new { id = goalTemplate.Id }, goalTemplate);
//     }

//     // DELETE: api/GoalTemplates/5
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> DeleteGoalTemplate(int id)
//     {
//         var goalTemplate = await _context.GoalTemplates.FindAsync(id);
//         if (goalTemplate == null)
//         {
//             return NotFound();
//         }

//         _context.GoalTemplates.Remove(goalTemplate);
//         await _context.SaveChangesAsync();

//         return NoContent();
//     }

//     private bool GoalTemplateExists(int id)
//     {
//         return _context.GoalTemplates.Any(e => e.Id == id);
//     }
// }