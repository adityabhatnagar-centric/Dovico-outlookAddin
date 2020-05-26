using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dovico.OutlookTimeEntryAddin.Business;
using Dovico.OutlookTimeEntryAddin.Application;
using Dovico.OutlookTimeEntryAddin.Integration;

namespace Dovico.OutlookTimeEntryAddin.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //AuthenticateLogic app = new AuthenticateLogic();
            //app.AuthenticateUser("CentricApp", "jm", "jm");

            //TimeEntryLogic app = new TimeEntryLogic();
            //EmployeeBO emp = app.GetEmployeeData("599ef335d1264aed95af1bfc4e513c93.14731");
            //app.GetClients("3065589156ff45c69a2615cce01e1824.14731", Convert.ToInt32(emp.ID));

            // jm jm
            //TimeEntryLogic app = new TimeEntryLogic();
            //EmployeeBO emp = app.GetEmployeeData("nyRYQnOjBOjOBDSkjvG1cX+T8mWe+K+KqM8OE3JqX0QJ+nKAFCFy+e0ZQfuoFgZYOtM8NgPEDmOND16sf8QJ7/gopA43wmvLkFrTIQZvxBs=");
            //app.GetAssignments("nyRYQnOjBOjOBDSkjvG1cX+T8mWe+K+KqM8OE3JqX0QJ+nKAFCFy+e0ZQfuoFgZYOtM8NgPEDmOND16sf8QJ7/gopA43wmvLkFrTIQZvxBs=", Convert.ToInt32(emp.ID));

            // rb rb 
            //TimeEntryLogic app = new TimeEntryLogic();
            //EmployeeBO emp = app.GetEmployeeData("BMavWVqVVNLk1YG4VOVqI0CKl5Y+PvdDSlstnBopwCTqOeas+NWu1TmiUygZdXY458REPkhRMXwhXWxiF3fTq1ma/ASTxgTpTvCNFBIoWac=");
            //app.GetAssignments("BMavWVqVVNLk1YG4VOVqI0CKl5Y+PvdDSlstnBopwCTqOeas+NWu1TmiUygZdXY458REPkhRMXwhXWxiF3fTq1ma/ASTxgTpTvCNFBIoWac=", Convert.ToInt32(emp.ID));

            // jm jm
            //TimeEntryLogic app = new TimeEntryLogic();
            //EmployeeBO emp = app.GetEmployeeData("nyRYQnOjBOjOBDSkjvG1cX+T8mWe+K+KqM8OE3JqX0QJ+nKAFCFy+e0ZQfuoFgZYOtM8NgPEDmOND16sf8QJ7+fbVkmcu1O02WthK5aFS1g=");
            //app.GetClients("nyRYQnOjBOjOBDSkjvG1cX+T8mWe+K+KqM8OE3JqX0QJ+nKAFCFy+e0ZQfuoFgZYOtM8NgPEDmOND16sf8QJ7+fbVkmcu1O02WthK5aFS1g=", Convert.ToInt32(emp.ID));

            // mj  mj 
            //TimeEntryLogic app = new TimeEntryLogic();
            //EmployeeBO emp = app.GetEmployeeData("2j6fl46RVMGxr2PmrcwHIE5EI/4eibX+CwaSg0h+Dj/kI52EKWAb3cv/o9CtJZ3metzRO/t0NfqVxoz6mXdFnnFGdX0yEhlvVHfQZO9cEeM=");
            //app.GetClients("2j6fl46RVMGxr2PmrcwHIE5EI/4eibX+CwaSg0h+Dj/kI52EKWAb3cv/o9CtJZ3metzRO/t0NfqVxoz6mXdFnnFGdX0yEhlvVHfQZO9cEeM=", Convert.ToInt32(emp.ID));
            //app.GetClients("xuwnZFZRhyw8ERt9HQ5NpErGLCCmZymgGA7OzrWfMtjn638rGlq0NFpvSmra4Q/I0g32PHvqwo5QHvG1afcqw4fIohJ904n67+ss1izB7PQ=", Convert.ToInt32(emp.ID));
            //app.GetProjects("xuwnZFZRhyw8ERt9HQ5NpErGLCCmZymgGA7OzrWfMtjn638rGlq0NFpvSmra4Q/I0g32PHvqwo5QHvG1afcqw4fIohJ904n67+ss1izB7PQ=", Convert.ToInt32(emp.ID), new AssignmentBO() { ItemID="-1",Name= "[None]",GetAssignmentsURI=""});
            //app.GetAssignments("xuwnZFZRhyw8ERt9HQ5NpErGLCCmZymgGA7OzrWfMtjn638rGlq0NFpvSmra4Q/I0g32PHvqwo5QHvG1afcqw4fIohJ904n67+ss1izB7PQ=", Convert.ToInt32(emp.ID));

            //TimeEntryLogic app1 = new TimeEntryLogic();
            //EmployeeBO emp1 = app1.GetEmployeeData("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=");
            //app1.GetTimeEntries("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=", Convert.ToInt32(emp1.ID), DateTime.Today.AddDays(-2), DateTime.Today);

            //TimeEntryLogic app = new TimeEntryLogic();
            //EmployeeBO emp = app.GetEmployeeData("xuwnZFZRhyw8ERt9HQ5NpErGLCCmZymgGA7OzrWfMtjn638rGlq0NFpvSmra4Q/I0g32PHvqwo5QHvG1afcqw4fIohJ904n67+ss1izB7PQ="); // mj mj
            //EmployeeBO emp = app.GetEmployeeData("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=");
            //app.GetTimeEntriesDailyTotal("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=", Convert.ToInt32(emp.ID), DateTime.Today.AddDays(-6), DateTime.Today);

            //TimeEntryLogic app = new TimeEntryLogic();
            //TimeEntryService inte = new TimeEntryService();
            //TimeEntrySubmissionBO timeEntryDetails = null;
            //EmployeeBO emp = app.GetEmployeeData("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=");
            //TimeEntryListBO tbo = app.GetSingleTimeEntry("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=", "Tea9bc1c4-3776-43f3-bcb6-6a2dbe129176");

            //timeEntryDetails = new TimeEntrySubmissionBO()
            //    {
            //        TimeEntryId = "",
            //        EmployeeId = Convert.ToInt32(emp.ID),
            //        ClientId = "101",
            //        ProjectId = "112",
            //        TaskId = "107",
            //        StartDate = DateTime.Today,
            //        EndDate = DateTime.Today,
            //        Hours = 4,
            //        Description = "Test From Code",
            //        Billable = false,
            //        CalendarEntry = false,
            //        StartTime = "0600",
            //        StopTime = "0800"

            //        // TODO : Add for Custom Fields
            //    };
            //app.SaveTimeEntry("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=", timeEntryDetails);

            //TimeEntryLogic app = new TimeEntryLogic();
            //app.SubmitWeekTimeEntryForApproval("3065589156ff45c69a2615cce01e1824.14731", 111, DateTime.Today, DateTime.Today.AddDays(6));

            //TimeEntryLogic app2 = new TimeEntryLogic();
            //CustomTemplateListBO A = app.GetCustomTemplates("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=", "A");
            //CustomTemplateListBO N = app.GetCustomTemplates("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=", "N");
            //CustomTemplateListBO D = app.GetCustomTemplates("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=", "D");
            //CustomTemplateListBO M = app2.GetCustomTemplates("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=", "M");
            //CustomTemplateListBO X = app.GetCustomTemplates("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=", "X");

            //TimeEntryLogic app2 = new TimeEntryLogic();
            //CustomTemplateListBO A = app2.GetTaskSpecificCustomTemplates("nyRYQnOjBOjOBDSkjvG1cX+T8mWe+K+KqM8OE3JqX0QJ+nKAFCFy+e0ZQfuoFgZYOtM8NgPEDmOND16sf8QJ70VTGdOHz8zE7lijLZ5ExZU=", "100");
            //CustomTemplateListBO B = app2.GetCustomTemplates("nyRYQnOjBOjOBDSkjvG1cX+T8mWe+K+KqM8OE3JqX0QJ+nKAFCFy+e0ZQfuoFgZYOtM8NgPEDmOND16sf8QJ70VTGdOHz8zE7lijLZ5ExZU=", "M");
            //TimeEntryLogic app3 = new TimeEntryLogic();
            //app3.DeleteTimeEntry("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=", "T2740d578-2b96-42c9-92ec-999eabafd19d");

            //TimeEntryLogic app = new TimeEntryLogic();
            //EmployeeBO emp = app.GetEmployeeData("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=");
            //TimeEntrySubmissionBO timeEntry = new TimeEntrySubmissionBO()
            //{
            //    EmployeeId = Convert.ToInt32(emp.ID),
            //    ClientId = "101",
            //    ProjectId = "112",
            //    TaskId = "107",
            //    StartDate = Convert.ToDateTime(DateTime.Now),
            //    EndDate = Convert.ToDateTime(DateTime.Now),
            //    Hours = Math.Round((Double)2, 2),
            //    Description = "Added Time Entry 1",
            //    Billable = Convert.ToBoolean(false),
            //    CalendarEntry = Convert.ToBoolean(false),
            //    StartTime = string.Empty,
            //    StopTime = string.Empty
            //    ,
            //    CustomFields = new List<CustomFieldSubmissionBO>
            //    {
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "149",
            //            Values = new List<ValueItemsSubmission>
            //            {
            //                new ValueItemsSubmission()
            //                {
            //                    Value = "Alphanumeric Value Added"
            //                }
            //            }
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "150",
            //            Values = new List<ValueItemsSubmission>
            //            {
            //                new ValueItemsSubmission()
            //                {
            //                    Value = Convert.ToString("12/11/2016")
            //                }
            //            }
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "151"
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "152"
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "153",
            //            Values = new List<ValueItemsSubmission>
            //            {
            //                new ValueItemsSubmission()
            //                {
            //                    Value = "Select 1"
            //                },
            //                new ValueItemsSubmission()
            //                {
            //                    Value = "Select 2"
            //                }
            //            }
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "154",
            //            Values = new List<ValueItemsSubmission>
            //            {
            //                new ValueItemsSubmission()
            //                {
            //                    Value = "12"
            //                }
            //            }
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "155"
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "156",
            //            Values = new List<ValueItemsSubmission>
            //            {
            //                new ValueItemsSubmission()
            //                {
            //                    Value = "Select Single 1"
            //                }
            //            }
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "157"
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "158"
            //        }
            //    }
            //};
            //app.SaveTimeEntry("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=", timeEntry);

            //TimeEntryLogic app = new TimeEntryLogic();
            //EmployeeBO emp = app.GetEmployeeData("3065589156ff45c69a2615cce01e1824.14731");
            //TimeEntrySubmissionBO timeEntry = new TimeEntrySubmissionBO()
            //{
            //    TimeEntryId = "Tea9bc1c4-3776-43f3-bcb6-6a2dbe129176",
            //    EmployeeId = Convert.ToInt32(emp.ID),
            //    ClientId = "101",
            //    ProjectId = "112",
            //    TaskId = "107",
            //    StartDate = Convert.ToDateTime(DateTime.Now),
            //    EndDate = Convert.ToDateTime(DateTime.Now),
            //    Hours = Math.Round((Double)8, 2),
            //    Description = "Check Updated - 2",
            //    Billable = Convert.ToBoolean(false),
            //    CalendarEntry = Convert.ToBoolean(false),
            //    StartTime = string.Empty,
            //    StopTime = string.Empty
            //    ,
            //    CustomFields = new List<CustomFieldSubmissionBO>
            //    {
            //        new CustomFieldSubmissionBO()
            //        {
            //            ID = "0bfd2a1e-40fb-4edb-af14-cbcd149d2f0c",
            //            TemplateID = "123"
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            ID = "be4ce0a4-3cb5-474e-8c1a-1d45d9549edd",
            //            TemplateID = "131",
            //            Values = new List<ValueItemsSubmission>
            //            {
            //                new ValueItemsSubmission()
            //                {
            //                    Value = "100"
            //                }
            //            }
            //        }
            //    }
            //};
            //app.SaveTimeEntry("3065589156ff45c69a2615cce01e1824.14731", timeEntry);


            //TimeEntryLogic app = new TimeEntryLogic();
            //EmployeeBO emp = app.GetEmployeeData("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=");
            //TimeEntrySubmissionBO timeEntry = new TimeEntrySubmissionBO()
            //{
            //    EmployeeId = Convert.ToInt32(emp.ID),
            //    ClientId = "101",
            //    ProjectId = "112",
            //    TaskId = "107",
            //    StartDate = Convert.ToDateTime(DateTime.Now),
            //    EndDate = Convert.ToDateTime(DateTime.Now),
            //    Hours = Math.Round((Double)2, 2),
            //    Description = "Added Time Entry 1",
            //    Billable = Convert.ToBoolean(false),
            //    CalendarEntry = Convert.ToBoolean(false),
            //    StartTime = string.Empty,
            //    StopTime = string.Empty
            //    ,
            //    CustomFields = new List<CustomFieldSubmissionBO>
            //    {
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "149",
            //            Values = new List<ValueItemsSubmission>
            //            {
            //                new ValueItemsSubmission()
            //                {
            //                    Value = "Alphanumeric Value Added"
            //                }
            //            }
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "150",
            //            Values = new List<ValueItemsSubmission>
            //            {
            //                new ValueItemsSubmission()
            //                {
            //                    Value = Convert.ToString("12/11/2016")
            //                }
            //            }
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "151"
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "152"
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "153",
            //            Values = new List<ValueItemsSubmission>
            //            {
            //                new ValueItemsSubmission()
            //                {
            //                    Value = "Select 1"
            //                },
            //                new ValueItemsSubmission()
            //                {
            //                    Value = "Select 2"
            //                }
            //            }
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "154",
            //            Values = new List<ValueItemsSubmission>
            //            {
            //                new ValueItemsSubmission()
            //                {
            //                    Value = "12"
            //                }
            //            }
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "155"
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "156",
            //            Values = new List<ValueItemsSubmission>
            //            {
            //                new ValueItemsSubmission()
            //                {
            //                    Value = "Select Single 1"
            //                }
            //            }
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "157"
            //        }
            //        ,
            //        new CustomFieldSubmissionBO()
            //        {
            //            TemplateID = "158"
            //        }
            //    }
            //};
            //app.SaveTimeEntry("EQGWHVXNa/HTPA81o8heh9FCvvuaJoVjxxDOeWn5vUmUylb60V7KB3W0h4pOqHmnhM4nTcD87OUdQ6meeGl9VPVAtuXhhciRLF0VBjR8Pco=", timeEntry);
        }
    }
}
