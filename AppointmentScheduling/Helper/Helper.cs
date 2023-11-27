using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppointmentScheduling.Helper
{
    public class Helper
    {
        public const string Admin = "Admin";
        public const string Patient = "Patient";
        public const string Doctor = "Doctor";

        public static string appointmentAdded = "Appointment added successfully.";
        public static string meetingConfirmed = "Meeting Confirmed successfully.";
        public static string meetingConfirmError = "Meeting Confirmed Error!.";
        public static string appointmentUpdated = "Appointment updated successfully.";
        public static string appointmentDeleted = "Appointment deleted successfully.";
        public static string appointmentExists = "Appointment for selected date and time already exists.";
        public static string appointmentNotExists = "Appointment does not exists.";

        public static string appointmentAddError = "Something went wrong, Please try again.";
        public static string appointmentUpdateError = "Something went wrong, Please try again.";
        public static string somethingWentWrong = "Something went wrong, Please try again.";

        public static int Success_Code = 1;
        public static int Failure_Code = 0;
        public static List<SelectListItem> GetRolesForDropDown(bool isAdmin)
        {

            if(isAdmin)
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Value= Admin,Text=Admin}
                  
                };
            }
            else
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Value= Doctor,Text=Doctor},
                    new SelectListItem{Value= Patient,Text=Patient}
                };
            }   
        }

        public static List<SelectListItem> GetTimeDropDown()
        {
            int minute = 60;
            List<SelectListItem> duration = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + " Hr" });
                minute = minute + 30;
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + " Hr 30 min" });
                minute = minute + 30;
            }
            return duration;
        }
    }
}
