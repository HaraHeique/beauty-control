using BeautyControl.API.Domain.Employees;

#pragma warning disable CS8618
namespace BeautyControl.API.Features.Account._Common
{
    public record UserResponse
    {
        // Identidade e propriedades partilhadas
        public int Id { get; init; }
        public string Email { get; init; }
        public bool Active { get; init; }

        // Contexto de suporte (Identidade/Auth)
        public string UserName { get; init; }

        // Contexto de negócio (Employee)
        public string FullName { get; init; }
        public Position Position { get; init; }
    }
}
