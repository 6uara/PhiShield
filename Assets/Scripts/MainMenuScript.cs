using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Singleton pattern implementation
    private static MainMenuManager _instance;
    
    public static MainMenuManager Instance
    {
        get 
        {
            if (_instance == null)
                Debug.LogError("MainMenuManager is null!");
                
            return _instance;
        }
    }
    
    // UI References
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject profilePanel;
    
    [Header("Difficulty Buttons")]
    [SerializeField] private Button bronzeButton;
    [SerializeField] private Button silverButton;
    [SerializeField] private Button goldButton;
    
    [Header("Scene Names")]
    [SerializeField] private string bronzeLevelScene = "BronzeLevelScene";
    [SerializeField] private string silverLevelScene = "SilverLevelScene";
    [SerializeField] private string goldLevelScene = "GoldLevelScene";
    
    [Header("Audio")]
    [SerializeField] private AudioSource menuMusic;
    [SerializeField] private AudioSource buttonSFX;
    
    // Player progress data
    private PlayerProgress _playerProgress;
    
    private void Awake()
    {
        // Singleton pattern implementation
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Initialize player progress
        LoadPlayerProgress();
        
        // Initialize UI states
        UpdateDifficultyButtonsState();
    }
    
    private void Start()
    {
        // Show main panel by default
        ShowMainPanel();
    }
    
    // Panel navigation methods
    public void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        profilePanel.SetActive(true);
        PlayButtonSound();
    }
    
    public void ShowOptionsPanel()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
        difficultyPanel.SetActive(false);
        profilePanel.SetActive(false);
        PlayButtonSound();
    }
    
    public void ShowDifficultyPanel()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        difficultyPanel.SetActive(true);
        profilePanel.SetActive(false);
        PlayButtonSound();
        
        // Update difficulty buttons state before showing panel
        UpdateDifficultyButtonsState();
    }
    
    public void ShowProfilePanel()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        profilePanel.SetActive(true);
        PlayButtonSound();
        
        // Refresh profile data
        UpdateProfileDisplay();
    }
    
    // Game starting methods
    public void StartGameAtDifficulty(int difficulty)
    {
        // 0 = Bronze, 1 = Silver, 2 = Gold
        PlayButtonSound();
        
        // Load appropriate scene based on difficulty
        switch (difficulty)
        {
            case 0:
                SceneManager.LoadScene(bronzeLevelScene);
                break;
            case 1:
                SceneManager.LoadScene(silverLevelScene);
                break;
            case 2:
                SceneManager.LoadScene(goldLevelScene);
                break;
            default:
                Debug.LogError("Invalid difficulty level: " + difficulty);
                break;
        }
        
        // Track game started for analytics
        _playerProgress.gamesPlayed++;
        SavePlayerProgress();
    }
    
    // Player progress methods
    private void LoadPlayerProgress()
    {
        // Initialize with defaults if not found
        _playerProgress = new PlayerProgress();
        
        if (PlayerPrefs.HasKey("PlayerProgressJSON"))
        {
            string json = PlayerPrefs.GetString("PlayerProgressJSON");
            _playerProgress = JsonUtility.FromJson<PlayerProgress>(json);
        }
    }
    
    private void SavePlayerProgress()
    {
        string json = JsonUtility.ToJson(_playerProgress);
        PlayerPrefs.SetString("PlayerProgressJSON", json);
        PlayerPrefs.Save();
    }
    
    // UI update methods
    private void UpdateDifficultyButtonsState()
    {
        // Bronze is always available
        bronzeButton.interactable = true;
        
        // Silver requires Bronze medal
        silverButton.interactable = _playerProgress.hasBronzeMedal;
        
        // Gold requires Silver medal or can be attempted directly
        goldButton.interactable = _playerProgress.hasSilverMedal || _playerProgress.allowDirectGoldAttempt;
    }
    
    private void UpdateProfileDisplay()
    {
        // Update profile display with medals and progress
        // (This would reference actual UI elements in the profile panel)
    }
    
    // Audio methods
    public void SetMusicVolume(float volume)
    {
        menuMusic.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    
    public void SetSFXVolume(float volume)
    {
        buttonSFX.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    
    private void PlayButtonSound()
    {
        if (buttonSFX != null)
            buttonSFX.Play();
    }
    
    // Toggle the direct gold attempt option
    public void ToggleDirectGoldAttempt(bool allow)
    {
        _playerProgress.allowDirectGoldAttempt = allow;
        SavePlayerProgress();
    }
    
    // Exit game
    public void ExitGame()
    {
        PlayButtonSound();
        
        // Save any pending changes
        SavePlayerProgress();
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

// Class to store player progress
[System.Serializable]
public class PlayerProgress
{
    public bool hasBronzeMedal = false;
    public bool hasSilverMedal = false;
    public bool hasGoldMedal = false;
    public bool allowDirectGoldAttempt = true;
    public int bestBronzeScore = 0;
    public int bestSilverScore = 0;
    public int bestGoldScore = 0;
    
    // Analytics data
    public int gamesPlayed = 0;
    public int phishingCorrectlyIdentified = 0;
    public int legitimateCorrectlyIdentified = 0;
    public int totalErrors = 0;
}