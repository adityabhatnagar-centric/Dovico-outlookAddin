using Dovico.OutlookTimeEntryAddin.Business;
using Dovico.OutlookTimeEntryAddin.Integration;
using NLog;
using System;

namespace Dovico.OutlookTimeEntryAddin.Application
{
    /// <summary>
    /// This class contains Authentication specific logic
    /// </summary>
    public class AuthenticateLogic
    {
        // For error logging
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Method to authenticate user
        /// </summary>
        /// <param name="company"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserBO AuthenticateUser(string company, string username, string password)
        {
            string dataAccessToken = string.Empty;
            string encryptedDataAccessToken = string.Empty;
            AuthenticateService integrationService = new AuthenticateService();
            UserBO user = new UserBO();

            try
            {
                // Check for company, username, password required validation and then call Authenticate of Integration layer
                if (!String.IsNullOrEmpty(company))
                {
                    if(!String.IsNullOrEmpty(username))
                    {
                        if(!String.IsNullOrEmpty(password))
                        {
                            dataAccessToken = integrationService.Authenticate(Constants.CONSUMER_SECRET_TOKEN, Constants.VERSION_5, company, username, password);
                            user.DataAccessToken = UtilityService.EncryptedText(dataAccessToken);
                        }
                        else
                        {
                            throw new Exception("Password cannot be Empty");
                        }
                    }
                    else
                    {
                        throw new Exception("Username cannot be Empty");
                    }
                }
                else
                {
                    throw new Exception("Company cannot be Empty");
                }
            }
            catch(DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
                user.ErrorMessage = e.Message;
            }
            catch(Exception e)
            {
                logger.Log(LogLevel.Error, e);
                user.ErrorMessage = e.Message;
            }

            return user;
        }

        /// <summary>
        /// Method to Fetch API Version
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <returns></returns>
        public string GetAPIVersion(string dataAccessToken)
        {
            string apiVersion = string.Empty;
            string encryptedDataAccessToken = string.Empty;
            AuthenticateService integrationService = new AuthenticateService();
            try
            {
                apiVersion = integrationService.GetAPIVersion(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken), Constants.VERSION_5);
            }
            catch (DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
            }

            return apiVersion;
        }
    }
}
