using AppointmentScheduling.Data;
using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduling.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;
        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<int> AddUpdate(AppointmentVM model)
        {
            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));

            if(model!=null && model.Id>0)
            {
                //Update Logic
                var appointment = await _db.Appointments.FirstOrDefaultAsync(x => x.Id == model.Id);
                appointment.Title = model.Title;
                appointment.Description = model.Description;
                appointment.StartDate = startDate;
                appointment.EndDate = endDate;
                appointment.Duration = model.Duration;
                appointment.PatientId = model.PatientId;
                appointment.DoctorId = model.DoctorId;
                appointment.IsDoctorApproved = false;
                appointment.AdminId = model.AdminId;

                await _db.SaveChangesAsync();

                return 1;
            }
            else
            {
                //Create Logic
                Appointment appointment = new Appointment
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    PatientId = model.PatientId,
                    DoctorId = model.DoctorId,
                    IsDoctorApproved = false,
                    AdminId = model.AdminId
                };

                await _db.Appointments.AddAsync(appointment);
                await _db.SaveChangesAsync();
                return 2;
            }
        }

        public List<DoctorViewModel> GetDoctorList()
        {
            var doctors = (from user in _db.Users 
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                           join roles in _db.Roles.Where(x=>x.Name == Helper.Helper.Doctor)
                           on userRoles.RoleId equals roles.Id
                           select new DoctorViewModel
                           {
                               Id = user.Id,
                               Name = user.Name
                           }).ToList();
            
            return doctors;
        }

        public List<PatientViewModel> GetPatientList()
        {
            var patients = (from user in _db.Users
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                           join roles in _db.Roles.Where(x => x.Name == Helper.Helper.Patient)
                           on userRoles.RoleId equals roles.Id
                           select new PatientViewModel
                           {
                               Id = user.Id,
                               Name = user.Name
                           }).ToList();

            return patients;
        }

        public  List<AppointmentVM> DoctorsEventById(string doctorId)
        {
            return _db.Appointments.Where(x => x.DoctorId == doctorId).ToList().
                Select(c => new AppointmentVM()
                {
                    Id = c.Id,
                    Description = c.Description,
                    StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Title = c.Title,
                    Duration = c.Duration,
                    IsDoctorApproved = c.IsDoctorApproved
                }).ToList();
        }

        public List<AppointmentVM> PatientsEventById(string patientId)
        {
            return _db.Appointments.Where(x => x.PatientId == patientId).ToList().
                Select(c => new AppointmentVM()
                {
                    Id = c.Id,
                    Description = c.Description,
                    StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Title = c.Title,
                    Duration = c.Duration,
                    IsDoctorApproved = c.IsDoctorApproved
                }).ToList();
        }

        public AppointmentVM GetById(int id)
        {
            return _db.Appointments.Where(x => x.Id == id).ToList().
                Select(c => new AppointmentVM()
                {
                    Id = c.Id,
                    Description = c.Description,
                    StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Title = c.Title,
                    Duration = c.Duration,
                    IsDoctorApproved = c.IsDoctorApproved,
                    PatientId = c.PatientId,
                    DoctorId = c.DoctorId,
                    DoctorName = _db.Users.Where(x=>x.Id==c.DoctorId).Select(x=>x.Name).FirstOrDefault(),
                    PatientName = _db.Users.Where(x=>x.Id==c.PatientId).Select(x=>x.Name).FirstOrDefault(),
                }).SingleOrDefault();
        }

        public async Task<int> Delete(int id)
        {
            var appointment = _db.Appointments.Find(id);
            if (appointment != null)
            {
                _db.Appointments.Remove(appointment);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> ConfirmEvent(int id)
        {
            var appointment = await _db.Appointments.FirstOrDefaultAsync(x=> x.Id == id);
            if(appointment != null)
            {
                appointment.IsDoctorApproved = true;
                return await _db.SaveChangesAsync();
            }
            return 0;
        }
    }
}
