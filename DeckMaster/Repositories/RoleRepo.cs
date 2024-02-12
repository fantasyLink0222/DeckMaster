using DeckMaster.Data;
using DeckMaster.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DeckMaster.Repositories
{
    public class RoleRepo
    {
        private readonly ApplicationDbContext _db;

        public RoleRepo(ApplicationDbContext db)
        {
            this._db = db;
            CreateInitialRole();
        }

        public List<RoleVM> GetAllRoles()
        {
            var roles = _db.Roles.Select(r => new RoleVM
            {
                Id = r.Id,
                RoleName = r.Name
            }).ToList();

            return roles;
        }

        public RoleVM GetRole(string roleName)
        {
            var role =
                _db.Roles.Where(r => r.Name == roleName)
                              .FirstOrDefault();

            if (role != null)
            {
                return new RoleVM()
                {
                    RoleName = role.Name
                                    ,
                    Id = role.Id
                };
            }
            return null;
        }

        public bool CreateRole(string roleName)
        {
            bool isSuccess = true;

            try
            {
                _db.Roles.Add(new IdentityRole
                {
                    Name = roleName,
                    Id = roleName,
                    NormalizedName = roleName.ToUpper()
                });
                _db.SaveChanges();
            }
            catch (Exception)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public void CreateInitialRole()
        {
            const string ADMIN = "Admin";

            var role = GetRole(ADMIN);

            if (role == null)
            {
                CreateRole(ADMIN);
            }
        }
        public async Task<(bool isSuccess, string message)> DeleteRoleAsync(string id)
        {
            // Check if the role is assigned to any user
            var isRoleAssigned = await _db.UserRoles.AnyAsync(ur => ur.RoleId == id);
            if (isRoleAssigned)
            {
                return (false, $"Role: {id} is assigned to a user and cannot be deleted.");
            }

            try
            {
                // If not, proceed to delete the role
                var roleToDelete = new IdentityRole { Id = id };
                _db.Roles.Attach(roleToDelete);
                _db.Roles.Remove(roleToDelete);
                await _db.SaveChangesAsync();

                return (true, $"Role: {id} deleted successfully.");
            }
            catch (Exception e)
            {
                return (false, $"Error deleting role with ID {id}: {e.Message}");
            }
        }


    }
}
