using System;

namespace Catalogo.Jogos.Api.Exceptions
{
    public class JogoNaoCadastradoException : Exception
    {
        public JogoNaoCadastradoException() : base("Este jogo não está cadastrado."){}
    }
}