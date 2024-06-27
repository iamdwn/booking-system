using System;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using Quartz;
using Service.Services;

namespace Service.Services.Quartzs
{
    public class SendEmailJob : IJob
    {
        private readonly ILogger<SendEmailJob> _logger;
        private readonly IEmailService _emailService;
        private readonly IEmailQueue _emailQueue;

        public SendEmailJob(ILogger<SendEmailJob> logger, IEmailService emailService, IEmailQueue emailQueue)
        {
            _logger = logger;
            _emailService = emailService;
            _emailQueue = emailQueue;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var recipients = await _emailQueue.GetQueue();
                if (recipients == null || !recipients.Any())
                {
                    _logger.LogWarning("No email addresses found in the queue.");
                    return;
                }

                while (recipients.Any())
                {
                    var recipient = await _emailQueue.DequeueEmailAsync();
                    if (recipient != null)
                    {
                        var subject = "Active Link";
                        var body = $"https://localhost:7283/Account/Active?email={recipient}";
                        await _emailService.SendEmailAsync(recipient, subject, body);
                        _logger.LogInformation($"Email sent to {recipient}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in executing SendEmailJob: {ex.Message}");
            }
        }

        private string GenerateOtp(string email) => BCrypt.Net.BCrypt.HashPassword(email);
    }
}
