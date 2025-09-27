using System.Collections.Generic;
using UnityEngine;

// Interfaz para observadores de generación de emails
public interface IEmailGenerationObserver
{
    void OnEmailsGenerated(List<IEmail> emails);
}
// Implementación concreta para el UI
public class EmailUIController : MonoBehaviour, IEmailGenerationObserver
{
    [SerializeField] private GameObject _emailPrefab;
    [SerializeField] private Transform _emailContainer;

    private void Start()
    {
        // Registrarse como observador
        EmailManager.Instance.AddObserver(this);
    }

    private void OnDestroy()
    {
        // Cancelar registro al destruir
        if (EmailManager.Instance != null)
            EmailManager.Instance.RemoveObserver(this);
    }

    // Implementación del método de observador
    public void OnEmailsGenerated(List<IEmail> emails)
    {
        // Limpiar emails existentes
        foreach (Transform child in _emailContainer)
        {
            Destroy(child.gameObject);
        }

        // Crear nuevos emails
        foreach (var email in emails)
        {
            GameObject emailObj = Instantiate(_emailPrefab, _emailContainer);
            EmailView emailView = emailObj.GetComponent<EmailView>();
            emailView.SetEmail(email);
        }
    }

}
