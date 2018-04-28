using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDetector
{
    /// <summary>
    /// Contains basic patient information used to identify the patient.
    /// </summary>
    public class PatientInfo
    {
        public string PatientName { get; set; }
        public Guid PatientFaceId { get; set; }
        public uint RoomNumber { get; set; }
    }

    /// <summary>
    /// This class plays the role of an orchestrating service that manages which patient should be in which room.
    /// Currently fake for demo purposes.
    /// </summary>
    public class RoomOrchestrationService
    {
        private List<PatientInfo> _patientInfos = new List<PatientInfo>();

        public RoomOrchestrationService()
        {
            var fakePatient = new PatientInfo()
            {
                PatientFaceId = new Guid(),
                PatientName = "John Smith",
                RoomNumber = 123
            };

            _patientInfos.Add(fakePatient);
        }

        /// <summary>
        /// Get the patient info for the patient in the specified room number.
        /// </summary>
        /// <returns></returns>
        public PatientInfo GetPatientInfo(uint roomNumber)
        {
            return _patientInfos.Where(p => p.RoomNumber == roomNumber).First();
        }
    }
}
