using System.Data;
using Microsoft.Data.SqlClient;
using StudentAttendanceManagement.Models;

namespace StudentAttendanceManagement.Repositories
{
    public class StudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AttendanceDB")
                ?? throw new InvalidOperationException("Connection string not found.");
        }
                                 
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            List<Student> students = new List<Student>();

            SqlConnection conn = new SqlConnection(_connectionString);

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Students", conn);
            
            DataTable table = new DataTable();

            adapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                Student student = new Student();

                student.Id = Convert.ToInt32(row["StudentId"]);

                student.Name = row["StudentName"].ToString() ?? string.Empty;
                student.RegistrationNumber = row["RegistrationNumber"].ToString() ?? string.Empty;
                student.Department = row["Department"].ToString() ?? string.Empty;
                student.AttendancePercentage = Convert.ToDecimal(row["AttendancePercentage"]);
                 
                students.Add(student);
            }

            return students;

        }


        public Task<int> AddStudentAsync(Student student)
        {
            return Task.Run(() =>
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlDataAdapter adapter =
                    new SqlDataAdapter("SELECT * FROM Students WHERE 1 = 0", connection);
                using SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                DataTable table = new DataTable();
                adapter.Fill(table);

                DataRow row = table.NewRow();
                row["StudentName"] = student.Name;
                row["RegistrationNumber"] = student.RegistrationNumber;
                row["Department"] = student.Department;
                row["AttendancePercentage"] = student.AttendancePercentage;

                table.Rows.Add(row);
                return adapter.Update(table);
            });
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            List<Student> students = await GetAllStudentsAsync();
            return students.FirstOrDefault(student => student.Id == id);
        }

        public Task<int> UpdateStudentAsync(Student student)
        {
            return Task.Run(() =>
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT * FROM Students WHERE StudentId = @Id", connection);

                adapter.SelectCommand.Parameters.Add("@Id", SqlDbType.Int)
                    .Value = student.Id;

                using SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                DataTable table = new DataTable();
                adapter.Fill(table);

                if (table.Rows.Count == 0)
                    return 0;

                DataRow row = table.Rows[0];
                row["StudentName"] = student.Name;
                row["RegistrationNumber"] = student.RegistrationNumber;
                row["Department"] = student.Department;
                row["AttendancePercentage"] = student.AttendancePercentage;

                return adapter.Update(table);
            });
        }

        public Task<int> DeleteStudentAsync(int id)
        {
            return Task.Run(() =>
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT * FROM Students WHERE StudentId = @Id", connection);

                adapter.SelectCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                using SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                DataTable table = new DataTable();
                adapter.Fill(table);

                if (table.Rows.Count == 0)
                    return 0;

                table.Rows[0].Delete();
                return adapter.Update(table);
            });
        }


    }
}
