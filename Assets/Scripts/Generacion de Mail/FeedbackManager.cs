using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Singleton para gestionar el feedback
public class FeedbackManager : MonoBehaviour
{
    private static FeedbackManager _instance;
    public static FeedbackManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("FeedbackManager");
                _instance = go.AddComponent<FeedbackManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
    [SerializeField] private GameObject _feedbackPanelPrefab;
    [SerializeField] private Transform _canvasTransform;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Proporcionar feedback
    public void ProvideFeedback(IEmail email, bool isCorrect, bool userThinkIsPhishing)
    {
        // Crear panel de feedback
        GameObject feedbackObj = Instantiate(_feedbackPanelPrefab, _canvasTransform);
        FeedbackPanel panel = feedbackObj.GetComponent<FeedbackPanel>();

        // Configurar panel
        panel.SetFeedback(email, isCorrect, userThinkIsPhishing);
    }
}
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
            _titleText.text = "¡Correcto!";
            _descriptionText.text = email.IsPhishing ?
                "Has identificado correctamente un email de phishing." :
                "Has identificado correctamente un email legítimo.";
        }
        else
        {
            // Respuesta incorrecta
            _titleText.text = "Incorrecto";
            _descriptionText.text = email.IsPhishing ?
                "Has identificado incorrectamente un email de phishing como legítimo." :
                "Has identificado incorrectamente un email legítimo como phishing.";
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
