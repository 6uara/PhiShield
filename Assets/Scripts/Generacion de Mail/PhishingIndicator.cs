using System;
using UnityEngine;

public interface IPhishingIndicator
{
    string Description { get; }
    PhishingIndicatorType Type { get; }
    int SeverityLevel { get; }
}

public enum PhishingIndicatorType
{
    SuspiciousSender,
    MaliciousLink,
    SpellingError,
    UrgencyTactic,
    SpoofedDomain,
    DataRequest,
    UnusualAttachment
}

// Implementación concreta de indicador
public class PhishingIndicator : IPhishingIndicator
{
    public string Description { get; private set; }
    public PhishingIndicatorType Type { get; private set; }
    public int SeverityLevel { get; private set; }

    public PhishingIndicator(PhishingIndicatorType type, string description, int severityLevel)
    {
        Type = type;
        Description = description;
        SeverityLevel = Mathf.Clamp(severityLevel, 1, 5);
    }
}