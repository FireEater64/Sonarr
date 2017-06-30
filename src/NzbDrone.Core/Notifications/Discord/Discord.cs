using System;
using System.Collections.Generic;
using FluentValidation.Results;
using NLog;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.Notifications.Slack;
using NzbDrone.Core.Rest;
using NzbDrone.Core.Validation;
using RestSharp;

namespace NzbDrone.Core.Notifications.Discord
{
    public class Discord : NotificationBase<DiscordSettings>
    {
        private readonly Logger _logger;

        public Discord(Logger logger)
        {
            _logger = logger;
        }

        public override string Name => "Discord";
        public override string Link => "https://discordapp.com/";

        public override ValidationResult Test()
        {
            var failures = new List<ValidationFailure>();

            failures.AddIfNotNull(TestMessage());

            return new ValidationResult(failures);
        }

        private ValidationFailure TestMessage()
        {
            try
            {
                var message = $"Test message from Sonarr posted at {DateTime.Now}";
                var payload = new DiscordPayload
                {
                    Content = message
                };

                NotifyDiscord(payload);
            }
            // TODO: Specific exception type
            catch (Exception ex)
            {
                return new NzbDroneValidationFailure("Unable to post", ex.Message);
            }

            return null;
        }

        private void NotifyDiscord(DiscordPayload payload)
        {
            var client = RestClientFactory.BuildClient(Settings.WebhookUrl);
            var request = new RestRequest(Method.POST)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new JsonNetSerializer()
            };
            request.AddBody(payload);
            client.ExecuteAndValidate(request);
        }
    }
}