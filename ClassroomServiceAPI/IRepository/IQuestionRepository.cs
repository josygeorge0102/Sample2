using ClassroomServiceAPI.Entities;

namespace ClassroomServiceAPI.Repository
{
    public interface IQuestionRepository
    {
        bool IsQuestionExist(int questionId);

        //get all the Questions according to the USER
        IEnumerable<Question> GetAllQuestions();
        IEnumerable<Question> GetAllQuestionsForATeacher(int teacherId);
        IEnumerable<Question> GetAllQuestionsForATestForAClassroom(int classroomId, int testId);      
        IEnumerable<Question> GetAllQuestionsForATeacherForASubject(int teacherId, string subject);
        IEnumerable<Question> GetAllQuestionsForAStudentForAClassroomForATest(int studentId, int classroomId, int testId);

        Question GetQuestionWithId(int questionId); // for convenience in update method

        bool CreateAQuestion(Question question);

        bool UpdateAQuestion(Question question);
        bool DeleteAQuestion(Question question);

    }
}
