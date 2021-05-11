using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Catalogo.Jogos.Api.Entities;
using Microsoft.Extensions.Configuration;

namespace Catalogo.Jogos.Api.Repositories
{
    public class JogoSqlServerRepository : IJogoRepository
    {
        private readonly SqlConnection sqlConnection;

        public JogoSqlServerRepository(IConfiguration configuration)
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            CriarTabela();
        }

        public async Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            var jogos = new List<Jogo>();

            var comando = $"select * from jogos order by id offset {((pagina - 1) * quantidade)} rows fetch next {quantidade} rows only";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while(sqlDataReader.Read())
            {
                jogos.Add(new Jogo
                {
                    Id = Guid.Parse(sqlDataReader["Id"].ToString()),
                    Nome = (string) sqlDataReader["Nome"],
                    Produtora = (string) sqlDataReader["Produtora"],
                    Preco = double.Parse(sqlDataReader["Preco"].ToString().Replace(".", ","))
                });
            }

            await sqlConnection.CloseAsync();

            return jogos;
        }

        public async Task<Jogo> Obter(Guid id)
        {
            Jogo jogo = null;

            var comando = $"select * from jogos where Id = '{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while(sqlDataReader.Read())
            {
                jogo = new Jogo
                {
                    Id = Guid.Parse(sqlDataReader["Id"].ToString()),
                    Nome = (string) sqlDataReader["Nome"],
                    Produtora = (string) sqlDataReader["Produtora"],
                    Preco = double.Parse(sqlDataReader["Preco"].ToString().Replace(".", ","))
                };
            }

            await sqlConnection.CloseAsync();
            
            return jogo;
        }

        public async Task<List<Jogo>> Obter(string nome, string produtora)
        {
            var jogos = new List<Jogo>();

            var comando = $"select * from jogos where Nome = '{nome}' and Produtora = '{produtora}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while(sqlDataReader.Read())
            {
                jogos.Add(new Jogo
                {
                    Id = Guid.Parse(sqlDataReader["Id"].ToString()),
                    Nome = (string) sqlDataReader["Nome"],
                    Produtora = (string) sqlDataReader["Produtora"],
                    Preco = double.Parse(sqlDataReader["Preco"].ToString().Replace(".", ","))
                });
            }

            await sqlConnection.CloseAsync();

            return jogos;
        }

        public async Task Adicionar(Jogo jogo)
        {
            var comando = $"insert jogos (Id, Nome, Produtora, Preco) values ('{jogo.Id}', '{jogo.Nome}', '{jogo.Produtora}', {jogo.Preco.ToString().Replace(",", ".")})";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Atualizar(Jogo jogo)
        {
            var comando = $"update jogos set Nome = '{jogo.Nome}', Produtora = '{jogo.Produtora}', Preco = {jogo.Preco.ToString().Replace(",", ".")} where Id = '{jogo.Id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Remover(Guid id)
        {
            var comando = $"delete from jogos where Id = '{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public void CriarTabela()
        {
            var comando = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Jogos') BEGIN CREATE TABLE Jogos (Id char(36) PRIMARY KEY,";
            comando += "Nome varchar(80) NOT NULL,";
            comando += "Produtora varchar(80) NOT NULL,";
            comando += "Preco varchar(14) NOT NULL) END";

            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }
    }
}