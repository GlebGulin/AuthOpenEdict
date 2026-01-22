using auth2.Data;
using auth2.DTOs.UserSettings;
using auth2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSettingsController : ControllerBase
    {
        private readonly IUserSettingService _userSettingService;

        public UserSettingsController(IUserSettingService userSettingService)
        {
            _userSettingService = userSettingService;
        }
        [Authorize]
        [HttpGet("{userId}")]
        public ActionResult<List<ApplicationUserSetting>> Get([FromRoute] string userId)
        {

            return Ok(_userSettingService.Get(userId));
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("")]
        public ActionResult<UpsertUserSettingResponseDto> Upsert([FromBody] UpsertUserSettingRequestDto request)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            ApplicationUserSetting setting = _userSettingService.Upsert(request.userId, request.name, request.value, request.type);
            UpsertUserSettingResponseDto response = new UpsertUserSettingResponseDto()
            {
                id = setting.Id,
            };

            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _userSettingService.Delete(id);
            return Ok();
        }
    }
}
