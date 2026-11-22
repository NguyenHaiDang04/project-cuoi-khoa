using System;

namespace NovaCenter.DTO
{
    internal sealed class UserSession
    {
        public int Id { get; }
        public string Username { get; }
        public string FullName { get; }
        public string Role { get; }
        public bool IsAdmin => Role == "Admin";

        public UserSession(int id, string username, string fullName, string role)
        {
            Id = id;
            Username = username;
            FullName = fullName;
            Role = role;
        }
    }

    internal sealed class Student
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool Active { get; set; }
    }

    internal sealed class Course
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }
        public decimal Fee { get; set; }
        public bool Active { get; set; }
    }

    internal sealed class ClassRoom
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int CourseId { get; set; }
        public string Teacher { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
    }
}
