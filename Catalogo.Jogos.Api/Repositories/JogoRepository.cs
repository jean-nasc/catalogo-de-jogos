using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalogo.Jogos.Api.Entities;

namespace Catalogo.Jogos.Api.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private static Dictionary<Guid, Jogo> jogos = new Dictionary<Guid, Jogo>()
        {
            {Guid.Parse("e4d07bf1-73ac-4858-913d-0fac4d621afa"), new Jogo{Id = Guid.Parse("e4d07bf1-73ac-4858-913d-0fac4d621afa"), Nome = "FIFA 20", Produtora = "EA Games", Preco = 159.90}},

            {Guid.Parse("5e04cd6a-3c66-4e78-92e9-01d9c1cb1556"), new Jogo{Id = Guid.Parse("5e04cd6a-3c66-4e78-92e9-01d9c1cb1556"), Nome = "Sonic All Star Racing Transformed", Produtora = "Sumo Digital", Preco = 79.90}},

            {Guid.Parse("4ae0901b-d85e-48b9-b3fb-3c8c859522b9"), new Jogo{Id = Guid.Parse("4ae0901b-d85e-48b9-b3fb-3c8c859522b9"), Nome = "Dragon Ball Xenoverse", Produtora = "Bandai Namco", Preco = 199.90}},

            {Guid.Parse("517e3fb1-9a93-40f9-9c52-18de57fa7f0e"), new Jogo{Id = Guid.Parse("517e3fb1-9a93-40f9-9c52-18de57fa7f0e"), Nome = "Rocket League", Produtora = "Psyonix", Preco = 0.00}}
        };

        public Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            return Task.FromResult(jogos.Values.Skip((pagina - 1) * quantidade).Take(quantidade).ToList());
        }

        public Task<Jogo> Obter(Guid id)
        {
            if(!jogos.ContainsKey(id)) return null;

            return Task.FromResult(jogos[id]);
        }

        public Task<List<Jogo>> Obter(string nome, string produtora)
        {
            return Task.FromResult(jogos.Values.Where(jogo => jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora)).ToList());
        }
        
        public Task Adicionar(Jogo jogo)
        {
            jogos.Add(jogo.Id, jogo);

            return Task.CompletedTask;
        }

        public Task Atualizar(Jogo jogo)
        {
            jogos[jogo.Id] = jogo;

            return Task.CompletedTask;
        }

        public Task Remover(Guid id)
        {
            jogos.Remove(id);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //Implementar caso trabalhe com banco de dados
        }
    }
}