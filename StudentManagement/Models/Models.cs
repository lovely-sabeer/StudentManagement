namespace StudentManagement.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
    public class Student : BaseEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public virtual ICollection<Enrollment> Enrollments { get; set; } = [];
        public ICollection<Mark> Marks { get; set; } = [];
    }
    public class Subject : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<Enrollment> Enrollments { get; set; } = [];
        public ICollection<Mark> Marks { get; set; } = [];
    }
    public class Enrollment : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }
        public Guid SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
    public class Mark : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }
        public Guid SubjectId { get; set; }
        public Subject? Subject { get; set; }
        public decimal Marks { get; set; }
        public decimal MaximumMarks { get; set; }
    }
}
