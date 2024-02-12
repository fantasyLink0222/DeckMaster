using DeckMaster.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DeckMaster.Models;
using DeckMaster.ViewModels;

namespace DeckMaster.Repositories
{
    public class MyRegisteredUserRepo
    {
        private readonly ApplicationDbContext _db;

        public MyRegisteredUserRepo(ApplicationDbContext db)
        {
            this._db = db;
        }



        public async Task<string> GetUserNameByEmailAsync(string email)
        {

            // Assuming MyRegisteredUser has an Email property
            var registeredUser = await _db.MyRegisteredUsers
                                               .FirstOrDefaultAsync(mru => mru.Email == email);

            if (registeredUser != null)
            {
                var userFullName = $"{registeredUser.FirstName} {registeredUser.LastName}";

                return userFullName;
            }

            return email;
        }

    }
}
