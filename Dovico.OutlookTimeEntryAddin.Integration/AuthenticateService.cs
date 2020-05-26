using Dovico.CommonLibrary;
using Dovico.CommonLibrary.StringBuilders;
using Dovico.OutlookTimeEntryAddin.Common;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Dovico.OutlookTimeEntryAddin.Integration
{
    /// <summary>
    /// This class contains Authentication specific integration methods with Dovico.CommonLibrary
    /// </summary>
    public class AuthenticateService
    {
        /// <summary>
        ///  Method to authenticate user
        /// </summary>
        /// <param name="consumerSecretToken"></param>
        /// <param name="version"></param>
        /// <param name="company"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>string</returns>
        public string Authenticate(string consumerSecretToken, string version, string company, string username, string password)
        {
            string dataAccessToken = string.Empty;

            // Create Post Data
            CStringBuilder postData = new CStringBuilder();
            postData.Append("{");
            postData.Append(string.Concat("\"CompanyName\":\"", company, "\","));
            postData.Append(string.Concat("\"UserName\":\"", username, "\","));
            postData.Append(string.Concat("\"Password\":\"", password, "\""));
            postData.Append("}");

            // Make request using Dovico.CommonLibrary CRestApiHelper.MakeAPIRequest method
            string result = CRestApiHelper.MakeAPIRequest(CRestApiHelper.BuildURI("Authenticate", null, version), "Post", 
                                                            CRestApiHelper.MIME_TYPE_APPLICATION_JSON, postData, 
                                                            consumerSecretToken, null);

            // Check for errors in return value 
            UtilityService.CheckErrorsInDovicoAPI(result);

            // Fetch DataAccessToken from CommonLibrary result
            JavaScriptSerializer objJavaScriptSerializer = new JavaScriptSerializer();
            objJavaScriptSerializer.MaxJsonLength = int.MaxValue;
            dynamic objJsonResultRootObject = objJavaScriptSerializer.DeserializeObject(result);
            Dictionary<string, object> dictionary = objJsonResultRootObject;
            dataAccessToken = dictionary["DataAccessToken"].ToString();

            return dataAccessToken;
        }

        public string GetAPIVersion(string consumerSecretToken, string dataAccessToken, string version)
        {

            // Make request using Dovico.CommonLibrary CRestApiHelper.MakeAPIRequest method
            string result = CRestApiHelper.MakeAPIRequest(CRestApiHelper.BuildURI("APIinfo", null, version), "GET",
                                                            CRestApiHelper.MIME_TYPE_APPLICATION_JSON, String.Empty,
                                                            consumerSecretToken, dataAccessToken);

            // Fetch DataAccessToken from CommonLibrary result
            JavaScriptSerializer objJavaScriptSerializer = new JavaScriptSerializer();
            objJavaScriptSerializer.MaxJsonLength = int.MaxValue;
            dynamic objJsonResultRootObject = objJavaScriptSerializer.DeserializeObject(result);
            if (objJsonResultRootObject != null)
            {
                Dictionary<string, object> dictionary = objJsonResultRootObject[0];
                bool isDovicoBasic = Convert.ToString(dictionary["IsDovicoBasic"]) == "T" ? true : false;
                if (isDovicoBasic || (!isDovicoBasic && Convert.ToInt32(dictionary["MaxVersion"]) == Convert.ToInt32(Constants.Version_5)))
                    version = Constants.Version_5;
                else if (!isDovicoBasic && Convert.ToInt32(dictionary["MaxVersion"]) >= Convert.ToInt32(Constants.Version_7))
                    version = Constants.Version_7;

            }

            return version;
        }
    }
}
