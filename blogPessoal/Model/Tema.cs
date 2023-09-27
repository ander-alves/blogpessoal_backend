using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blogPessoal.Model
{
    public class Tema
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;

        [InverseProperty("Tema")]
        [JsonIgnore]
        public virtual ICollection<Postagem>? Postagem { get; set; }

    }
}
