using UnityEngine;

public class EmailManager : MonoBehaviour
{
    [Header("Configuración Principal")]
    public EmailGenerator emailGenerator;
    public ClickableMailUI mailUI;
    public EmailAnalysisManager analysisManager;

    [Header("Templates Disponibles")]
    public EmailTemplate[] emailTemplates;

    private EmailTemplate currentTemplate;

    void Start()
    {
        // Seleccionar template inicial
        if (emailTemplates.Length > 0)
        {
            currentTemplate = emailTemplates[0];
        }
    }

    public void GenerateRandomEmail()
    {
        if (emailGenerator != null && mailUI != null && currentTemplate != null)
        {
            EmailData newEmail = emailGenerator.GetRandomEmailCombination(currentTemplate);
            if (newEmail != null)
            {
                mailUI.SetEmailData(newEmail);
                Debug.Log("Nuevo email generado: " + newEmail.subject);
            }
        }
    }

    public void SetTemplate(int templateIndex)
    {
        if (templateIndex >= 0 && templateIndex < emailTemplates.Length)
        {
            currentTemplate = emailTemplates[templateIndex];
            Debug.Log("Template cambiado a: " + currentTemplate.name);
        }
    }

    public void SetTemplate(EmailTemplate template)
    {
        currentTemplate = template;
        Debug.Log("Template cambiado a: " + currentTemplate.name);
    }
}