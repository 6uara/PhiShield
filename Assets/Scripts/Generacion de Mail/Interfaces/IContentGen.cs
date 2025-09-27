using System.Collections.Generic;
using UnityEngine;

// Estrategia para generación de contenido
public interface IContentGenerator
{
    string GeneratePhishingSender(EmailTheme theme, EmailDifficulty difficulty);
    string GenerateLegitimateSender(EmailTheme theme);
    string GeneratePhishingSubject(EmailTheme theme, EmailDifficulty difficulty);
    string GenerateLegitimateSubject(EmailTheme theme);
    string GeneratePhishingBody(EmailTheme theme, EmailDifficulty difficulty);
    string GenerateLegitimateBody(EmailTheme theme);
    List<IPhishingIndicator> GeneratePhishingIndicators(EmailTheme theme, EmailDifficulty difficulty);
}
