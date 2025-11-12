using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsNetApi.Models 
{
    [Table("TB_GS_MENSAGEM")]
    public class Mensagem
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("MENSAGEM")]
        public string TextoMensagem { get; set; } 

        [Column("NIVEL_ESTRESSE")]
        public int NivelEstresse { get; set; }

        [Column("ID_USUARIO")]
        public long UsuarioId { get; set; } 

        [ForeignKey(nameof(UsuarioId))]
        public Usuario Usuario { get; set; }

        public Mensagem() { }
    }
}