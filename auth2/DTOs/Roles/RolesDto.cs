namespace auth2.DTOs
{
    public class RolesDto
    {
        List<RoleDto> Roles { get; set; }
        public RolesDto()
        {
            Roles = new List<RoleDto>();
        }
    }
}
