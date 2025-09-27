// Implementaciones concretas de Email
using System;
using System.Collections.Generic;

public class PhishingEmail : IEmail
{
    public string Sender { get; private set; }
    public string Subject { get; private set; }
    public string Body { get; private set; }
    public DateTime Date { get; private set; }
    public bool IsPhishing => true;
    public EmailTheme Theme { get; private set; }
    public EmailDifficulty Difficulty { get; private set; }
    private List<IPhishingIndicator> _indicators;

    public PhishingEmail(string sender, string subject, string body, DateTime date,
                        EmailTheme theme, EmailDifficulty difficulty,
                        List<IPhishingIndicator> indicators)
    {
        Sender = sender;
        Subject = subject;
        Body = body;
        Date = date;
        Theme = theme;
        Difficulty = difficulty;
        _indicators = indicators ?? new List<IPhishingIndicator>();
    }

    public List<IPhishingIndicator> GetPhishingIndicators()
    {
        return new List<IPhishingIndicator>(_indicators);
    }
}

public class LegitimateEmail : IEmail
{
    public string Sender { get; private set; }
    public string Subject { get; private set; }
    public string Body { get; private set; }
    public DateTime Date { get; private set; }
    public bool IsPhishing => false;
    public EmailTheme Theme { get; private set; }
    public EmailDifficulty Difficulty { get; private set; }

    public LegitimateEmail(string sender, string subject, string body, DateTime date,
                          EmailTheme theme, EmailDifficulty difficulty)
    {
        Sender = sender;
        Subject = subject;
        Body = body;
        Date = date;
        Theme = theme;
        Difficulty = difficulty;
    }

    public List<IPhishingIndicator> GetPhishingIndicators()
    {
        return new List<IPhishingIndicator>(); // Siempre vacío para emails legítimos
    }
}
