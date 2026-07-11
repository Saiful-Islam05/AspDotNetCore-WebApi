using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;
using StudentAPI.Models;

namespace StudentAPI.Repositories
{
    // ✅ IStudentRepository implement করছি
    // Interface এর সব method এখানে লিখতে হবে
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;
        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }



        // =====================================================
        // ✅ GET ALL — সব Student আনো
        // =====================================================
        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }



        // =====================================================
        // ✅ GET BY ID — একটি Student আনো
        // =====================================================    
        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }



        // =====================================================
        // ✅ CREATE — একটি নতুন Student তৈরি করো
        // =====================================================
        public async Task<Student> CreateAsync(Student student)
        {
            // Database এ Add করো
            _context.Students.Add(student);
            // Save করো
            await _context.SaveChangesAsync();
            // নতুন student return করো (Id সহ)
            return student;
        }


        // =====================================================
        // ✅ UPDATE — Student Update করো
        // =====================================================
        public async Task<Student?> UpdateAsync(int id, Student updatedStudent)
        {
            // আগে খোঁজো
            var student = await _context.Students.FindAsync(id);

            // না পেলে null return করো
            if (student == null) return null;

            // Update করো
            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.City = updatedStudent.City;
            student.Email = updatedStudent.Email;
            student.Phone = updatedStudent.Phone;

            // Save করো
            await _context.SaveChangesAsync();

            return student;
        }

        // =====================================================
        // ✅ DELETE — Student মুছে ফেলো
        // =====================================================
        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
