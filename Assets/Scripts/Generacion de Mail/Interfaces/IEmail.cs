using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEmail
{
    string Sender { get; }
    string Subject { get; }
    string Body { get; }
    DateTime Date { get; }
    bool IsPhishing { get; }
    EmailTheme Theme { get; }
    EmailDifficulty Difficulty { get; }
    List<IPhishingIndicator> GetPhishingIndicators();
}
