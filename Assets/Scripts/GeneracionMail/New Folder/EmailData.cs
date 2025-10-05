using UnityEngine;

[System.Serializable]
public class EmailData
{
    public string subject;
    public string body;
    public string sender;
    public string linkText;

    public EmailData(string subject, string body, string sender, string linkText)
    {
        this.subject = subject;
        this.body = body;
        this.sender = sender;
        this.linkText = linkText;
    }
}