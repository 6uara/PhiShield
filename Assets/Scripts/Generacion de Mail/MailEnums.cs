using System;
using System.Collections.Generic;
using UnityEngine;

// Enumeraciones para categorización de emails
public enum EmailTheme
{
    Banking,
    Corporate,
    Personal,
    Shopping,
    Social
}

public enum EmailDifficulty
{
    Easy,
    Medium,
    Hard,
    Expert
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