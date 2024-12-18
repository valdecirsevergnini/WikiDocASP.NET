using System.ComponentModel.DataAnnotations;

namespace WikiSistemaASP.NET.Models
{
    // A classe Usuario representa um usuário do sistema
    public class Usuario
    {
        // Propriedade Id para identificar o usuário de forma única
        public int Id { get; set; }

        // Propriedade Username: Nome de usuário que deve ser único
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome de usuário não pode exceder 100 caracteres.")]
        public string Username { get; set; } = string.Empty;

        // Propriedade Senha: Armazena a senha do usuário
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, ErrorMessage = "A senha não pode exceder 100 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        // Propriedade Nome: Nome completo do usuário
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome não pode exceder 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        // Propriedade IsAdmin: Indica se o usuário possui privilégios de administrador
        [Display(Name = "Administrador")]
        public bool IsAdmin { get; set; } = false; // Define se o usuário é administrador
    }
}
