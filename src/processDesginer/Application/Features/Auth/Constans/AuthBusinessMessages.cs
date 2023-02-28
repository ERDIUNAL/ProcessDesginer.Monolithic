namespace Application.Features.Auth.Constans;

public static class AuthBusinessMessages
{
    public const string UserEmailAlreadyExists = "Bu e-posta adresi ile daha önce kayıt oluşturulmuş!";
    public const string UserNotFound = "Kullanıcı bulunamadı!";
    public const string UserPasswordNotMatch = "Kullanıcı şifresi eşleşmiyor!";
    public const string RefreshTokenNotFound = "RefreshToken cannot found!";
    public const string RefreshTokenNotActive = "RefreshToken is not active!";
    public const string UserAlreadyHasAuthenticator = "Kullanıcı zaten bir doğrulayıcıya sahip!";
    public const string VerifyEmail = "E-posta adresinizi doğrulayın!";
    public const string ClickOnBelowVerifyEmail = "E-posta adresini doğrulamak için lütfen aşağıdaki linke tıklayın!";
    public const string UserEmailAuthenticatorNotFound = "Kullanıcıya ait onaylanması gereken e-posta doğrulama isteği bulunamadı!";
    public const string UserOtpAuthenticatorNotFound = "Kullanıcıya ait onaylanması gereken OTP doğrulama isteği bulunamadı!";
}
