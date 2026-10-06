using Microsoft.AspNetCore.Mvc;
using StudentAttendanceManagement.Models;
using StudentAttendanceManagement.Repositories;

namespace StudentAttendanceManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentRepository _repository;

        public StudentController(StudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            List<Student> students =
                await _repository.GetAllStudentsAsync();

            return View(students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid)
                return View(student);

            await _repository.AddStudentAsync(student);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Student? student =
                await _repository.GetStudentByIdAsync(id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(student);

            int rowsAffected =
                await _repository.UpdateStudentAsync(student);

            if (rowsAffected == 0)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            int rowsAffected =
                await _repository.DeleteStudentAsync(id);

            if (rowsAffected == 0)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}