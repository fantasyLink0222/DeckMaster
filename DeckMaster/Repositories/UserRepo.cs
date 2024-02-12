using DeckMaster.Data;
using DeckMaster.ViewModels;


namespace DeckMaster.Repositories
{
    public class UserRepo
    {
        private readonly ApplicationDbContext _db;

        public UserRepo(ApplicationDbContext context)
        {
            this._db = context;

        }

        public List<UserVM> GetAllUsers()
        {
            var users = _db.Users.Select(u => new UserVM { Email = u.Email }).ToList();

            return users;
        }


    }
}
