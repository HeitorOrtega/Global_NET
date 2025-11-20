using Microsoft.AspNetCore.Mvc;
using GsNetApi.Models;
using GsNetApi.Services.Interfaces;
using Asp.Versioning;

namespace GsNetApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MensagemController : ControllerBase
    {
        private readonly IMensagemService _service;
        private readonly LinkGenerator _linkGenerator;

        public MensagemController(IMensagemService service, LinkGenerator linkGenerator)
        {
            _service = service;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _service.GetAllAsync();
            var totalItems = items.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagedItems = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new
            {
                page,
                pageSize,
                totalItems,
                totalPages,
                data = pagedItems.Select(i => AddHateoasLinks(i))
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();

            return Ok(AddHateoasLinks(item));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Mensagem entity)
        {
            var created = await _service.CreateAsync(entity);

            var url = _linkGenerator.GetUriByAction(
                HttpContext,
                nameof(GetById),
                values: new { id = created.Id, version = "1.0" });

            return Created(url!, AddHateoasLinks(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Mensagem entity)
        {
            var updated = await _service.UpdateAsync(id, entity);
            if (updated == null) return NotFound();

            return Ok(AddHateoasLinks(updated));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        // HATEOAS
        private object AddHateoasLinks(Mensagem item)
        {
            var self = _linkGenerator.GetUriByAction(
                HttpContext,
                nameof(GetById),
                values: new { id = item.Id, version = "1.0" });

            var update = _linkGenerator.GetUriByAction(
                HttpContext,
                nameof(Update),
                values: new { id = item.Id, version = "1.0" });

            var delete = _linkGenerator.GetUriByAction(
                HttpContext,
                nameof(Delete),
                values: new { id = item.Id, version = "1.0" });

            return new
            {
                item.Id,
                item.TextoMensagem,
                item.NivelEstresse,
                item.UsuarioId,

                links = new[]
                {
                    new { rel = "self", method = "GET", href = self },
                    new { rel = "update", method = "PUT", href = update },
                    new { rel = "delete", method = "DELETE", href = delete }
                }
            };
        }
    }
}
