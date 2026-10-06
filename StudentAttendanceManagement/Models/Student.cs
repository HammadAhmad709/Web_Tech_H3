namespace StudentAttendanceManagement.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public string RegistrationNumber { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public decimal AttendancePercentage  { get; set; } = 0;

    }
}
