using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EmailTemplates", menuName = "PhiShield/Email Templates")]
// ScriptableObject para almacenar plantillas
public class EmailTemplateScriptableObject : ScriptableObject
{
    public EmailTheme Theme;

    public List<string> PhishingSenderTemplates;
    public List<string> LegitimateSenderTemplates;
    public List<string> PhishingSubjectTemplates;
    public List<string> LegitimateSubjectTemplates;
    public List<string> PhishingBodyTemplates;
    public List<string> LegitimateBodyTemplates;
    public List<string> PhishingIndicatorTemplates;

}
