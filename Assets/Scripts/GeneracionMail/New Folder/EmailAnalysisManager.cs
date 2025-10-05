using UnityEngine;
using TMPro;

public class EmailAnalysisManager : MonoBehaviour
{
    [Header("Sistema de Email")]
    public EmailGenerator emailGenerator;
    public ClickableMailUI mailUI;
    public EmailTemplate currentTemplate;

    [Header("UI de Análisis - TextMeshPro")]
    public TMP_Text analysisResultText;
    public GameObject analysisPanel;

    void Start()
    {
        if (mailUI != null)
        {
            mailUI.OnEmailElementClicked.AddListener(AnalyzeEmailElement);
        }
    }

    public void GenerateNewEmail()
    {
        if (emailGenerator != null && currentTemplate != null)
        {
            EmailData newEmail = emailGenerator.GetRandomEmailCombination(currentTemplate);
            if (newEmail != null && mailUI != null)
            {
                mailUI.SetEmailData(newEmail);
            }
        }
    }

    public void GenerateNewEmail(EmailTemplate template)
    {
        if (emailGenerator != null && mailUI != null)
        {
            EmailData newEmail = emailGenerator.GetRandomEmailCombination(template);
            if (newEmail != null)
            {
                mailUI.SetEmailData(newEmail);
            }
        }
    }

    private void AnalyzeEmailElement(string elementType, string content)
    {
        string analysisResult = "";

        switch (elementType)
        {
            case "Sender":
                analysisResult = AnalyzeSender(content);
                break;
            case "Subject":
                analysisResult = AnalyzeSubject(content);
                break;
            case "Link":
                analysisResult = AnalyzeLink(content);
                break;
        }

        DisplayAnalysisResult(elementType, content, analysisResult);
    }

    private string AnalyzeSender(string sender)
    {
        if (sender.Contains("@"))
        {
            string domain = sender.Split('@')[1];
            if (domain.Contains("suspicious") || domain.Contains("fake"))
                return "Remitente sospechoso detectado";
            else if (domain.Contains("trusted") || domain.Contains("company"))
                return "Remitente verificado";
            else
                return "Remitente parece legítimo";
        }
        return "Formato de email inválido";
    }

    private string AnalyzeSubject(string subject)
    {
        string[] suspiciousWords = { "urgente", "ganador", "premio", "gratis", "oferta", "importante" };
        string[] safeWords = { "factura", "confirmación", "notificación", "actualización" };

        foreach (string word in suspiciousWords)
        {
            if (subject.ToLower().Contains(word))
                return "Asunto contiene palabras sospechosas";
        }

        foreach (string word in safeWords)
        {
            if (subject.ToLower().Contains(word))
                return "Asunto parece legítimo";
        }

        return "Asunto neutral - requiere más análisis";
    }

    private string AnalyzeLink(string link)
    {
        if (link.Contains("http://") && !link.Contains("https://"))
            return "Link no seguro (HTTP en lugar de HTTPS)";

        if (link.Contains(".exe") || link.Contains(".zip") || link.Contains(".rar"))
            return "Link descarga archivos ejecutables - PELIGROSO";

        if (link.Contains("bit.ly") || link.Contains("tinyurl"))
            return "Link acortado - proceder con precaución";

        return "Link parece seguro";
    }

    private void DisplayAnalysisResult(string elementType, string content, string result)
    {
        if (analysisResultText != null)
        {
            analysisResultText.text = $"<b>{elementType}:</b> {content}\n\n<b>Análisis:</b> {result}";
        }

        if (analysisPanel != null)
        {
            analysisPanel.SetActive(true);
        }
    }

    public void CloseAnalysisPanel()
    {
        if (analysisPanel != null)
        {
            analysisPanel.SetActive(false);
        }
    }
}