using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalogo.Jogos.Api.Entities;

namespace Catalogo.Jogos.Api.Repositories
{
    public interface IJogoRepository : IDisposable
    {
        Task<List<Jogo>> Obter(int pagina, int quantidade);
        Task<Jogo> Obter(Guid id);
        Task<List<Jogo>> Obter(string nome, string produtora);
        Task Adicionar(Jogo jogo);
        Task Atualizar(Jogo jogo);
        Task Remover(Guid id);
    }
}