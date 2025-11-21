using Microsoft.AspNetCore.Mvc;
using GsNetApi.Models;
using GsNetApi.Services.Interfaces;
using Asp.Versioning;

namespace GsNetApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class LocalizacaoTrabalhoControllerV2 : ControllerBase
    {
        private readonly ILocalizacaoTrabalhoService _service;
        private readonly LinkGenerator _linkGenerator;

        public LocalizacaoTrabalhoControllerV2(
            ILocalizacaoTrabalhoService service,
            LinkGenerator linkGenerator)
        {
            _service = service;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            var items = await _service.GetAllAsync();

            var totalItems = items.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagedItems = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                page,
                pageSize,
                totalItems,
                totalPages,
                data = pagedItems.Select(i => AddHateoasLinks(i))
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(AddHateoasLinks(item));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocalizacaoTrabalho entity)
        {
            var created = await _service.CreateAsync(entity);

            var url = _linkGenerator.GetUriByAction(
                HttpContext,
                nameof(GetById),
                values: new { id = created.Id, version = "2.0" });

            return Created(url!, AddHateoasLinks(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] LocalizacaoTrabalho entity)
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

        private object AddHateoasLinks(LocalizacaoTrabalho item)
        {
            var self = _linkGenerator.GetUriByAction(
                HttpContext,
                nameof(GetById),
                values: new { id = item.Id, version = "2.0" });

            var update = _linkGenerator.GetUriByAction(
                HttpContext,
                nameof(Update),
                values: new { id = item.Id, version = "2.0" });

            var delete = _linkGenerator.GetUriByAction(
                HttpContext,
                nameof(Delete),
                values: new { id = item.Id, version = "2.0" });

            return new
            {
                item.Id,
                item.Tipo,
                item.GrausCelcius,
                item.NivelUmidade,
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
