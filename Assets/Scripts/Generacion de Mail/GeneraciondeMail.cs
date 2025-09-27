// Builder para construir emails complejos paso a paso
using System;
using System.Collections.Generic;


// Builder concreto para emails de phishing
public class PhishingEmailBuilder : IEmailBuilder
{
    private string _sender;
    private string _subject;
    private string _body;
    private DateTime _date = DateTime.Now;
    private EmailTheme _theme = EmailTheme.Corporate;
    private EmailDifficulty _difficulty = EmailDifficulty.Medium;
    private List<IPhishingIndicator> _indicators = new List<IPhishingIndicator>();

    public IEmailBuilder SetSender(string sender)
    {
        _sender = sender;
        return this;
    }

    public IEmailBuilder SetSubject(string subject)
    {
        _subject = subject;
        return this;
    }

    public IEmailBuilder SetBody(string body)
    {
        _body = body;
        return this;
    }

    public IEmailBuilder SetDate(DateTime date)
    {
        _date = date;
        return this;
    }

    public IEmailBuilder SetTheme(EmailTheme theme)
    {
        _theme = theme;
        return this;
    }

    public IEmailBuilder SetDifficulty(EmailDifficulty difficulty)
    {
        _difficulty = difficulty;
        return this;
    }

    public IEmailBuilder AddPhishingIndicator(IPhishingIndicator indicator)
    {
        _indicators.Add(indicator);
        return this;
    }

    public IEmail Build()
    {
        if (string.IsNullOrEmpty(_sender) || string.IsNullOrEmpty(_subject) || string.IsNullOrEmpty(_body))
        {
            throw new InvalidOperationException("Email cannot be built with missing required fields");
        }

        return new PhishingEmail(_sender, _subject, _body, _date, _theme, _difficulty, _indicators);
    }

}

// Builder concreto para emails legítimos
public class LegitimateEmailBuilder : IEmailBuilder
{
    private string _sender;
    private string _subject;
    private string _body;
    private DateTime _date = DateTime.Now;
    private EmailTheme _theme = EmailTheme.Corporate;
    private EmailDifficulty _difficulty = EmailDifficulty.Medium;

    public IEmailBuilder SetSender(string sender)
    {
        _sender = sender;
        return this;
    }

    public IEmailBuilder SetSubject(string subject)
    {
        _subject = subject;
        return this;
    }

    public IEmailBuilder SetBody(string body)
    {
        _body = body;
        return this;
    }

    public IEmailBuilder SetDate(DateTime date)
    {
        _date = date;
        return this;
    }

    public IEmailBuilder SetTheme(EmailTheme theme)
    {
        _theme = theme;
        return this;
    }

    public IEmailBuilder SetDifficulty(EmailDifficulty difficulty)
    {
        _difficulty = difficulty;
        return this;
    }

    public IEmailBuilder AddPhishingIndicator(IPhishingIndicator indicator)
    {
        // No aplicable para emails legítimos, simplemente ignorar
        return this;
    }

    public IEmail Build()
    {
        if (string.IsNullOrEmpty(_sender) || string.IsNullOrEmpty(_subject) || string.IsNullOrEmpty(_body))
        {
            throw new InvalidOperationException("Email cannot be built with missing required fields");
        }

        return new LegitimateEmail(_sender, _subject, _body, _date, _theme, _difficulty);
    }

}