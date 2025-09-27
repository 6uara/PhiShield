using System.Collections.Generic;
using UnityEngine;

// Interfaz para observadores de generaci�n de emails
public interface IEmailGenerationObserver
{
    void OnEmailsGenerated(List<IEmail> emails);
}
