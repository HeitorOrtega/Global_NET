using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GsNetApi.Models 
{
    [Table("TB_GS_USUARIO")]
    public class Usuario
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("NOME")]
        public string Nome { get; set; }

        [Column("CPF")]
        public string Cpf { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("SENHA")]
        public string Senha { get; set; }

        [Column("ID_LOCAL_TRABALHO")]
        public long LocalizacaoTrabalhoId { get; set; } 

        [ForeignKey(nameof(LocalizacaoTrabalhoId))]
        public LocalizacaoTrabalho LocTrabalho { get; set; }

        public ICollection<Mensagem> Mensagens { get; set; } = new List<Mensagem>();

        public Usuario() { }
    }
}