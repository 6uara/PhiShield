using UnityEngine;

public class EmailGenerator : MonoBehaviour
{
    public EmailData GetRandomEmailCombination(EmailTemplate template)
    {
        if (template == null || template.emailCombinations.Count == 0)
        {
            Debug.LogError("EmailTemplate inválido!");
            return null;
        }

        int randomIndex = Random.Range(0, template.emailCombinations.Count);
        var selected = template.emailCombinations[randomIndex];

        return new EmailData(selected.subject, selected.body, selected.sender, selected.linkText);
    }
}