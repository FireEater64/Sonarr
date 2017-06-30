using System.Drawing.Text;
using FluentValidation;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.ThingiProvider;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.Notifications.Discord
{
    internal class DiscordSettingsValidator : AbstractValidator<DiscordSettings>
    {
        public DiscordSettingsValidator()
        {
            RuleFor(c => c.WebhookUrl).NotEmpty();
        }
    }

    public class DiscordSettings : IProviderConfig
    {
        private static readonly DiscordSettingsValidator Validator = new DiscordSettingsValidator();

        [FieldDefinition(0, Label = "Webhook URL", HelpText = "Discord channel webhook url", Type = FieldType.Url, HelpLink = "https://support.discordapp.com/hc/en-us/articles/228383668-Intro-to-Webhooks")]
        public string WebhookUrl { get; set; }

        public NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}