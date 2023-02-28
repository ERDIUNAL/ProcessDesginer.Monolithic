namespace Crea.Core.Mailing;

public interface IMailService
{
    Task SendAsync(Mail mailData);
}