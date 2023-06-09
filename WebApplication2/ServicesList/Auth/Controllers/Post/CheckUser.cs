using WebApplication2.Models;

namespace WebApplication2.ServicesList.Auth.Controllers.Post {
    public class CheckUser {

        public bool Check(LoginUser user) {

            if (user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
                return false;
            else {
                return true;
            }
        }
    }
}
