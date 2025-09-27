using System.Collections.Generic;
using UnityEngine;

// Facade para la creación y gestión de emails
public class EmailManager : MonoBehaviour
{
    // Implementación Singleton
    private static EmailManager _instance;
    public static EmailManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("EmailManager");
                _instance = go.AddComponent<EmailManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
    // Factory y generador de contenido
    private IEmailFactory _phishingFactory;
    private IEmailFactory _legitimateFactory;
    private IContentGenerator _contentGenerator;
    private ITemplateRepository _templateRepository;

    // Lista de emails generados
    private List<IEmail> _currentEmails = new List<IEmail>();

    // Observadores
    private List<IEmailGenerationObserver> _observers = new List<IEmailGenerationObserver>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Inicializar componentes
        _templateRepository = GetComponent<ScriptableObjectTemplateRepository>();
        if (_templateRepository == null)
            _templateRepository = gameObject.AddComponent<ScriptableObjectTemplateRepository>();

        _contentGenerator = new TemplateBasedContentGenerator(_templateRepository);
        _phishingFactory = new PhishingEmailFactory(_contentGenerator);
        _legitimateFactory = new LegitimateEmailFactory(_contentGenerator);
    }

    // Método para generar un conjunto de emails para un nivel
    public List<IEmail> GenerateEmailsForLevel(int level, int count, float phishingRatio)
    {
        // Limpiar lista anterior
        _currentEmails.Clear();

        // Determinar dificultad basada en nivel
        EmailDifficulty difficulty = DetermineDifficultyByLevel(level);

        // Calcular cuántos emails de phishing
        int phishingCount = Mathf.RoundToInt(count * phishingRatio);
        int legitimateCount = count - phishingCount;

        // Generar emails de phishing
        for (int i = 0; i < phishingCount; i++)
        {
            _currentEmails.Add(GenerateRandomPhishingEmail(difficulty));
        }

        // Generar emails legítimos
        for (int i = 0; i < legitimateCount; i++)
        {
            _currentEmails.Add(GenerateRandomLegitimateEmail(difficulty));
        }

        // Mezclar emails
        ShuffleEmails();

        // Notificar a los observadores
        NotifyObservers();

        return new List<IEmail>(_currentEmails);
    }

    // Generar un email de phishing aleatorio
    private IEmail GenerateRandomPhishingEmail(EmailDifficulty difficulty)
    {
        // Seleccionar un tema aleatorio
        EmailTheme theme = GetRandomTheme();

        // Generar email según el tema
        switch (theme)
        {
            case EmailTheme.Banking:
                return _phishingFactory.CreateBankingEmail(difficulty);
            case EmailTheme.Corporate:
                return _phishingFactory.CreateCorporateEmail(difficulty);
            case EmailTheme.Personal:
                return _phishingFactory.CreatePersonalEmail(difficulty);
            case EmailTheme.Shopping:
                return _phishingFactory.CreateShoppingEmail(difficulty);
            case EmailTheme.Social:
                return _phishingFactory.CreateSocialEmail(difficulty);
            default:
                return _phishingFactory.CreateCorporateEmail(difficulty);
        }
    }

    // Generar un email legítimo aleatorio
    private IEmail GenerateRandomLegitimateEmail(EmailDifficulty difficulty)
    {
        EmailTheme theme = GetRandomTheme();

        switch (theme)
        {
            case EmailTheme.Banking:
                return _legitimateFactory.CreateBankingEmail(difficulty);
            case EmailTheme.Corporate:
                return _legitimateFactory.CreateCorporateEmail(difficulty);
            case EmailTheme.Personal:
                return _legitimateFactory.CreatePersonalEmail(difficulty);
            case EmailTheme.Shopping:
                return _legitimateFactory.CreateShoppingEmail(difficulty);
            case EmailTheme.Social:
                return _legitimateFactory.CreateSocialEmail(difficulty);
            default:
                return _legitimateFactory.CreateCorporateEmail(difficulty);
        }
    }

    // Obtener un tema aleatorio
    private EmailTheme GetRandomTheme()
    {
        return (EmailTheme)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(EmailTheme)).Length);
    }

    // Determinar dificultad por nivel
    private EmailDifficulty DetermineDifficultyByLevel(int level)
    {
        if (level <= 3)
            return EmailDifficulty.Easy;
        else if (level <= 6)
            return EmailDifficulty.Medium;
        else if (level <= 9)
            return EmailDifficulty.Hard;
        else
            return EmailDifficulty.Expert;
    }

    // Mezclar emails
    private void ShuffleEmails()
    {
        int n = _currentEmails.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            IEmail temp = _currentEmails[k];
            _currentEmails[k] = _currentEmails[n];
            _currentEmails[n] = temp;
        }
    }

    // Métodos para el patrón Observer
    public void AddObserver(IEmailGenerationObserver observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void RemoveObserver(IEmailGenerationObserver observer)
    {
        if (_observers.Contains(observer))
            _observers.Remove(observer);
    }

    private void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.OnEmailsGenerated(_currentEmails);
        }
    }

}
