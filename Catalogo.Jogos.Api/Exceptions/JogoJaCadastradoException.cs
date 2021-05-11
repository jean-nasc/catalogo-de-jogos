using System;

namespace Catalogo.Jogos.Api.Exceptions
{
    public class JogoJaCadastradoException : Exception
    {
        public JogoJaCadastradoException() : base("Este jogo já está cadastrado."){ }
    }
}