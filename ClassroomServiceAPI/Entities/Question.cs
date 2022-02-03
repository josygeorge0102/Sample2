using System.ComponentModel.DataAnnotations;

namespace ClassroomServiceAPI.Entities
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string CorrectAnswer { get; set; } // store the ans string and not option No. to allow non-MCQ questions later

        //options are nullable here to allow flexibility for 
        //1. adding increase / decrease the No. of options for different question
        //2. for non-MCQ questions later
        public string? OptionA { get; set; } 
        public string? OptionB { get; set; }
        public string? OptionC { get; set; }
        public string? OptionD { get; set; }
        public int TeacherId { get; set; }
    }
}