namespace ChallengeNet.Core.Models
{
    public static class Consts
    {
        #region AccessToken

        public const string Audience = "Audience";
        public const string ExpiresTokenInMinutes = "ExpiresTokenInMinutes";
        public const string Issuer = "Issuer";
        public const string SecretKey = "SecretKey";

        #endregion

        #region UserRules

        public const string RuleAdmin = "admin";
        public const string RuleUser = "user";

        #endregion

        #region Errors

        public const string ErrorInternalServerDescription = "Internal server error, contact administrator";
        public const string ErrorNotFoundDescription = "not found";
        public const string ErrorUserAndOrPasswordInvalidDescription = "Username and/or Password invalid";

        #endregion
    }
}