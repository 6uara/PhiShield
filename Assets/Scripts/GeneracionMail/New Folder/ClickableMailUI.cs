using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class EmailElementEvent : UnityEvent<string, string> { }

public class ClickableMailUI : MonoBehaviour
{
    [Header("Referencias de Botones")]
    public Button senderButton;
    public Button subjectButton;
    public Button linkButton;

    [Header("Referencias de Texto - TextMeshPro")]
    public TMP_Text senderButtonText;
    public TMP_Text subjectButtonText;
    public TMP_Text linkButtonText;
    public TMP_Text bodyText;

    [Header("Eventos")]
    public EmailElementEvent OnEmailElementClicked;

    private EmailData currentEmailData;

    void Start()
    {
        // Configurar listeners
        senderButton.onClick.AddListener(() => OnButtonClicked("Sender", currentEmailData?.sender));
        subjectButton.onClick.AddListener(() => OnButtonClicked("Subject", currentEmailData?.subject));
        linkButton.onClick.AddListener(() => OnButtonClicked("Link", currentEmailData?.linkText));
    }

    public void SetEmailData(EmailData emailData)
    {
        currentEmailData = emailData;

        // Solo actualizar textos - las posiciones las maneja el Layout
        senderButtonText.text = emailData.sender;
        subjectButtonText.text = emailData.subject;
        linkButtonText.text = emailData.linkText;
        bodyText.text = emailData.body;
    }

    private void OnButtonClicked(string elementType, string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            Debug.Log($"{elementType} clickeado: {content}");
            OnEmailElementClicked?.Invoke(elementType, content);
        }
    }

    public EmailData GetCurrentEmailData()
    {
        return currentEmailData;
    }
}