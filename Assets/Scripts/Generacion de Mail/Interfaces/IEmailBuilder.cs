using System;
using UnityEngine;

public interface IEmailBuilder
{
    IEmailBuilder SetSender(string sender);
    IEmailBuilder SetSubject(string subject);
    IEmailBuilder SetBody(string body);
    IEmailBuilder SetDate(DateTime date);
    IEmailBuilder SetTheme(EmailTheme theme);
    IEmailBuilder SetDifficulty(EmailDifficulty difficulty);
    IEmailBuilder AddPhishingIndicator(IPhishingIndicator indicator);
    IEmail Build();
}
