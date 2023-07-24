using AlumniProject.Entity;

namespace AlumniProject.Service
{
    public interface IRoleService
    {
        Task<Role> GetRoleById(int id);
    }
}
