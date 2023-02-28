namespace Application.Services.Core.AuthService;

public static class AuthServiceBusinessMessages
{
    public const string AuthenticatorCodeSubject = "Login olabilmek için kodu giriniz - RentACar";
    public const string InvalidAuthenticatorCode = "İki aşamalı doğrulama kodu yanlış!";
    public static string AuthenticatorCodeTextBody(string authenticatorCode) => $"İki aşamalı doğrulama kodunuz: {authenticatorCode.Substring(0, 3)} {authenticatorCode.Substring(3)}";
}
