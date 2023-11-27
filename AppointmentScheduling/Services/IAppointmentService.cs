using AppointmentScheduling.Models.ViewModels;

namespace AppointmentScheduling.Services
{
    public interface IAppointmentService
    {
        public List<DoctorViewModel> GetDoctorList();
        public List<PatientViewModel> GetPatientList();
        public Task<int> AddUpdate(AppointmentVM model);
        public List<AppointmentVM> DoctorsEventById(string doctorId);
        public List<AppointmentVM> PatientsEventById(string patientId);

        public AppointmentVM GetById(int id);

        public Task<int> Delete(int id);
        public Task<int> ConfirmEvent(int id);
    }
}
