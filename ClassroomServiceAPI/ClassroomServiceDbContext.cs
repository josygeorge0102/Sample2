using ClassroomServiceAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassroomServiceAPI
{
    public class ClassroomServiceDbContext : DbContext
    {
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<StudentClassroomMapper> StudentClassroomMappers { get; set; }
       
        public ClassroomServiceDbContext(DbContextOptions<ClassroomServiceDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentClassroomMapper>()
                .HasKey(m => new { m.ClassroomId, m.StudentId });

            modelBuilder.Entity<Test>()
                .HasAlternateKey(t => new { t.TestId, t.QuestionId});

            modelBuilder.Entity<Classroom>()
                .Property(c => c.IsArchived)
                .HasDefaultValue(false);

            modelBuilder.Entity<Result>()
                .HasOne(r => r.Test)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            //Todo: Remove this fake data
            modelBuilder.Entity<Classroom>()
                .HasData(
                new Classroom()
                {
                    ClassroomId = 1,
                    Name = "Suyog Classroom",
                    ImageUrl = "example.com/myImage",
                    TeacherId = 1,
                    Subject = "suyos Fav. Subject",
                },
                new Classroom()
                {
                    ClassroomId = 2,
                    Name = "Shuchi Classroom",
                    ImageUrl = "example.com/myImage",
                    TeacherId = 2,
                    Subject = "shuchi Fav. Subject",
                }
                );
            modelBuilder.Entity<Test>()
                .HasData(
                new Test()
                {
                    Id = 1,
                    TestId = 1,
                    Name = "My new test",
                    QuestionId = 1,
                    ClassroomId = 1,
                    TestDate = DateTime.UtcNow
                },
                new Test()
                {
                    Id = 2,
                    TestId = 1,
                    Name = "My very new test",
                    QuestionId = 2,
                    ClassroomId = 1,
                    TestDate = DateTime.UtcNow
                });
            modelBuilder.Entity<Question>()
                .HasData(
                new Question()
                {
                    QuestionId = 1,
                    Subject = "suyos Fav. Subject",
                    Title = "What is my Fav. Language",
                    CorrectAnswer = "C#",
                    TeacherId = 1,
                    OptionA = "C#",
                    OptionB = "Typescript",
                    OptionC = "Javascript",
                    OptionD = "C"
                },
                new Question()
                {
                    QuestionId = 2,
                    Subject = "shuchi Fav. Subject",
                    Title = "What is my Fav. Language",
                    CorrectAnswer = "javascript",
                    TeacherId = 1,
                    OptionA = "C#",
                    OptionB = "Typescript",
                    OptionC = "Javascript",
                    OptionD = "C"
                });
            modelBuilder.Entity<Result>()
                .HasData(
                new Result()
                {
                    ResultId = 1,
                    ClassroomId = 1,
                    TestId = 1,
                    QuestionId = 1,
                    StudentAnswer = "C#",
                    StudentScore = 5,
                    StudentId = 4,
                    TestDate = DateTime.UtcNow
                },
                new Result()
                {
                    ResultId = 2,
                    ClassroomId = 2,
                    TestId = 1,
                    QuestionId = 2,
                    StudentAnswer = "javascript",
                    StudentScore = 10,
                    StudentId = 5,
                    TestDate = DateTime.UtcNow
                });
            modelBuilder.Entity<StudentClassroomMapper>()
                .HasData(
                new StudentClassroomMapper()
                {
                    StudentId = 1234,
                    ClassroomId = 1
                },
                new StudentClassroomMapper()
                {
                    StudentId = 5678,
                    ClassroomId = 2
                }
                );
        }
    }
}
