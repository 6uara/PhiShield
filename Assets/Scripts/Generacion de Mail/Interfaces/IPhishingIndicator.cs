using UnityEngine;

public interface IPhishingIndicator
{
    string Description { get; }
    PhishingIndicatorType Type { get; }
    int SeverityLevel { get; }
}
