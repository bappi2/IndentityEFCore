using UserService.Models;

namespace UserService.Services;

public interface IEmailService
{
    void SendEmail(Message message);
}