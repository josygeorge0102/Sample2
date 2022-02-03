using System.ComponentModel.DataAnnotations;

namespace ClassroomServiceAPI.Entities
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Name { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
        public DateTimeOffset? TestDate { get; set; } // can be used for scheduling a test
    }
}

