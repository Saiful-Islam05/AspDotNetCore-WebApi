using StudentAPI.Models;

namespace StudentAPI.Services
{
    public interface IStudentService
    {
        Task<List<StudentResponseDTO>> GetAllAsync();
        Task<StudentResponseDTO?> GetByIdAsync(int id);
        Task<StudentResponseDTO> CreateAsync(StudentCreateDTO dto);
        Task<StudentResponseDTO?> UpdateAsync(int id, StudentUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}