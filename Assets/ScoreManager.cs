// ScoreManager para gestionar el sistema de puntuaci�n
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Configuraci�n del sistema de puntuaci�n
    [Header("Configuraci�n de puntos")]
    [SerializeField] private int _basePointsCorrect = 100;
    [SerializeField] private int _basePointsIncorrect = -50;
    [SerializeField] private float _difficultyMultiplierEasy = 1.0f;
    [SerializeField] private float _difficultyMultiplierMedium = 1.5f;
    [SerializeField] private float _difficultyMultiplierHard = 2.0f;
    [SerializeField] private float _streakMultiplierBase = 0.1f;
    [SerializeField] private int _maxStreakBonus = 5;

    // Estado actual
    [Header("Estado actual")]
    [SerializeField] private int _currentScore = 0;
    [SerializeField] private int _highScore = 0;
    [SerializeField] private int _currentStreak = 0;

    // Estad�sticas
    [Header("Estad�sticas")]
    [SerializeField] private int _totalCorrect = 0;
    [SerializeField] private int _totalIncorrect = 0;
    [SerializeField] private int _phishingCorrect = 0;
    [SerializeField] private int _phishingIncorrect = 0;
    [SerializeField] private int _legitimateCorrect = 0;
    [SerializeField] private int _legitimateIncorrect = 0;

    // Eventos
    public event System.Action<int> OnScoreChanged;
    public event System.Action<int> OnHighScoreChanged;

    private void Awake()
    {
        // Cargar puntaje m�ximo guardado si existe
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Actualizar puntaje basado en identificaci�n de email
    public void UpdateScore(bool isCorrect, EmailDifficulty difficulty)
    {
        int pointsEarned = 0;

        if (isCorrect)
        {
            // Calcular puntos base para respuesta correcta
            pointsEarned = _basePointsCorrect;

            // Aplicar multiplicador por dificultad
            pointsEarned = ApplyDifficultyMultiplier(pointsEarned, difficulty);

            // Incrementar racha actual
            _currentStreak++;

            // Aplicar bonus por racha
            pointsEarned = ApplyStreakBonus(pointsEarned);

            // Actualizar estad�sticas
            _totalCorrect++;
        }
        else
        {
            // Puntos para respuesta incorrecta
            pointsEarned = _basePointsIncorrect;

            // Reiniciar racha
            _currentStreak = 0;

            // Actualizar estad�sticas
            _totalIncorrect++;
        }

        // Actualizar puntaje actual
        _currentScore += pointsEarned;
        if (_currentScore < 0) _currentScore = 0; // Evitar puntajes negativos

        // Verificar si es un nuevo puntaje m�ximo
        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            PlayerPrefs.SetInt("HighScore", _highScore);

            // Notificar sobre cambio en puntaje m�ximo
            if (OnHighScoreChanged != null)
                OnHighScoreChanged(_highScore);
        }

        // Notificar sobre cambio en puntaje actual
        if (OnScoreChanged != null)
            OnScoreChanged(_currentScore);
    }

    // Aplicar multiplicador seg�n dificultad del email
    private int ApplyDifficultyMultiplier(int points, EmailDifficulty difficulty)
    {
        switch (difficulty)
        {
            case EmailDifficulty.Easy:
                return Mathf.RoundToInt(points * _difficultyMultiplierEasy);
            case EmailDifficulty.Medium:
                return Mathf.RoundToInt(points * _difficultyMultiplierMedium);
            case EmailDifficulty.Hard:
                return Mathf.RoundToInt(points * _difficultyMultiplierHard);
            default:
                return points;
        }
    }

    // Aplicar bonus por racha de respuestas correctas
    private int ApplyStreakBonus(int points)
    {
        // Limitar el bonus de racha
        int effectiveStreak = Mathf.Min(_currentStreak - 1, _maxStreakBonus);

        if (effectiveStreak > 0)
        {
            float streakBonus = 1f + (_streakMultiplierBase * effectiveStreak);
            return Mathf.RoundToInt(points * streakBonus);
        }

        return points;
    }

    // Actualizar estad�sticas espec�ficas seg�n tipo de email
    public void UpdateEmailTypeStats(bool isPhishing, bool wasCorrectlyIdentified)
    {
        if (isPhishing)
        {
            if (wasCorrectlyIdentified)
                _phishingCorrect++;
            else
                _phishingIncorrect++;
        }
        else
        {
            if (wasCorrectlyIdentified)
                _legitimateCorrect++;
            else
                _legitimateIncorrect++;
        }
    }

    // M�todo para obtener puntaje actual
    public int GetCurrentScore()
    {
        return _currentScore;
    }

    // M�todo para obtener puntaje m�ximo
    public int GetHighScore()
    {
        return _highScore;
    }

    // M�todo para reiniciar puntaje (por ejemplo, al iniciar un nuevo juego)
    public void ResetScore()
    {
        _currentScore = 0;
        _currentStreak = 0;

        // Notificar sobre cambio en puntaje
        if (OnScoreChanged != null)
            OnScoreChanged(_currentScore);
    }

    // M�todos para obtener estad�sticas
    public float GetAccuracyRate()
    {
        int total = _totalCorrect + _totalIncorrect;
        return total > 0 ? (float)_totalCorrect / total * 100 : 0;
    }

    public float GetPhishingDetectionRate()
    {
        int total = _phishingCorrect + _phishingIncorrect;
        return total > 0 ? (float)_phishingCorrect / total * 100 : 0;
    }

    public float GetLegitimateRecognitionRate()
    {
        int total = _legitimateCorrect + _legitimateIncorrect;
        return total > 0 ? (float)_legitimateCorrect / total * 100 : 0;
    }
}
