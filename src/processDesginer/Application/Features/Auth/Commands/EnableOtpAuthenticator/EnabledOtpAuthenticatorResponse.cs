namespace Application.Features.Auth.Commands.EnableOtpAuthenticator;

public class EnabledOtpAuthenticatorResponse
{
    public string SecretKey { get; set; }
    public string SecretKeyUrl { get; set; }
}
