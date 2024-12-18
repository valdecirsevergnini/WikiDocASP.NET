using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WikiSistemaASP.NET.Models
{
    /// <summary>
    /// Representa um módulo no sistema, que pode conter múltiplos tópicos.
    /// </summary>
    public class Modulo
    {
        // Chave primária do módulo
        public int Id { get; set; }

        // Nome do módulo - obrigatório
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome não pode exceder 200 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        // Descrição do módulo - opcional
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        // Relacionamento com a entidade Topico: Um módulo pode conter vários tópicos
        public ICollection<Topico> Topicos { get; set; } = new List<Topico>();
    }
}
