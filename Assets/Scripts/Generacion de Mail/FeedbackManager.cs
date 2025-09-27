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


