using System.ComponentModel.DataAnnotations;

namespace auth2.DTOs.UserSettings
{
    public class UpsertUserSettingRequestDto
    {
        [Required]
        public string userId { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string value { get; set; }
        [Required]
        public string type { get; set; }

    }
}
