namespace StudentAPI.Models
{
    // =====================================================
    // ✅ DTO 1 — Response DTO
    // Database → Client এ পাঠানোর জন্য
    // শুধু safe fields আছে
    // =====================================================
    public class StudentDTOs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
    }

    // =====================================================
    // ✅ DTO 2 — Create DTO
    // Client → Server এ পাঠানোর জন্য (POST)
    // Id নেই কারণ server নিজে দেবে
    // =====================================================
    public class CreateStudentDTO
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
    }

    // =====================================================
    // ✅ DTO 3 — Update DTO
    // Client → Server এ পাঠানোর জন্য (PUT)
    // =====================================================
    public class  StudentUpdateDTO
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }

    }
}
