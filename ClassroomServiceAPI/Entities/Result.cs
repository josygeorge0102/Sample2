using System.ComponentModel.DataAnnotations;

namespace ClassroomServiceAPI.Entities
{
    public class Result
    {
        [Key]
        public int ResultId { get; set; }
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public int StudentId { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string StudentAnswer { get; set; }  // store the ans string and not option No. to allow non-MCQ questions later
        public int StudentScore { get; set; } // storing as int to allow non-MCQ questions later which can have different grading according to answer.
        public DateTimeOffset TestDate { get; set; }
    }
}
