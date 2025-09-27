using UnityEngine;
using UnityEngine.UI;


// Panel de feedback
public class FeedbackPanel : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro _titleText;
    [SerializeField] private TMPro.TextMeshPro _descriptionText;
    [SerializeField] private GameObject _indicatorsPanel;
    [SerializeField] private GameObject _indicatorPrefab;
    [SerializeField] private Button _closeButton;
    private void Start()
    {
        _closeButton.onClick.AddListener(ClosePanel);
    }

    public void SetFeedback(IEmail email, bool isCorrect, bool userThinkIsPhishing)
    {
        if (isCorrect)
        {
            // Respuesta correcta
            _titleText.text = "�Correcto!";
            _descriptionText.text = email.IsPhishing ?
                "Has identificado correctamente un email de phishing." :
                "Has identificado correctamente un email leg�timo.";
        }
        else
        {
            // Respuesta incorrecta
            _titleText.text = "Incorrecto";
            _descriptionText.text = email.IsPhishing ?
                "Has identificado incorrectamente un email de phishing como leg�timo." :
                "Has identificado incorrectamente un email leg�timo como phishing.";
        }

        // Mostrar indicadores si es phishing
        if (email.IsPhishing)
        {
            _indicatorsPanel.SetActive(true);

            // Limpiar indicadores existentes
            foreach (Transform child in _indicatorsPanel.transform)
            {
                Destroy(child.gameObject);
            }

            // Crear nuevos indicadores
            var indicators = email.GetPhishingIndicators();
            foreach (var indicator in indicators)
            {
                GameObject indicatorObj = Instantiate(_indicatorPrefab, _indicatorsPanel.transform);
                Text indicatorText = indicatorObj.GetComponentInChildren<Text>();
                indicatorText.text = $"{indicator.Type}: {indicator.Description} (Nivel: {indicator.SeverityLevel})";
            }
        }
        else
        {
            _indicatorsPanel.SetActive(false);
        }
    }

    private void ClosePanel()
    {
        Destroy(gameObject);
    }
}
