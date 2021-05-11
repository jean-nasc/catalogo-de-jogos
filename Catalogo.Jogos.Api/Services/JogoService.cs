using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalogo.Jogos.Api.Entities;
using Catalogo.Jogos.Api.Exceptions;
using Catalogo.Jogos.Api.InputModels;
using Catalogo.Jogos.Api.Repositories;
using Catalogo.Jogos.Api.ViewModels;

namespace Catalogo.Jogos.Api.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;
        
        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        
         public async Task<List<JogoViewModel>> ObterTodos(int pagina, int quantidade)
        {
            var jogos = await _jogoRepository.Obter(pagina, quantidade);

            return jogos.Select(jogo => new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            }).ToList();
        }

        public async Task<JogoViewModel> ObterPorId(Guid id)
        {
            var jogo = await _jogoRepository.Obter(id);

            if(jogo == null) return null;

            return new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }

        public async Task<JogoViewModel> Adicionar(JogoInputModel jogo)
        {
            var entidadeJogo = await _jogoRepository.Obter(jogo.Nome, jogo.Produtora);

            if(entidadeJogo.Count > 0) throw new JogoJaCadastradoException();

            var adicionaJogo = new Jogo
            {
                Id = Guid.NewGuid(),
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };

            await _jogoRepository.Adicionar(adicionaJogo);

            return new JogoViewModel
            {
                Id = adicionaJogo.Id,
                Nome = adicionaJogo.Nome,
                Produtora = adicionaJogo.Produtora,
                Preco = adicionaJogo.Preco
            };
        }

        public async Task Atualizar(Guid id, JogoInputModel jogo)
        {
            var entidadeJogo = await _jogoRepository.Obter(id);

            if(entidadeJogo == null) throw new JogoNaoCadastradoException();

            entidadeJogo.Nome = jogo.Nome;
            entidadeJogo.Produtora = jogo.Produtora;
            entidadeJogo.Preco = jogo.Preco;

            await _jogoRepository.Atualizar(entidadeJogo);
        }

        public async Task Atualizar(Guid id, double preco)
        {
            var entidadeJogo = await _jogoRepository.Obter(id);

            if(entidadeJogo == null) throw new JogoNaoCadastradoException();

            entidadeJogo.Preco = preco;

            await _jogoRepository.Atualizar(entidadeJogo);
        }

        public async Task Remover(Guid id)
        {
            var entidadeJogo = await _jogoRepository.Obter(id);

            if(entidadeJogo == null) throw new JogoNaoCadastradoException();

            await _jogoRepository.Remover(id);
        }

        public void Dispose()
        {
            _jogoRepository?.Dispose();
        }
    }
}