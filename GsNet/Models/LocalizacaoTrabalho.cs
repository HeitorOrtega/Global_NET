using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GsNetApi.Enums; 

namespace GsNetApi.Models 
{
    [Table("TB_GS_LOCALIZACAO")]
    public class LocalizacaoTrabalho
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("TIPO")]
        public TipoTrabalho Tipo { get; set; }

        [Column("GRAUS_CELCIUS")]
        public int GrausCelcius { get; set; }

        [Column("NIVEL_UMIDADE")]
        public float NivelUmidade { get; set; }

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

        public LocalizacaoTrabalho() { }
    }
}