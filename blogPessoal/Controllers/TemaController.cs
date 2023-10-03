 using blogPessoal.Model;
using blogPessoal.Service;
using blogPessoal.Service.Implements;
using blogPessoal.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace blogPessoal.Controllers
{
    [Authorize]
    [Route("~/tema")]
    [ApiController]
    public class TemaController : ControllerBase
    {
        private readonly ITemaService _temaService;
        private readonly IValidator<Tema> _temaValidator;

        public TemaController(
            ITemaService temaService,
            IValidator<Tema> temaValidator
            )
        {
            _temaService = temaService;
            _temaValidator = temaValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _temaService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var resposta = await _temaService.GetById(id);

            if (resposta == null)
            {
                return NotFound();
            }
            return Ok(resposta);
        }

        [HttpGet("descricao/{descricao}")]
        public async Task<ActionResult> GetByTitulo(string descricao)
        {
            return Ok(await _temaService.GetByDescricao(descricao));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Tema tema)
        {
            var validarTema = await _temaValidator.ValidateAsync(tema);

            if (!validarTema.IsValid) // Correção aqui: troquei !validarTema.IsValid
            {
                return StatusCode(StatusCodes.Status400BadRequest, validarTema);
            }

            await _temaService.Create(tema);

            return CreatedAtAction(nameof(GetById), new { id = tema.Id }, tema);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Tema tema)
        {
            if (tema.Id == 0)
                return BadRequest("ID da Postagem Inavalido");

            var validarTema = await _temaValidator.ValidateAsync(tema);

            if (!validarTema.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarTema);

            var resposta = await _temaService.Update(tema);

            if (resposta is null)
                return NotFound("Postagem ou Tema nao encontrados");

            return Ok(resposta);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {

            var BuscaTema = await _temaService.GetById(id);
            if (BuscaTema is null)
            {
                return NotFound("Postagem não Encontrada");
            }
            await _temaService.Delete(BuscaTema);


            return NoContent();
        }

    }
}
