using Dovico.CommonLibrary;
using Dovico.OutlookTimeEntryAddin.Business;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Dovico.OutlookTimeEntryAddin.Integration
{
    /// <summary>
    /// This class contains utility integration methods with Dovico.CommonLibrary
    /// </summary>
    public class UtilityService
    {
        const string PASSWORD = "dovico";
        const string SALT = "centric";

        /// <summary>
        /// Method to encrypt data
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EncryptedText(string input)
        {
            // Make request using Dovico.CommonLibrary CEncryption.Encrypt method
            string output = CEncryption.Encrypt(input, PASSWORD, SALT);
            return output;
        }

        /// <summary>
        /// Method to decrypt data
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string DecryptedText(string input)
        {
            // Make request using Dovico.CommonLibrary CEncryption.Decrypt method
            string output = CEncryption.Decrypt(input, PASSWORD, SALT);
            return output;
        }

        /// <summary>
        /// Method to check errors in Dovico API result
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool CheckErrorsInDovicoAPI(string input)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = int.MaxValue;

            // If there is Description key in the data then it is error and then throw DovicoException exception
            dynamic jsonResultRootObject = javaScriptSerializer.DeserializeObject(input);
            if (jsonResultRootObject != null)
            {
                Dictionary<string, object> dictionary = jsonResultRootObject;
                if (dictionary.ContainsKey("Description"))
                {
                    string errorMessage = dictionary["Description"].ToString();
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        throw new DovicoException(errorMessage);
                    }
                }
            }

            return false;
        }
    }
}
