using cotr.backend.Model.Request;
using cotr.backend.Model.Tables;

namespace cotr.backend.test.Moq
{
    public class UserMockData
    {
        public static LoginRequest LoginRequest = new("Luqky", "Luqky");

        public static Users Users = new(1, "Luqky", "email_random@gmail.com", "Sergio", "Luque", "Chacón", DateTime.Now, "Findirect");

        public static UserCredential UserCredential = new(1, "$2a$04$d/IJcjoKj1T.dQEhg7hJSu", "$2a$04$d/IJcjoKj1T.dQEhg7hJSunS62qkbAi0hsWmfhLPtiEFBjwdBp0nC", DateTime.Now, 0, null, null, true);

        public static UserCredential InactiveUserCredential = new(1, "$2a$04$d/IJcjoKj1T.dQEhg7hJSu", "$2a$04$d/IJcjoKj1T.dQEhg7hJSunS62qkbAi0hsWmfhLPtiEFBjwdBp0nC", DateTime.Now, 0, null, null, false);
        public static SignupRequest SignupRequest = new("Luqky", "email_random@gmail.com", "Sergio", "Luque", "Chacón", DateTime.Now, "Findirect", "Luqky");

        public const string Salt = "$2a$04$d/IJcjoKj1T.dQEhg7hJSu";
        public const string HashedPassword = "$2a$04$d/IJcjoKj1T.dQEhg7hJSunS62qkbAi0hsWmfhLPtiEFBjwdBp0nC";
    }
}
