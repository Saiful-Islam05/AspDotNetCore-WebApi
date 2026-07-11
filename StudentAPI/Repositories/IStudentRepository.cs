using StudentAPI.Models;

namespace StudentAPI.Repositories
{

    // ✅ Interface — চুক্তিপত্র
    // এখানে শুধু METHOD এর নাম লিখবো
    // কীভাবে কাজ করবে সেটা লিখবো না
    public interface IStudentRepository
    {
        //Bring all students from the database
        Task<List<Student>> GetAllAsync();

        //Bring a student by ID from the database
        Task<Student?> GetByIdAsync(int id);

        //Add a new student to the database
        Task<Student> CreateAsync(Student student);

        //Update an existing student in the database
        Task<Student?> UpdateAsync(int id, Student student);

        //Delete a student from the database
        Task<bool> DeleteAsync(int id);
    }
}
