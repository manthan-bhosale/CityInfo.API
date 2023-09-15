namespace CityInfo.API.Sevices
{
    public class CloudMailService : IMailService
    {
        private readonly string _mailTo = string.Empty;
        private readonly string _mailFrom = string.Empty;


        public CloudMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAddress"] = null!;
            _mailFrom = configuration["mailSettings:mailFromAddress"] = null!;
        }

        public void Send(string subject, string message)
        {
            // send mail - output tp console window
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, " +
                $"with {nameof(CloudMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
    
}
