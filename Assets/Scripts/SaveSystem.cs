using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    // Datos de perfil del jugador
    public string playerName;
    public string profilePictureBase64;
    
    // Progreso de niveles
    public Dictionary<string, bool> CompletedLevels = new Dictionary<string, bool>();
    
    // Preferencias
    public float sfxVolume = 1.0f;
    public float musicVolume = 1.0f;
    public int resolutionIndex = 0;
    public bool fullscreen = true;
}

public class SaveSystem : MonoBehaviour
{
    private static SaveSystem _instance;
    public static SaveSystem Instance 
    { 
        get 
        { 
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SaveSystem>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("SaveSystem");
                    _instance = go.AddComponent<SaveSystem>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    
    // Ruta del archivo de guardado
    private string SavePath => Path.Combine(Application.persistentDataPath, "playerData.json");
    
    // Datos del jugador
    private PlayerData _playerData;
    
    // Referencias a otros managers
    private OptionsManager _optionsManager;
    
    // Imagen de perfil
    public Image profileImage;
    public RawImage profileRawImage;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Inicializar datos
        LoadData();
    }
    
    private void Start()
    {
        // Obtener referencia al OptionsManager
        _optionsManager = FindFirstObjectByType<OptionsManager>();
        
        // Aplicar datos cargados
        ApplyLoadedData();
    }
    
    public void LoadData()
    {
        if (File.Exists(SavePath))
        {
            try
            {
                string json = File.ReadAllText(SavePath);
                _playerData = JsonUtility.FromJson<PlayerData>(json);
                
                // Para diccionarios, necesitamos una solución personalizada
                // ya que JsonUtility no maneja diccionarios nativamente
                string levelDataPath = Path.Combine(Application.persistentDataPath, "levelData.json");
                if (File.Exists(levelDataPath))
                {
                    json = File.ReadAllText(levelDataPath);
                    LevelDataWrapper wrapper = JsonUtility.FromJson<LevelDataWrapper>(json);
                    _playerData.CompletedLevels = new Dictionary<string, bool>();
                    
                    foreach (LevelData level in wrapper.levels)
                    {
                        _playerData.CompletedLevels[level.levelId] = level.completed;
                    }
                }
                
                Debug.Log("Datos cargados correctamente");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al cargar datos: " + e.Message);
                CreateNewData();
            }
        }
        else
        {
            CreateNewData();
        }
    }
    
    private void CreateNewData()
    {
        _playerData = new PlayerData();
        _playerData.playerName = "Usuario";
        _playerData.sfxVolume = 1.0f;
        _playerData.musicVolume = 1.0f;
        SaveData();
    }
    
    public void SaveData()
    {
        try
        {
            // Actualizar datos desde el OptionsManager si está disponible
            if (_optionsManager != null)
            {
                _playerData.sfxVolume = _optionsManager.sfxVolumeSlider.value;
                _playerData.musicVolume = _optionsManager.musicVolumeSlider.value;
                _playerData.resolutionIndex = _optionsManager.resolutionDropdown.value;
                _playerData.fullscreen = Screen.fullScreen;
            }
            
            string json = JsonUtility.ToJson(_playerData, true);
            File.WriteAllText(SavePath, json);
            
            // Guardar el diccionario de niveles completados
            SaveLevelData();
            
            Debug.Log("Datos guardados correctamente");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al guardar datos: " + e.Message);
        }
    }
    
    // Clase auxiliar para guardar diccionarios
    [System.Serializable]
    private class LevelData
    {
        public string levelId;
        public bool completed;
    }
    
    [System.Serializable]
    private class LevelDataWrapper
    {
        public List<LevelData> levels = new List<LevelData>();
    }
    
    private void SaveLevelData()
    {
        LevelDataWrapper wrapper = new LevelDataWrapper();
        foreach (var kvp in _playerData.CompletedLevels)
        {
            wrapper.levels.Add(new LevelData { levelId = kvp.Key, completed = kvp.Value });
        }
        
        string json = JsonUtility.ToJson(wrapper, true);
        string levelDataPath = Path.Combine(Application.persistentDataPath, "levelData.json");
        File.WriteAllText(levelDataPath, json);
    }
    
    private void ApplyLoadedData()
    {
        if (_optionsManager != null)
        {
            // Aplicar volumen
            _optionsManager.sfxVolumeSlider.value = _playerData.sfxVolume;
            _optionsManager.musicVolumeSlider.value = _playerData.musicVolume;
            
            // Aplicar resolución
            if (_optionsManager.resolutionDropdown.options.Count > _playerData.resolutionIndex)
            {
                _optionsManager.resolutionDropdown.value = _playerData.resolutionIndex;
                _optionsManager.SetResolution(_playerData.resolutionIndex);
            }
            
            // Aplicar pantalla completa
            Screen.fullScreen = _playerData.fullscreen;
        }
        
        // Cargar imagen de perfil si existe
        LoadProfileImage();
    }
    
    public void SetPlayerName(string username)
    {
        _playerData.playerName = username;
        SaveData();
    }
    
    public string GetPlayerName()
    {
        return _playerData.playerName;
    }
    
    public void MarkLevelCompleted(string levelId, bool completed = true)
    {
        _playerData.CompletedLevels[levelId] = completed;
        SaveData();
    }
    
    public bool IsLevelCompleted(string levelId)
    {
        if (_playerData.CompletedLevels.ContainsKey(levelId))
        {
            return _playerData.CompletedLevels[levelId];
        }
        return false;
    }
    
    public void SaveProfilePicture(Texture2D texture)
    {
        if (texture != null)
        {
            try
            {
                // Convertir la textura a base64
                byte[] textureData = texture.EncodeToPNG();
                _playerData.profilePictureBase64 = System.Convert.ToBase64String(textureData);
                SaveData();
                
                // Actualizar la imagen en la UI
                if (profileImage != null)
                {
                    Sprite sprite = Sprite.Create(
                        texture, 
                        new Rect(0, 0, texture.width, texture.height), 
                        new Vector2(0.5f, 0.5f)
                    );
                    profileImage.sprite = sprite;
                }
                
                if (profileRawImage != null)
                {
                    profileRawImage.texture = texture;
                }
                
                Debug.Log("Imagen de perfil guardada correctamente");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al guardar imagen de perfil: " + e.Message);
            }
        }
    }
    
    private void LoadProfileImage()
    {
        if (!string.IsNullOrEmpty(_playerData.profilePictureBase64))
        {
            try
            {
                byte[] textureData = System.Convert.FromBase64String(_playerData.profilePictureBase64);
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(textureData))
                {
                    // Actualizar la imagen en la UI
                    if (profileImage != null)
                    {
                        Sprite sprite = Sprite.Create(
                            texture, 
                            new Rect(0, 0, texture.width, texture.height), 
                            new Vector2(0.5f, 0.5f)
                        );
                        profileImage.sprite = sprite;
                    }
                    
                    if (profileRawImage != null)
                    {
                        profileRawImage.texture = texture;
                    }
                    
                    Debug.Log("Imagen de perfil cargada correctamente");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al cargar imagen de perfil: " + e.Message);
            }
        }
    }
}
