using Dto;

namespace Services;

public interface IEmailService
{
    void SendEmail(EmailDto request);
}