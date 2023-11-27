using AppointmentScheduling.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduling.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [Authorize(Roles = Helper.Helper.Admin)]
        public IActionResult Index()
        {
            ViewBag.DoctorList = _appointmentService.GetDoctorList();
            ViewBag.PatientList = _appointmentService.GetPatientList();
            ViewBag.Duration = Helper.Helper.GetTimeDropDown();
            _appointmentService.GetPatientList();
            return View();
        }
    }
}
