using AlumniProject.Data.Repostitory;
using AlumniProject.Entity;
using AlumniProject.ExceptionHandler;

namespace AlumniProject.Service.ServiceImp
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepo _roleRepo;
        public RoleService(IRoleRepo roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public async Task<Role> GetRoleById(int id)
        {
            var role = await _roleRepo.GetByIdAsync(r => r.Id == id);
            if (role == null)
            {
                throw new NotFoundException("Role not found with Id: " + id);
            }
            return role;
        }
    }
}
