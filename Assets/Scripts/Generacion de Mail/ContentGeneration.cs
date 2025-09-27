using System;
using System.Collections.Generic;
using UnityEngine;


// Implementación concreta de generador de contenido
public class TemplateBasedContentGenerator : IContentGenerator
{
    private readonly ITemplateRepository _templateRepository;

    public TemplateBasedContentGenerator(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public string GeneratePhishingSender(EmailTheme theme, EmailDifficulty difficulty)
    {
        var templates = _templateRepository.GetPhishingSenderTemplates(theme);
        var template = SelectTemplateByDifficulty(templates, difficulty);
        return ProcessTemplate(template);
    }

    public string GenerateLegitimateSender(EmailTheme theme)
    {
        var templates = _templateRepository.GetLegitimateSenderTemplates(theme);
        return ProcessTemplate(SelectRandomTemplate(templates));
    }

    public string GeneratePhishingSubject(EmailTheme theme, EmailDifficulty difficulty)
    {
        var templates = _templateRepository.GetPhishingSubjectTemplates(theme);
        var template = SelectTemplateByDifficulty(templates, difficulty);
        return ProcessTemplate(template);
    }

    public string GenerateLegitimateSubject(EmailTheme theme)
    {
        var templates = _templateRepository.GetLegitimateSubjectTemplates(theme);
        return ProcessTemplate(SelectRandomTemplate(templates));
    }

    public string GeneratePhishingBody(EmailTheme theme, EmailDifficulty difficulty)
    {
        var templates = _templateRepository.GetPhishingBodyTemplates(theme);
        var template = SelectTemplateByDifficulty(templates, difficulty);
        return ProcessTemplate(template);
    }

    public string GenerateLegitimateBody(EmailTheme theme)
    {
        var templates = _templateRepository.GetLegitimateBodyTemplates(theme);
        return ProcessTemplate(SelectRandomTemplate(templates));
    }

    public List<IPhishingIndicator> GeneratePhishingIndicators(EmailTheme theme, EmailDifficulty difficulty)
    {
        var indicators = new List<IPhishingIndicator>();

        // Número de indicadores basado en la dificultad
        int indicatorCount = difficulty switch
        {
            EmailDifficulty.Easy => UnityEngine.Random.Range(3, 5),
            EmailDifficulty.Medium => UnityEngine.Random.Range(2, 4),
            EmailDifficulty.Hard => UnityEngine.Random.Range(1, 3),
            EmailDifficulty.Expert => UnityEngine.Random.Range(1, 2),
            _ => 3
        };

        // Obtener plantillas de indicadores
        var indicatorTemplates = _templateRepository.GetPhishingIndicatorTemplates(theme);

        // Seleccionar indicadores aleatoriamente
        for (int i = 0; i < indicatorCount; i++)
        {
            if (indicatorTemplates.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, indicatorTemplates.Count);
                var template = indicatorTemplates[index];

                var indicatorType = (PhishingIndicatorType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(PhishingIndicatorType)).Length);
                int severity = CalculateSeverityBasedOnDifficulty(difficulty);

                indicators.Add(new PhishingIndicator(indicatorType, ProcessTemplate(template), severity));

                // Remover para evitar duplicados
                indicatorTemplates.RemoveAt(index);

                if (indicatorTemplates.Count == 0)
                    break;
            }
        }

        return indicators;
    }

    private string SelectRandomTemplate(List<string> templates)
    {
        if (templates == null || templates.Count == 0)
            return "Template not found";

        int index = UnityEngine.Random.Range(0, templates.Count);
        return templates[index];
    }

    private string SelectTemplateByDifficulty(List<string> templates, EmailDifficulty difficulty)
    {
        if (templates == null || templates.Count == 0)
            return "Template not found";

        // Simplificado para ejemplo - idealmente tendríamos templates clasificados por dificultad
        return SelectRandomTemplate(templates);
    }

    private string ProcessTemplate(string template)
    {
        // Procesar variables en la plantilla (ejemplo simple)
        template = template.Replace("{{DATE}}", DateTime.Now.ToString("dd/MM/yyyy"));
        template = template.Replace("{{COMPANY}}", "AcmeCorp");
        template = template.Replace("{{NAME}}", "Usuario");

        return template;
    }

    private int CalculateSeverityBasedOnDifficulty(EmailDifficulty difficulty)
    {
        return difficulty switch
        {
            EmailDifficulty.Easy => UnityEngine.Random.Range(4, 6),     // Muy obvio (4-5)
            EmailDifficulty.Medium => UnityEngine.Random.Range(3, 5),   // Moderado (3-4)
            EmailDifficulty.Hard => UnityEngine.Random.Range(2, 4),     // Sutil (2-3)
            EmailDifficulty.Expert => UnityEngine.Random.Range(1, 3),   // Muy sutil (1-2)
            _ => 3
        };
    }

}
