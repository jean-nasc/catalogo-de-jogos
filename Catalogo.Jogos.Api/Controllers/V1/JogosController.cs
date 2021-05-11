using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Catalogo.Jogos.Api.Exceptions;
using Catalogo.Jogos.Api.InputModels;
using Catalogo.Jogos.Api.Services;
using Catalogo.Jogos.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Catalogo.Jogos.Api.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        /// <summary>
        /// Buscar todos os jogos de forma paginada.
        /// </summary>
        /// <remarks>
        /// Não é possível retornar jogos sem paginação.
        /// </remarks>
        /// <param name="pagina"> Indica qual página está sendo consultada. Mínimo 1. </param>
        /// <param name="quantidade"> Indica a quantidade de resgistros por página. Mínimo 1 e máximo 50. </param>
        /// <response code="200"> Retorna uma lista de jogos. </response>
        /// <response code="204"> Caso não haja jogos. </response>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> ObterJogos([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var jogos = await _jogoService.ObterTodos(pagina, quantidade);

            if(jogos.Count == 0) return NoContent();

            return Ok(jogos);
        }

        /// <summary>
        /// Buscar jogo por id.
        /// </summary>
        /// <remarks>
        /// Não é possível retornar o jogo sem o id.
        /// </remarks>
        /// <param name="id"> Representa a identificação única do jogo. </param>
        /// <response code="200"> Retorna um jogo filtrado por id. </response>
        /// <response code="204"> Caso não haja jogo cadastrado com o id especificado. </response>

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<JogoViewModel>> ObterJogoPorId([FromRoute] Guid id)
        {
            var jogo = await _jogoService.ObterPorId(id);

            if(jogo == null) return NoContent();

            return Ok(jogo);
        }

        /// <summary>
        /// Adicionar novo jogo.
        /// </summary>
        /// <remarks>
        /// Não é possível adicionar um jogo com o mesmo nome para a mesma produtora.
        /// </remarks>
        /// <response code="200"> Retorna o jogo adicionado. </response>
        /// <response code="422"> Caso o jogo já esteja cadastrado. </response>

        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> AdicionarJogo([FromBody] JogoInputModel jogo)
        {
            try
            {
                var viewModel = await _jogoService.Adicionar(jogo);

                return Ok(viewModel);
            }
            catch (JogoJaCadastradoException ex)
            {
                return UnprocessableEntity(ex.Message);
            }

        }

        /// <summary>
        /// Atualizar jogo por id.
        /// </summary>
        /// <remarks>
        /// Não é possível atualizar o jogo sem o id.
        /// </remarks>
        /// <param name="id"> Representa a identificação única do jogo. </param>
        /// <response code="200"> Jogo atualizado com sucesso. </response>
        /// <response code="404"> Caso não haja jogo cadastrado com o id especificado. </response>
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid id, [FromBody] JogoInputModel jogo)
        {
            try
            {
                await _jogoService.Atualizar(id, jogo);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Atualizar preço do jogo por id.
        /// </summary>
        /// <remarks>
        /// Não é possível atualizar o preço do jogo sem o id.
        /// </remarks>
        /// <param name="id"> Representa a identificação única do jogo. </param>
        /// <param name="preco"> Representa o valor do jogo. </param>
        /// <response code="200"> Jogo atualizado com sucesso. </response>
        /// <response code="404"> Caso não haja jogo cadastrado com o id especificado. </response>
        
        [HttpPatch("{id:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid id, [FromRoute] double preco)
        {
            try
            {
                await _jogoService.Atualizar(id, preco);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletar jogo por id.
        /// </summary>
        /// <remarks>
        /// Não é possível deletar o jogo sem o id.
        /// </remarks>
        /// <param name="id"> Representa a identificação única do jogo. </param>
        /// <response code="200"> Jogo deletado com sucesso. </response>
        /// <response code="404"> Caso não haja jogo cadastrado com o id especificado. </response>
        
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> ExcluirJogo([FromRoute] Guid id)
        {
            try
            {
                await _jogoService.Remover(id);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}