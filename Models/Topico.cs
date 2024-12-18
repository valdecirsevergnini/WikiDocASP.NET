using System.ComponentModel.DataAnnotations;

namespace WikiSistemaASP.NET.Models
{
    /// <summary>
    /// Representa um tópico dentro de um módulo no sistema Wiki.
    /// </summary>
    public class Topico
    {
        /// <summary>
        /// Identificador único do tópico.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Título do tópico, obrigatório e com limite de 200 caracteres.
        /// </summary>
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(200, ErrorMessage = "O título não pode exceder 200 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Conteúdo do tópico, pode ser em texto, Markdown ou HTML.
        /// </summary>
        public string Conteudo { get; set; } = string.Empty;

        /// <summary>
        /// URL opcional para imagens associadas ao tópico.
        /// </summary>
        [Url(ErrorMessage = "O formato da URL da imagem é inválido.")]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// URL opcional para vídeos associados ao tópico.
        /// </summary>
        [Url(ErrorMessage = "O formato da URL do vídeo é inválido.")]
        public string? VideoUrl { get; set; }

        /// <summary>
        /// ID do módulo ao qual o tópico pertence. Este campo é obrigatório.
        /// </summary>
        [Required(ErrorMessage = "O ID do módulo é obrigatório.")]
        public int ModuloId { get; set; }

        /// <summary>
        /// Relacionamento com a classe Modulo, indicando o módulo do tópico.
        /// </summary>
        [Required]
        public Modulo Modulo { get; set; } = null!;
    }
}
