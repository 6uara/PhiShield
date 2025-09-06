using UnityEngine;
using SimpleFileBrowser;
using System.Collections;
using System.IO;
public class ProfileImageLoader : MonoBehaviour
{
    [Header("UI References")]
    public UnityEngine.UI.Image userPhotoDisplay;
    public UnityEngine.UI.Button uploadButton;
    
    [Header("Settings")]
    public int maxImageSize = 512; // Tamaño máximo en píxeles
    public bool saveToPlayerPrefs = true;
    public string playerPrefsKey = "PhiShieldUserPhoto";
    
    private Texture2D currentTexture;
    
    private void Start()
    {
        // Asignar listener al botón
        if (uploadButton != null)
            uploadButton.onClick.AddListener(OpenPhotoBrowser);
            
        // Cargar imagen guardada (si existe)
        if (saveToPlayerPrefs && PlayerPrefs.HasKey(playerPrefsKey))
        {
            LoadSavedImage();
        }
    }
    
    public void OpenPhotoBrowser()
    {
        // Configurar el navegador
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Imágenes", ".jpg", ".png", ".jpeg"));
        FileBrowser.SetDefaultFilter(".jpg");
        
        // Personalizar apariencia para estilo retro
        //FileBrowser.SetSkin(FileBrowserHelpers.Skin.Dark); // O Light, o Custom
        
        StartCoroutine(ShowBrowser());
    }
    
    private IEnumerator ShowBrowser()
    {
        yield return FileBrowser.WaitForLoadDialog(
            FileBrowser.PickMode.Files,
            false,
            null,
            null,
            "Seleccionar imagen de perfil",
            "Cargar"
        );
        
        if (FileBrowser.Success)
        {
            string filePath = FileBrowser.Result[0];
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);
            ProcessImageData(fileData);
        }
    }
    
    private void ProcessImageData(byte[] imageData)
    {
        // Crear textura desde los datos
        currentTexture = new Texture2D(2, 2);
        currentTexture.LoadImage(imageData);
        
        // Redimensionar si es necesario
        if (currentTexture.width > maxImageSize || currentTexture.height > maxImageSize)
        {
            currentTexture = ResizeTexture(currentTexture, maxImageSize);
        }
        
        // Mostrar en UI
        DisplayTextureInUI(currentTexture);
        
        // Guardar si está habilitado
        if (saveToPlayerPrefs)
        {
            SaveTextureToPlayerPrefs(currentTexture);
        }
    }
    
    private void DisplayTextureInUI(Texture2D texture)
    {
        if (userPhotoDisplay != null)
        {
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
            userPhotoDisplay.sprite = sprite;
        }
    }
    
    private Texture2D ResizeTexture(Texture2D source, int maxSize)
    {
        int width = source.width;
        int height = source.height;
        
        // Calcular nueva dimensión manteniendo proporción
        float ratio = (float)width / height;
        
        if (width > height)
        {
            width = maxSize;
            height = Mathf.RoundToInt(width / ratio);
        }
        else
        {
            height = maxSize;
            width = Mathf.RoundToInt(height * ratio);
        }
        
        // Crear textura redimensionada
        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(source, rt);
        
        RenderTexture prevRT = RenderTexture.active;
        RenderTexture.active = rt;
        
        Texture2D resized = new Texture2D(width, height);
        resized.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        resized.Apply();
        
        RenderTexture.active = prevRT;
        RenderTexture.ReleaseTemporary(rt);
        
        return resized;
    }
    
    private void SaveTextureToPlayerPrefs(Texture2D texture)
    {
        byte[] pngData = texture.EncodeToPNG();
        string base64Data = System.Convert.ToBase64String(pngData);
        PlayerPrefs.SetString(playerPrefsKey, base64Data);
        PlayerPrefs.Save();
    }
    
    private void LoadSavedImage()
    {
        string base64Data = PlayerPrefs.GetString(playerPrefsKey);
        if (!string.IsNullOrEmpty(base64Data))
        {
            try
            {
                byte[] pngData = System.Convert.FromBase64String(base64Data);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(pngData);
                DisplayTextureInUI(texture);
                currentTexture = texture;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al cargar imagen guardada: " + e.Message);
            }
        }
    }
}