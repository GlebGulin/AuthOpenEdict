using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace auth2.Data
{
    [Table("ApplicationUserSetting")]
    public class ApplicationUserSetting
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(450)]
        public string UserId { get; set; } = null!;
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = null!;
        [MaxLength(512)]
        public string? Value { get; set; }
        [MaxLength(50)]
        public string? Type { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
