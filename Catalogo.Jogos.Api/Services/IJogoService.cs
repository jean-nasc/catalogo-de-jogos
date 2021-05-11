using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalogo.Jogos.Api.InputModels;
using Catalogo.Jogos.Api.ViewModels;

namespace Catalogo.Jogos.Api.Services
{
    public interface IJogoService : IDisposable
    {
        Task<List<JogoViewModel>> ObterTodos(int pagina, int quantidade);
        Task<JogoViewModel> ObterPorId(Guid id);
        Task<JogoViewModel> Adicionar(JogoInputModel jogo);
        Task Atualizar(Guid id, JogoInputModel jogo);
        Task Atualizar(Guid id, double preco);
        Task Remover(Guid id);
    }
}