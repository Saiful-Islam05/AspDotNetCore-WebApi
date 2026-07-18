using StudentAPI.Models;

namespace StudentAPI.Repositories
{
    public interface ICourseRepository
    {
       Task<List<Course>> GetAllAsync(); 
       Task<Course?> GetByIdAsync(int id);
       Task<Course?> CreateAsync(Course course);
       Task<Course?> UpdateAsync(int id, Course course);
       Task<bool> DeleteAsync(int id);

      // Search by Extra Title
         Task<List<Course>> SearchByTitleAsync(string title);
    }
}
