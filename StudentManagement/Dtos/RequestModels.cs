namespace StudentManagement.Dtos
{
    public class MarkCreateReq
    {
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public decimal Marks { get; set; }
        public decimal MaximumMarks { get; set; }
    }
    public class MarkUpdateReq
    {
        public decimal Marks { get; set; }
        public decimal MaximumMarks { get; set; }
    }
    public class EnrollmentCreateReq
    {
        public Guid StudentId { get; set; }
        public List<Guid> SubjectIds { get; set; } = [];
    }
    public class SubjectCreateReq
    {
        public string Name { get; set; } = string.Empty;
    }

    public class SubjectUpdateReq
    {
        public string Name { get; set; } = string.Empty;
    }
    public class StudentCreateReq
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
    public class StudentUpdateReq
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
    public class RegisterReq
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
    public class LoginReq
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
