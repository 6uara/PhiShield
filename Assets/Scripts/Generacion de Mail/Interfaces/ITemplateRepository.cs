using System.Collections.Generic;
using UnityEngine;

// Interfaz para acceder a las plantillas
public interface ITemplateRepository
{
    List<string> GetPhishingSenderTemplates(EmailTheme theme);
    List<string> GetLegitimateSenderTemplates(EmailTheme theme);
    List<string> GetPhishingSubjectTemplates(EmailTheme theme);
    List<string> GetLegitimateSubjectTemplates(EmailTheme theme);
    List<string> GetPhishingBodyTemplates(EmailTheme theme);
    List<string> GetLegitimateBodyTemplates(EmailTheme theme);
    List<string> GetPhishingIndicatorTemplates(EmailTheme theme);
}
