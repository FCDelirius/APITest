using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository repo, ILogger<UsersController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll(CancellationToken cancellationToken)
        {
            var users = await _repo.GetAllAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<User>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var user = await _repo.GetByIdAsync(id, cancellationToken);
            if (user == null)
            {
                return NotFound(new { error = "User not found." });
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] UserCreateDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.Trim(),
                DateOfBirth = dto.DateOfBirth
            };

            await _repo.CreateAsync(user, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _repo.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                return NotFound(new { error = "User not found." });
            }

            existing.FirstName = dto.FirstName.Trim();
            existing.LastName = dto.LastName.Trim();
            existing.Email = dto.Email.Trim();
            existing.DateOfBirth = dto.DateOfBirth;

            var updated = await _repo.UpdateAsync(existing, cancellationToken);
            if (!updated)
            {
                return StatusCode(500, new { error = "Failed to update user." });
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _repo.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound(new { error = "User not found." });
            }

            return NoContent();
        }
    }
}