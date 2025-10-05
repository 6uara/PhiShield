using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EmailTemplate", menuName = "Email System/Email Template")]
public class EmailTemplate : ScriptableObject
{
    [System.Serializable]
    public class EmailCombination
    {
        public string subject;
        [TextArea(3, 10)]
        public string body;
        public string sender;
        public string linkText;
    }

    public List<EmailCombination> emailCombinations = new List<EmailCombination>();
}