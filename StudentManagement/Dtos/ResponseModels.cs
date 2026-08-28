namespace StudentManagement.Dtos
{
    public class ResultRes
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public List<SubjectMarkRes> Subjects { get; set; } = [];
        public decimal Percentage { get; set; }
        public int Rank { get; set; }
    }

    public class SubjectMarkRes
    {
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public decimal Marks { get; set; }
        public decimal MaximumMarks { get; set; }
        public decimal Percentage { get; set; }
    }

    public class RankingRes
    {
        public int Rank { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public decimal Percentage { get; set; }
    }
    public class MarkRes
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public decimal Marks { get; set; }
        public decimal MaximumMarks { get; set; }
    }
    public class EnrollmentRes
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
    }
    public class SubjectRes
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class StudentRes
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class UserRes
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
