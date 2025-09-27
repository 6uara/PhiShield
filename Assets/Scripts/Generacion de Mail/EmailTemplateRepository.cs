using System.Collections.Generic;
using UnityEngine;


// Implementaci�n usando ScriptableObjects
public class ScriptableObjectTemplateRepository : MonoBehaviour, ITemplateRepository
{
    [SerializeField] private EmailTemplateScriptableObject[] _templateSets;

    // Implementaci�n de los m�todos del repositorio
    public List<string> GetPhishingSenderTemplates(EmailTheme theme)
    {
        foreach (var set in _templateSets)
        {
            if (set.Theme == theme)
                return set.PhishingSenderTemplates;
        }
        return new List<string>();
    }

    // Implementaci�n de los dem�s m�todos...

    public List<string> GetLegitimateSenderTemplates(EmailTheme theme)
    {
        foreach (var set in _templateSets)
        {
            if (set.Theme == theme)
                return set.LegitimateSenderTemplates;
        }
        return new List<string>();
    }

    public List<string> GetPhishingSubjectTemplates(EmailTheme theme)
    {
        foreach (var set in _templateSets)
        {
            if (set.Theme == theme)
                return set.PhishingSubjectTemplates;
        }
        return new List<string>();
    }

    public List<string> GetLegitimateSubjectTemplates(EmailTheme theme)
    {
        foreach (var set in _templateSets)
        {
            if (set.Theme == theme)
                return set.LegitimateSubjectTemplates;
        }
        return new List<string>();
    }

    public List<string> GetPhishingBodyTemplates(EmailTheme theme)
    {
        foreach (var set in _templateSets)
        {
            if (set.Theme == theme)
                return set.PhishingBodyTemplates;
        }
        return new List<string>();
    }

    public List<string> GetLegitimateBodyTemplates(EmailTheme theme)
    {
        foreach (var set in _templateSets)
        {
            if (set.Theme == theme)
                return set.LegitimateBodyTemplates;
        }
        return new List<string>();
    }

    public List<string> GetPhishingIndicatorTemplates(EmailTheme theme)
    {
        foreach (var set in _templateSets)
        {
            if (set.Theme == theme)
                return set.PhishingIndicatorTemplates;
        }
        return new List<string>();
    }

}


