namespace Dovico.OutlookTimeEntryAddin.WebAddInWeb.ViewModels
{
    /// <summary>
    /// Base class to hold request status and error message
    /// </summary>
    public class BaseVM
    {
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }
}