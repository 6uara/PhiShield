using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// GameManager como Singleton para controlar el flujo del juego
public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("GameManager");
                _instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // Referencias a otros sistemas
    [SerializeField] private EmailManager _emailManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private ScoreManager _scoreManager;

    // Configuración del juego
    [Header("Configuración de niveles")]
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private int _emailsPerLevel = 10;
    [SerializeField] private float _phishingRatio = 0.5f; // 50% phishing, 50% legítimos
    [SerializeField] private int _currentEmailIndex = 0;

    // Estado del juego
    private List<IEmail> _currentLevelEmails;
    private bool _levelInProgress = false;
    private int _correctIdentifications = 0;
    private int _wrongIdentifications = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Inicializar componentes si no están asignados
        if (_scoreManager == null)
            _scoreManager = FindFirstObjectByType<ScoreManager>();

        if (_uiManager == null)
            _uiManager = FindFirstObjectByType<UIManager>();

        if (_emailManager == null)
            _emailManager = FindFirstObjectByType<EmailManager>();
    }

    private void Start()
    {
        // Iniciar el primer nivel
        StartLevel();
    }

    public void StartLevel()
    {
        // Reiniciar contadores para el nivel
        _currentEmailIndex = 0;
        _correctIdentifications = 0;
        _wrongIdentifications = 0;
        _levelInProgress = true;

        // Generar emails para el nivel actual
        _currentLevelEmails = _emailManager.GenerateEmailsForLevel(_currentLevel, _emailsPerLevel, _phishingRatio);

        // Actualizar UI con información del nivel
        _uiManager.UpdateLevelInfo(_currentLevel);

        // Cargar el primer email
        LoadNextEmail();
    }

    public void IdentifyEmail(bool userThinkIsPhishing)
    {
        if (!_levelInProgress)
            return;

        IEmail currentEmail = _currentLevelEmails[_currentEmailIndex - 1];
        bool isCorrect = (userThinkIsPhishing == currentEmail.IsPhishing);

        // Actualizar estadísticas
        if (isCorrect)
            _correctIdentifications++;
        else
            _wrongIdentifications++;

        // Actualizar puntaje
        _scoreManager.UpdateScore(isCorrect, currentEmail.Difficulty);

        // Mostrar feedback
        _uiManager.ShowEmailFeedback(currentEmail, isCorrect, userThinkIsPhishing);

        // Comprobar si debemos continuar al siguiente email
        CheckLevelProgress();
    }

    private void LoadNextEmail()
    {
        if (_currentEmailIndex < _currentLevelEmails.Count)
        {
            // Mostrar el siguiente email
            _uiManager.DisplayEmail(_currentLevelEmails[_currentEmailIndex]);
            _currentEmailIndex++;
        }
        else
        {
            // No hay más emails en este nivel
            EndLevel();
        }
    }

    private void CheckLevelProgress()
    {
        if (_currentEmailIndex >= _currentLevelEmails.Count)
        {
            // No hay más emails, terminar nivel
            EndLevel();
        }
        else
        {
            // Programar carga del siguiente email después del feedback
            Invoke("LoadNextEmail", 2.0f);
        }
    }

    private void EndLevel()
    {
        _levelInProgress = false;

        // Calcular resultados del nivel
        float successRate = (float)_correctIdentifications / _emailsPerLevel * 100;
        bool levelPassed = successRate >= GetRequiredSuccessRate();

        // Mostrar resultados del nivel
        _uiManager.ShowLevelResults(_currentLevel, _correctIdentifications, _wrongIdentifications, successRate, levelPassed);

        if (levelPassed)
        {
            // Preparar para el siguiente nivel
            _currentLevel++;

            // Ajustar dificultad incrementando ratio de phishing y disminuyendo indicadores obvios
            if (_currentLevel > 5)
            {
                _phishingRatio = Mathf.Min(_phishingRatio + 0.1f, 0.8f);
                _emailsPerLevel = Mathf.Min(_emailsPerLevel + 2, 20);
            }
        }
    }

    // Determinar el porcentaje de éxito requerido según el nivel
    private float GetRequiredSuccessRate()
    {
        // Más exigente en niveles avanzados
        if (_currentLevel <= 2)
            return 70f;  // 70% para niveles iniciales
        else if (_currentLevel <= 5)
            return 75f;  // 75% para niveles intermedios
        else
            return 80f;  // 80% para niveles avanzados
    }

    // Método público para continuar al siguiente nivel después de mostrar resultados
    public void ContinueToNextLevel()
    {
        StartLevel();
    }

    // Método público para reintentar el nivel actual
    public void RetryLevel()
    {
        StartLevel();
    }
     
    // Obtener información del progreso actual
    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public int GetTotalEmailsInLevel()
    {
        return _emailsPerLevel;
    }

    public int GetCurrentEmailIndex()
    {
        return _currentEmailIndex;
    }
}