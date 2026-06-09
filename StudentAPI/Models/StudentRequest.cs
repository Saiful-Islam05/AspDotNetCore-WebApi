namespace StudentAPI.Models
{
    // ✅ POST / PUT এ data নেওয়ার জন্য আলাদা Model
    public class StudentRequest
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
    }

    // Search / Filter এর জন্য আলাদা Model
    public class StudentFilter
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? City { get; set; }
    }
}