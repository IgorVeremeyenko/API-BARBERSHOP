namespace WebApplication2.Services {
    public class GeneratingSaltForPasswordHashing {

        public string GenerateSalt() {
            var random = new Random();
            var salt = new byte[16];
            random.NextBytes(salt);
            return Convert.ToBase64String(salt);
        }
    }
}
