using Microsoft.AspNetCore.Mvc;
using GsNetApi.Models;
using GsNetApi.Services.Interfaces;

namespace GsNetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalizacaoTrabalhoController : ControllerBase
    {
        private readonly ILocalizacaoTrabalhoService _service;

        public LocalizacaoTrabalhoController(ILocalizacaoTrabalhoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocalizacaoTrabalho entity)
        {
            var created = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] LocalizacaoTrabalho entity)
        {
            var updated = await _service.UpdateAsync(id, entity);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
