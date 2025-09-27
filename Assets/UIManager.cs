// UIManager para gestionar interfaces y presentación de emails
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Paneles principales")]
    [SerializeField] private GameObject _emailPanel;
    [SerializeField] private GameObject _feedbackPanel;
    [SerializeField] private GameObject _resultsPanel;
    [SerializeField] private GameObject _levelInfoPanel;

    [Header("Elementos de email")]
    [SerializeField] private TMPro.TextMeshPro _senderText;
    [SerializeField] private TMPro.TextMeshPro _subjectText;
    [SerializeField] private TMPro.TextMeshPro _bodyText;
    [SerializeField] private TMPro.TextMeshPro _dateText;
    [SerializeField] private Image _emailThemeIcon;

    [Header("Elementos de UI")]
    [SerializeField] private TMPro.TextMeshPro _levelText;
    [SerializeField] private TMPro.TextMeshPro _emailCountText;
    [SerializeField] private TMPro.TextMeshPro _scoreText;
    [SerializeField] private Button _phishingButton;
    [SerializeField] private Button _legitimateButton;

    [Header("Elementos de feedback")]
    [SerializeField] private TMPro.TextMeshPro _feedbackHeaderText;
    [SerializeField] private TMPro.TextMeshPro _feedbackDetailText;
    [SerializeField] private Image _feedbackIcon;
    [SerializeField] private Button _continueButton;

    [Header("Elementos de resultados de nivel")]
    [SerializeField] private TMPro.TextMeshPro _levelResultHeaderText;
    [SerializeField] private TMPro.TextMeshPro _correctAnswersText;
    [SerializeField] private TMPro.TextMeshPro _incorrectAnswersText;
    [SerializeField] private TMPro.TextMeshPro _successRateText;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _retryLevelButton;

    [Header("Recursos visuales")]
    [SerializeField] private Sprite _successIcon;
    [SerializeField] private Sprite _failureIcon;
    [SerializeField] private Sprite[] _themeIcons; // Íconos para los diferentes temas de email

    // Referencias a otros managers
    private GameManager _gameManager;
    private ScoreManager _scoreManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _scoreManager = FindObjectOfType<ScoreManager>();

        // Configurar botones
        _phishingButton.onClick.AddListener(() => IdentifyCurrentEmail(true));
        _legitimateButton.onClick.AddListener(() => IdentifyCurrentEmail(false));
        _continueButton.onClick.AddListener(CloseFeedback);
        _nextLevelButton.onClick.AddListener(ContinueToNextLevel);
        _retryLevelButton.onClick.AddListener(RetryCurrentLevel);

        // Ocultar paneles inicialmente
        _feedbackPanel.SetActive(false);
        _resultsPanel.SetActive(false);
    }

    // Mostrar un email en la interfaz
    public void DisplayEmail(IEmail email)
    {
        // Actualizar los campos de UI con la información del email
        _senderText.text = email.Sender;
        _subjectText.text = email.Subject;
        _bodyText.text = email.Body;
        _dateText.text = email.Date.ToString("dd/MM/yyyy HH:mm");

        // Seleccionar ícono según el tema del email
        _emailThemeIcon.sprite = GetIconForEmailTheme(email.Theme);

        // Actualizar contador de emails
        UpdateEmailCounter();

        // Mostrar panel de email y habilitar botones
        _emailPanel.SetActive(true);
        _phishingButton.interactable = true;
        _legitimateButton.interactable = true;
    }

    // Actualizar información del nivel actual
    public void UpdateLevelInfo(int level)
    {
        _levelText.text = "Nivel " + level;
        UpdateEmailCounter();
        UpdateScore();
    }

    // Actualizar contador de emails procesados/totales
    private void UpdateEmailCounter()
    {
        int current = _gameManager.GetCurrentEmailIndex();
        int total = _gameManager.GetTotalEmailsInLevel();
        _emailCountText.text = string.Format("Email {0} de {1}", current, total);
    }

    // Actualizar puntaje en pantalla
    private void UpdateScore()
    {
        _scoreText.text = "Puntaje: " + _scoreManager.GetCurrentScore();
    }

    // Procesar la identificación del usuario del email actual
    private void IdentifyCurrentEmail(bool isPhishing)
    {
        // Deshabilitar botones para evitar clics múltiples
        _phishingButton.interactable = false;
        _legitimateButton.interactable = false;

        // Notificar al GameManager sobre la decisión
        _gameManager.IdentifyEmail(isPhishing);
    }

    // Mostrar feedback después de identificar un email
    public void ShowEmailFeedback(IEmail email, bool wasCorrect, bool userSelectedPhishing)
    {
        // Ocultar panel de email
        _emailPanel.SetActive(false);

        // Configurar el panel de feedback
        if (wasCorrect)
        {
            _feedbackHeaderText.text = "¡Correcto!";
            _feedbackIcon.sprite = _successIcon;
            _feedbackHeaderText.color = Color.green;
        }
        else
        {
            _feedbackHeaderText.text = "Incorrecto";
            _feedbackIcon.sprite = _failureIcon;
            _feedbackHeaderText.color = Color.red;
        }

        // Preparar texto detallado de feedback
        string feedbackDetails = "";
        if (email.IsPhishing)
        {
            feedbackDetails += "Este era un email de phishing.\n\n";
            feedbackDetails += "Indicadores de phishing detectados:\n";

            var indicators = email.GetPhishingIndicators();
            foreach (var indicator in indicators)
            {
                //feedbackDetails += "• " + indicator.GetDescription() + "\n";
            }
        }
        else
        {
            feedbackDetails += "Este era un email legítimo.\n\n";
            feedbackDetails += "Características de seguridad:\n";
            feedbackDetails += "• Remitente verificado\n";
            feedbackDetails += "• No solicita información sensible\n";
            feedbackDetails += "• Enlaces seguros\n";
        }

        _feedbackDetailText.text = feedbackDetails;

        // Mostrar panel de feedback
        _feedbackPanel.SetActive(true);

        // Actualizar puntaje en pantalla
        UpdateScore();
    }

    // Cerrar el panel de feedback y continuar
    private void CloseFeedback()
    {
        _feedbackPanel.SetActive(false);
    }

    // Mostrar resultados del nivel
    public void ShowLevelResults(int level, int correctAnswers, int incorrectAnswers, float successRate, bool levelPassed)
    {
        // Ocultar otros paneles
        _emailPanel.SetActive(false);
        _feedbackPanel.SetActive(false);

        // Configurar panel de resultados
        _levelResultHeaderText.text = levelPassed ?
            "¡Nivel " + level + " Completado!" :
            "Nivel " + level + " No Superado";

        _correctAnswersText.text = "Respuestas correctas: " + correctAnswers;
        _incorrectAnswersText.text = "Respuestas incorrectas: " + incorrectAnswers;
        _successRateText.text = "Tasa de éxito: " + successRate.ToString("0.0") + "%";

        // Habilitar/deshabilitar botones según el resultado
        _nextLevelButton.gameObject.SetActive(levelPassed);
        _retryLevelButton.gameObject.SetActive(true);

        // Mostrar panel de resultados
        _resultsPanel.SetActive(true);
    }

    // Continuar al siguiente nivel
    private void ContinueToNextLevel()
    {
        _resultsPanel.SetActive(false);
        _gameManager.ContinueToNextLevel();
    }

    // Reintentar el nivel actual
    private void RetryCurrentLevel()
    {
        _resultsPanel.SetActive(false);
        _gameManager.RetryLevel();
    }

    // Obtener ícono correspondiente al tema del email
    private Sprite GetIconForEmailTheme(EmailTheme theme)
    {
        int themeIndex = (int)theme;
        if (themeIndex >= 0 && themeIndex < _themeIcons.Length)
            return _themeIcons[themeIndex];
        else
            return _themeIcons[0]; // Ícono por defecto
    }
}
