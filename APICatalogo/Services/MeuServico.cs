namespace APICatalogo.Services
{
    public class MeuServico : IMeuServico
    {
        public override string Saudacao(string Nome)
        {
            return $"Bem-Vindo, {Nome} \n\n {DateTime.UtcNow}";
        }


    }
}
