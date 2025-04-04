namespace WhatsappApisSender.Handlers
{
    public interface IAuthenticationHandler
    {
        Task<(bool Success, string Message)> RegisterAsync(string email, string password);

        Task<(bool Success, string AccessToken, string RefreshToken, string Message)> LoginAsync(string email, string password);

        Task<(bool success, string accessToken, string error)> RefreshToken(string refreshToken);

        Task<(bool success, string token, string message)> ForgotPasswordAsync(string email);

        Task<(bool success, string message)> ResetPasswordAsync(string email, string token, string newPassword);
    }
}
