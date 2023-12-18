using cotr.backend.Model;
using cotr.backend.Model.Request;
using cotr.backend.Model.Tables;

namespace cotr.backend.test.Moq
{
    public class UserMockData
    {
        public static LoginRequest LoginRequest = new("Luqky", "Luqky");

        public static Users SignupUser = new("Luqky", "email_random@gmail.com", false, "1234", DateTime.Now.AddDays(10), "Sergio", "Luque", "Chacón", DateTime.Now, "Findirect");
        public static Users Users = new(1, "Luqky", "email_random@gmail.com", false, "1234", DateTime.Now.AddDays(10), "Sergio", "Luque", "Chacón", DateTime.Now, "Findirect");
        public static Users UserValidated = new(1, "Luqky", "email_random@gmail.com", true, null, null, "Sergio", "Luque", "Chacón", DateTime.Now, "Findirect");

        public static UserCredential UserCredential = new(1, "$2a$04$d/IJcjoKj1T.dQEhg7hJSunS62qkbAi0hsWmfhLPtiEFBjwdBp0nC", DateTime.Now, 0, null, null, true);
        public static UserCredential UserCredentialRecoveryPassword = new(1, "$2a$04$d/IJcjoKj1T.dQEhg7hJSunS62qkbAi0hsWmfhLPtiEFBjwdBp0nC", DateTime.Now, 0, "1234", DateTime.Now.AddDays(1), true);
        public static UserCredential UserCredentialRecoveryPasswordBadTokenExpiration = new(1, "$2a$04$d/IJcjoKj1T.dQEhg7hJSunS62qkbAi0hsWmfhLPtiEFBjwdBp0nC", DateTime.Now, 0, "1234", DateTime.Now.AddDays(-1), true);
        public static UserCredential InactiveUserCredential = new(1, "$2a$04$d/IJcjoKj1T.dQEhg7hJSunS62qkbAi0hsWmfhLPtiEFBjwdBp0nC", DateTime.Now, 0, null, null, false);

        public static SignupRequest SignupRequest = new("Luqky", "email_random@gmail.com", "Sergio", "Luque", "Chacón", DateTime.Now.AddYears(-17), "Findirect", "Luqky1234");
        public static SignupRequest SignupRequestBadPassword = new("Luqky", "email_random@gmail.com", "Sergio", "Luque", "Chacón", DateTime.Now.AddYears(-17), "Findirect", "Luqky");

        public const string HashedPassword = "$2a$04$d/IJcjoKj1T.dQEhg7hJSunS62qkbAi0hsWmfhLPtiEFBjwdBp0nC";

        public static UpdatePasswordRequest UpdatePasswordRequest = new("0123456789asdfgh", "Luqky1234");
        public static UpdatePasswordRequest UpdatePasswordRequestBadPassword = new("0123456789asdfgh", "Luqky");

        public static EmailMessage RecoveryEmail = new("email_random@gmail.com", "Cambia tu contraseña", "<p>Hola <strong>Sergio</strong>,<br>Puedes cambiar tu contraseña accediendo al siguiente enlace:</p><p><a href='https://blue-rock-0344a9c03.4.azurestaticapps.net/change-password?token=1234'>Cambia tu contraseña</a></p><p>Si no has solicitado el cambio ponte en contacto con nosotros</p>");
    
        public static EmailMessage SignupEmail = new("email_random@gmail.com", "Verifica tu cuenta", "<p>Hola <strong>Sergio</strong>,<br>Gracias por registrarte en nuestra plataforma, accede al siguiente enlace para verificar tu correo:</p><p><a href='https://blue-rock-0344a9c03.4.azurestaticapps.net/user/verify?token=1234'>Codes of the ring</a></p><p>Si no has solicitado el registro ponte en contacto con nosotros</p>");
    }
}
