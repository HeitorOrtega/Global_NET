using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsNetApi.Models 
{
    [Table("TB_GS_LOGIN")]
    public class Login
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("EMAIL")]
        [Required]
        public string Email { get; set; }

        [Column("SENHA")]
        [Required]
        public string Senha { get; set; }

        public Login() { }
    }
}