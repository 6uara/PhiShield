using UnityEngine;

// Abstract Factory
public interface IEmailFactory
{
    IEmail CreateBankingEmail(EmailDifficulty difficulty);
    IEmail CreateCorporateEmail(EmailDifficulty difficulty);
    IEmail CreatePersonalEmail(EmailDifficulty difficulty);
    IEmail CreateShoppingEmail(EmailDifficulty difficulty);
    IEmail CreateSocialEmail(EmailDifficulty difficulty);
}
