using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WikiSistemaASP.NET.Models
{
    /// <summary>
    /// Representa um módulo no sistema, que pode conter múltiplos tópicos.
    /// </summary>
    public class Modulo
    {
        /// <summary>
        /// Chave primária do módulo.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do módulo - obrigatório.
        /// </summary>
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome não pode exceder 200 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Descrição do módulo - opcional.
        /// </summary>
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Relacionamento com a entidade Topico: Um módulo pode conter vários tópicos.
        /// </summary>
        public List<Topico> Topicos { get; set; } = new List<Topico>();
    }
}
