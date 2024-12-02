using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure;
using Azure.Communication.Email;

namespace KlarkaAdventCalendar.Api
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            var connectionString = Environment.GetEnvironmentVariable("EMAIL_CONNECTION_STRING");
            var emailClient = new EmailClient(connectionString);

            var emailMessage = new EmailMessage(
              senderAddress: "DoNotReply@46f18bca-ca64-4724-afd7-58913db75f43.azurecomm.net",
              content: new EmailContent("Test Email")
              {
                  PlainText = "Hello world via email.",
                  Html = @"
		<html>
			<body>
				<h1>Hello world via email.</h1>
			</body>
		</html>"
              },
              recipients: new EmailRecipients(new List<EmailAddress> { new EmailAddress("honzaklicpera@gmail.com") }));



            EmailSendOperation emailSendOperation = await emailClient.SendAsync(WaitUntil.Completed, emailMessage);

            _logger.LogInformation(connectionString);
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
