namespace AuthLambda.Dtos
{
    public class AuthDto
    {
        public string Cpf { get; set; }

        public AuthDto(string cpf)
        {
            this.Cpf = cpf;

            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(this.Cpf)) throw new InvalidDataException("Cpf is invalid");
        }
    }
}