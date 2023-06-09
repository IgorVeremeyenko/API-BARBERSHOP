using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Services.Cache;
using WebApplication2.Services;

namespace WebApplication2.ServicesList.Admins.Controllers.Put {
    public class PutByIdService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> Init(HashingPassword hashingPassword, int id, Admin admin, GeneratingSaltForPasswordHashing generatingSaltForPasswordHashing) {

            if (id != admin.Id) {
                return new BadRequestResult();
            }
            var salt = generatingSaltForPasswordHashing.GenerateSalt();

            var hashedPassword = hashingPassword.HashPassword(admin.Password, salt);

            var adm = await _context.Admins.FindAsync(id);

            if (adm != null) {
                adm.Password = hashedPassword;
                adm.Salt = salt;
            }

            _context.Entry(adm).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!AdminExists(id, _context)) {
                    return new NotFoundResult();
                }
                else {
                    throw;
                }
            }

            return new NoContentResult();
        }

        private bool AdminExists(int id, MyDatabaseContext context) {
            return (context.Admins?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
