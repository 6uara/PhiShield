using System;
using UnityEngine;


// Concrete Factory para emails de phishing
public class PhishingEmailFactory : IEmailFactory
{
    private readonly IContentGenerator _contentGenerator;

    public PhishingEmailFactory(IContentGenerator contentGenerator)
    {
        _contentGenerator = contentGenerator;
    }

    public IEmail CreateBankingEmail(EmailDifficulty difficulty)
    {
        var builder = new PhishingEmailBuilder();

        // Generar el contenido utilizando la estrategia de generación
        var senderData = _contentGenerator.GeneratePhishingSender(EmailTheme.Banking, difficulty);
        var subject = _contentGenerator.GeneratePhishingSubject(EmailTheme.Banking, difficulty);
        var body = _contentGenerator.GeneratePhishingBody(EmailTheme.Banking, difficulty);

        // Configurar indicadores de phishing basados en la dificultad
        var indicators = _contentGenerator.GeneratePhishingIndicators(EmailTheme.Banking, difficulty);

        return builder
            .SetSender(senderData)
            .SetSubject(subject)
            .SetBody(body)
            .SetDate(GenerateRecentDate())
            .SetTheme(EmailTheme.Banking)
            .SetDifficulty(difficulty)
            .Build();
    }

    public IEmail CreateCorporateEmail(EmailDifficulty difficulty)
    {
        var builder = new PhishingEmailBuilder();

        var senderData = _contentGenerator.GeneratePhishingSender(EmailTheme.Corporate, difficulty);
        var subject = _contentGenerator.GeneratePhishingSubject(EmailTheme.Corporate, difficulty);
        var body = _contentGenerator.GeneratePhishingBody(EmailTheme.Corporate, difficulty);
        var indicators = _contentGenerator.GeneratePhishingIndicators(EmailTheme.Corporate, difficulty);

        return builder
            .SetSender(senderData)
            .SetSubject(subject)
            .SetBody(body)
            .SetDate(GenerateRecentDate())
            .SetTheme(EmailTheme.Corporate)
            .SetDifficulty(difficulty)
            .Build();
    }

    public IEmail CreatePersonalEmail(EmailDifficulty difficulty)
    {
        var builder = new PhishingEmailBuilder();

        var senderData = _contentGenerator.GeneratePhishingSender(EmailTheme.Personal, difficulty);
        var subject = _contentGenerator.GeneratePhishingSubject(EmailTheme.Personal, difficulty);
        var body = _contentGenerator.GeneratePhishingBody(EmailTheme.Personal, difficulty);
        var indicators = _contentGenerator.GeneratePhishingIndicators(EmailTheme.Personal, difficulty);

        return builder
            .SetSender(senderData)
            .SetSubject(subject)
            .SetBody(body)
            .SetDate(GenerateRecentDate())
            .SetTheme(EmailTheme.Personal)
            .SetDifficulty(difficulty)
            .Build();
    }

    public IEmail CreateShoppingEmail(EmailDifficulty difficulty)
    {
        var builder = new PhishingEmailBuilder();

        var senderData = _contentGenerator.GeneratePhishingSender(EmailTheme.Shopping, difficulty);
        var subject = _contentGenerator.GeneratePhishingSubject(EmailTheme.Shopping, difficulty);
        var body = _contentGenerator.GeneratePhishingBody(EmailTheme.Shopping, difficulty);
        var indicators = _contentGenerator.GeneratePhishingIndicators(EmailTheme.Shopping, difficulty);

        return builder
            .SetSender(senderData)
            .SetSubject(subject)
            .SetBody(body)
            .SetDate(GenerateRecentDate())
            .SetTheme(EmailTheme.Shopping)
            .SetDifficulty(difficulty)
            .Build();
    }

    public IEmail CreateSocialEmail(EmailDifficulty difficulty)
    {
        var builder = new PhishingEmailBuilder();

        var senderData = _contentGenerator.GeneratePhishingSender(EmailTheme.Social, difficulty);
        var subject = _contentGenerator.GeneratePhishingSubject(EmailTheme.Social, difficulty);
        var body = _contentGenerator.GeneratePhishingBody(EmailTheme.Social, difficulty);
        var indicators = _contentGenerator.GeneratePhishingIndicators(EmailTheme.Social, difficulty);

        return builder
            .SetSender(senderData)
            .SetSubject(subject)
            .SetBody(body)
            .SetDate(GenerateRecentDate())
            .SetTheme(EmailTheme.Social)
            .SetDifficulty(difficulty)
            .Build();
    }

    private DateTime GenerateRecentDate()
    {
        // Generar una fecha de los últimos 7 días
        int daysAgo = UnityEngine.Random.Range(0, 7);
        return DateTime.Now.AddDays(-daysAgo);
    }

}

// Concrete Factory para emails legítimos
public class LegitimateEmailFactory : IEmailFactory
{
    private readonly IContentGenerator _contentGenerator;

    public LegitimateEmailFactory(IContentGenerator contentGenerator)
    {
        _contentGenerator = contentGenerator;
    }

    public IEmail CreateBankingEmail(EmailDifficulty difficulty)
    {
        var builder = new LegitimateEmailBuilder();

        var senderData = _contentGenerator.GenerateLegitimateSender(EmailTheme.Banking);
        var subject = _contentGenerator.GenerateLegitimateSubject(EmailTheme.Banking);
        var body = _contentGenerator.GenerateLegitimateBody(EmailTheme.Banking);

        return builder
            .SetSender(senderData)
            .SetSubject(subject)
            .SetBody(body)
            .SetDate(GenerateRecentDate())
            .SetTheme(EmailTheme.Banking)
            .SetDifficulty(difficulty)
            .Build();
    }

    public IEmail CreateCorporateEmail(EmailDifficulty difficulty)
    {
        var builder = new LegitimateEmailBuilder();

        var senderData = _contentGenerator.GenerateLegitimateSender(EmailTheme.Corporate);
        var subject = _contentGenerator.GenerateLegitimateSubject(EmailTheme.Corporate);
        var body = _contentGenerator.GenerateLegitimateBody(EmailTheme.Corporate);

        return builder
            .SetSender(senderData)
            .SetSubject(subject)
            .SetBody(body)
            .SetDate(GenerateRecentDate())
            .SetTheme(EmailTheme.Corporate)
            .SetDifficulty(difficulty)
            .Build();
    }

    public IEmail CreatePersonalEmail(EmailDifficulty difficulty)
    {
        var builder = new LegitimateEmailBuilder();

        var senderData = _contentGenerator.GenerateLegitimateSender(EmailTheme.Personal);
        var subject = _contentGenerator.GenerateLegitimateSubject(EmailTheme.Personal);
        var body = _contentGenerator.GenerateLegitimateBody(EmailTheme.Personal);

        return builder
            .SetSender(senderData)
            .SetSubject(subject)
            .SetBody(body)
            .SetDate(GenerateRecentDate())
            .SetTheme(EmailTheme.Personal)
            .SetDifficulty(difficulty)
            .Build();
    }

    public IEmail CreateShoppingEmail(EmailDifficulty difficulty)
    {
        var builder = new LegitimateEmailBuilder();

        var senderData = _contentGenerator.GenerateLegitimateSender(EmailTheme.Shopping);
        var subject = _contentGenerator.GenerateLegitimateSubject(EmailTheme.Shopping);
        var body = _contentGenerator.GenerateLegitimateBody(EmailTheme.Shopping);

        return builder
            .SetSender(senderData)
            .SetSubject(subject)
            .SetBody(body)
            .SetDate(GenerateRecentDate())
            .SetTheme(EmailTheme.Shopping)
            .SetDifficulty(difficulty)
            .Build();
    }

    public IEmail CreateSocialEmail(EmailDifficulty difficulty)
    {
        var builder = new LegitimateEmailBuilder();

        var senderData = _contentGenerator.GenerateLegitimateSender(EmailTheme.Social);
        var subject = _contentGenerator.GenerateLegitimateSubject(EmailTheme.Social);
        var body = _contentGenerator.GenerateLegitimateBody(EmailTheme.Social);

        return builder
            .SetSender(senderData)
            .SetSubject(subject)
            .SetBody(body)
            .SetDate(GenerateRecentDate())
            .SetTheme(EmailTheme.Social)
            .SetDifficulty(difficulty)
            .Build();
    }

    private DateTime GenerateRecentDate()
    {
        int daysAgo = UnityEngine.Random.Range(0, 7);
        return DateTime.Now.AddDays(-daysAgo);
    }

}
