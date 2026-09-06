using StudentAPI.Models;
using StudentAPI.Repositories;

namespace StudentAPI.Services
{
    public class StudentService : IStudentService
    {
        // ✅ Repository inject করছি — DbContext নয়
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        // =====================================================
        // ✅ GET ALL
        // =====================================================
        public async Task<List<StudentResponseDTO>> GetAllAsync()
        {
            var students = await _repository.GetAllAsync();

            // Student → StudentResponseDTO convert
            return students.Select(s => new StudentResponseDTO
            {
                Id = s.Id,
                Name = s.Name,
                Age = s.Age,
                City = s.City,
                Email = s.Email,
                Phone = s.Phone,
                CreatedAt = s.CreatedAt
            }).ToList();
        }

        // =====================================================
        // ✅ GET BY ID
        // =====================================================
        public async Task<StudentResponseDTO?> GetByIdAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id);

            if (student == null) return null;

            return new StudentResponseDTO
            {
                Id = student.Id,
                Name = student.Name,
                Age = student.Age,
                City = student.City,
                Email = student.Email,
                Phone = student.Phone,
                CreatedAt = student.CreatedAt
            };
        }

        // =====================================================
        // ✅ CREATE
        // =====================================================
        public async Task<StudentResponseDTO> CreateAsync(
            StudentCreateDTO dto)
        {
            // DTO → Student convert
            var student = new Student
            {
                Name = dto.Name,
                Age = dto.Age,
                City = dto.City,
                Email = dto.Email,
                Phone = dto.Phone,
                CreatedAt = DateTime.Now,
                Password = "defaultPass",
                BankAccount = "BD000"
            };

            var created = await _repository.CreateAsync(student);

            return new StudentResponseDTO
            {
                Id = created.Id,
                Name = created.Name,
                Age = created.Age,
                City = created.City,
                Email = created.Email,
                Phone = created.Phone,
                CreatedAt = created.CreatedAt
            };
        }

        // =====================================================
        // ✅ UPDATE
        // =====================================================
        public async Task<StudentResponseDTO?> UpdateAsync(
            int id, StudentUpdateDTO dto)
        {
            var student = new Student
            {
                Name = dto.Name,
                Age = dto.Age,
                City = dto.City,
                Email = dto.Email,
                Phone = dto.Phone
            };

            var updated = await _repository.UpdateAsync(id, student);

            if (updated == null) return null;

            return new StudentResponseDTO
            {
                Id = updated.Id,
                Name = updated.Name,
                Age = updated.Age,
                City = updated.City,
                Email = updated.Email,
                Phone = updated.Phone,
                CreatedAt = updated.CreatedAt
            };
        }

        // =====================================================
        // ✅ DELETE
        // =====================================================
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}