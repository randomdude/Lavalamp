namespace lavalampData
{
    using System.Security;
    using System;

    public interface IUser
    {
        /// <summary>
        /// the database Id of the user
        /// </summary>
        long? Id { get; set; }
        
        /// <summary>
        /// A string identifying the user
        /// </summary>
        string Name { get; set; }

        string FacialDescription { get; set; }

        /// <summary>
        /// Changes the users password
        /// </summary>
        /// <param name="password">the encrypted user password</param>
        void changeUserPwd(SecureString password);

        /// <summary>
        /// stores a password for a rule item which requires it
        /// this password should be stored encrypted
        /// </summary>
        /// <param name="ruleItem">is this ruleItem guid good/unique enough?</param>
        /// <param name="password"></param>
        void storeRuleItemPwd(Guid ruleItem, SecureString password);

        /// <summary>
        /// retrives a encrypted string for a ruleitem.
        /// </summary>
        /// <param name="ruleItem"></param>
        /// <returns></returns>
        SecureString retrieveRuleItemPwd(Guid ruleItem);

        string hashedPassword();
    }
}
