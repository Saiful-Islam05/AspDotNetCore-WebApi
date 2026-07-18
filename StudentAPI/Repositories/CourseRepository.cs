using Microsoft.EntityFrameworkCore;
using StudentAPI.Models;
using StudentAPI.Data;

namespace StudentAPI.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Get All Courses
        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses
                .OrderBy(c => c.Title)  // Order by Title in ascending order
                .ToListAsync();
        }

        // ✅ Get Course by ID
        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        // ✅ Create a new Course
        public async Task<Course?> CreateAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        // ✅ Update an existing Course
        public async Task<Course?> UpdateAsync(int id, Course updatedCoures)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return null;
            
            course.Title = updatedCoures.Title;
            course.Description = updatedCoures.Description;
            course.CreditHours = updatedCoures.CreditHours;

            await _context.SaveChangesAsync();
            return course;
        }

        // ✅ Delete a Course
        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return false;
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }


        // ✅ Search Courses by Title
        public async Task<List<Course>> SearchByTitleAsync(string title)
        {
            return await _context.Courses
                .Where(c => c.Title!.ToLower()
                .Contains(title.ToLower()))  
                .ToListAsync();
        }
    }
}
