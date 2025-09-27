using Unity.VisualScripting;
using UnityEngine;

// Componente para mostrar un email
public class EmailView : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro _senderText;
    [SerializeField] private TMPro.TextMeshPro _subjectText;
    [SerializeField] private TMPro.TextMeshPro _bodyText;
    [SerializeField] private TMPro.TextMeshPro _dateText;
    [SerializeField] private GameObject _indicatorsPanel;
    [SerializeField] private GameObject _indicatorPrefab;
    private IEmail _currentEmail;

    public void SetEmail(IEmail email)
    {
        _currentEmail = email;

        // Actualizar UI
        _senderText.text = email.Sender;
        _subjectText.text = email.Subject;
        _bodyText.text = email.Body;
        _dateText.text = email.Date.ToString("dd/MM/yyyy HH:mm");

        // No mostrar indicadores en la UI - son para el análisis
        _indicatorsPanel.SetActive(false);
    }

    // Método para identificar el email
    public void OnIdentifyButtonClicked(bool userThinkIsPhishing)
    {
        // Comprobar si la identificación es correcta
        bool isCorrect = (userThinkIsPhishing == _currentEmail.IsPhishing);

        // Notificar al sistema de feedback
        FeedbackManager.Instance.ProvideFeedback(_currentEmail, isCorrect, userThinkIsPhishing);
    }

    // Método para mostrar indicadores (modo de aprendizaje/feedback)
    public void ShowIndicators()
    {
        if (_currentEmail.IsPhishing)
        {
            _indicatorsPanel.SetActive(true);

            // Limpiar indicadores existentes
            foreach (Transform child in _indicatorsPanel.transform)
            {
                Destroy(child.gameObject);
            }

            // Crear nuevos indicadores
            var indicators = _currentEmail.GetPhishingIndicators();
            foreach (var indicator in indicators)
            {
                GameObject indicatorObj = Instantiate(_indicatorPrefab, _indicatorsPanel.transform);
                TMPro.TextMeshPro indicatorText = indicatorObj.GetComponentInChildren<TMPro.TextMeshPro>();
                indicatorText.text = $"{indicator.Type}: {indicator.Description} (Nivel: {indicator.SeverityLevel})";
            }
        }
    }

}