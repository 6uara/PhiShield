using System.Collections.Generic;
using UnityEngine;

// Interfaz para observadores de generación de emails
public interface IEmailGenerationObserver
{
    void OnEmailsGenerated(List<IEmail> emails);
}
