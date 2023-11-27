using AppointmentScheduling.Models.ViewModels;
using AppointmentScheduling.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppointmentScheduling.Controllers.Api
{
    [Route("Api/Appointment")]
    [ApiController]
    public class AppointmentApiController : Controller
    {
        private IAppointmentService _appointmentService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly string loginUserId;
        private readonly string role;
        public AppointmentApiController(IAppointmentService appointmentService, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        
        [Route("SaveCalendarData")]
        [HttpPost]
        public async Task<IActionResult> SaveCalendarData(AppointmentVM data)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();

            try
            {
                commonResponse.Status =  _appointmentService.AddUpdate(data).Result;

                if (commonResponse.Status == 1)
                {
                    commonResponse.Message = Helper.Helper.appointmentUpdated;
                }
                if (commonResponse.Status == 2)
                {
                    commonResponse.Message = Helper.Helper.appointmentAdded;
                }
            }
            catch (Exception ex)
            {
                commonResponse.Message = ex.Message;
                commonResponse.Status = Helper.Helper.Failure_Code;
            }

            return Ok(commonResponse);
        }

        
        [HttpGet]
        [Route("GetCalendarData")]
        public IActionResult GetCalendarData(string doctorId)
        {
            CommonResponse<List<AppointmentVM>> commonResponse = new CommonResponse<List<AppointmentVM>>();
            try
            {
                if (role == Helper.Helper.Patient)
                {
                    commonResponse.DataEnum = _appointmentService.PatientsEventById(loginUserId);
                    commonResponse.Status = Helper.Helper.Success_Code;
                }
                else if (role == Helper.Helper.Doctor)
                {
                    commonResponse.DataEnum = _appointmentService.DoctorsEventById(loginUserId);
                    commonResponse.Status = Helper.Helper.Success_Code;
                }
                else
                {
                    List<AppointmentVM> li =  _appointmentService.DoctorsEventById(doctorId);
                    commonResponse.DataEnum = li;
                    commonResponse.Status = Helper.Helper.Success_Code;
                }

            }
            catch(Exception ex)
            {
                commonResponse.Message = ex.Message;
                commonResponse  .Status = Helper.Helper.Failure_Code;
            }
            return Ok(commonResponse);
        }
        
        [HttpGet]
        [Route("GetCalendarDataById/{id}")]
        public IActionResult GetCalendarDataById(int id)
        {
            CommonResponse<AppointmentVM> commonResponse = new CommonResponse<AppointmentVM>();
            try
            { 
                commonResponse.DataEnum =_appointmentService.GetById(id);
                commonResponse.Status = Helper.Helper.Success_Code;

            }
            catch(Exception ex)
            {
                commonResponse.Message = ex.Message;
                commonResponse  .Status = Helper.Helper.Failure_Code;
            }
            return Ok(commonResponse);
        }


        
        [HttpGet]
        [Route("DeleteAppointment/{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();

            try
            {
                commonResponse.Status = await _appointmentService.Delete(id);

                commonResponse.Message = commonResponse.Status == 1 ? Helper.Helper.appointmentDeleted : Helper.Helper.somethingWentWrong;
               
            }
            catch (Exception ex)
            {
                commonResponse.Message = ex.Message;
                commonResponse.Status = Helper.Helper.Failure_Code;
            }

            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("ConfirmEvent/{id}")]
        public async Task<IActionResult> ConfirmEvent(int id)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();

            try
            {
                var result = await _appointmentService.ConfirmEvent(id);
                if(result>0)
                {
                    commonResponse.Status = Helper.Helper.Success_Code;
                    commonResponse.Message = Helper.Helper.meetingConfirmed;
                }
                else
                {
                    commonResponse.Status = Helper.Helper.Failure_Code;
                    commonResponse.Message = Helper.Helper.meetingConfirmError;
                }

            }
            catch (Exception ex)
            {
                commonResponse.Message = ex.Message;
                commonResponse.Status = Helper.Helper.Failure_Code;
            }

            return Ok(commonResponse);
        }

    }
}
