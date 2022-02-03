using ClassroomServiceAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassroomServiceAPI.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ClassroomServiceDbContext _context;

        public QuestionRepository(ClassroomServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool CreateAQuestion(Question question)
        {
            _context.Questions.Add(question);
            _context.SaveChanges();

            return IsQuestionExist(question.QuestionId);
        }

        public bool DeleteAQuestion(Question question)
        {
            _context.Questions.Remove(question);
            _context.SaveChanges();

            return !IsQuestionExist(question.QuestionId);
        }

        public IEnumerable<Question> GetAllQuestions()
        {
            return  _context.Questions.ToList();
        }

        public IEnumerable<Question> GetAllQuestionsForAStudentForAClassroomForATest(int studentId, int classroomId, int testId)
        {
            var questionList = _context.Results
                .Where(r => r.ClassroomId == classroomId)
                .Where(r => r.TestId == testId)
                .Where(r => r.StudentId == studentId)
                .Select(r => r.QuestionId);

            return _context.Questions.Where(question => questionList.Contains(question.QuestionId));
        }

        public IEnumerable<Question> GetAllQuestionsForATeacher(int teacherId)
        {
            return _context.Questions.Where( question => question.TeacherId == teacherId);
        }

        public IEnumerable<Question> GetAllQuestionsForATeacherForASubject(int teacherId, string subject)
        {
            return _context.Questions.Where(question => string.Compare(question.Subject,subject) == 0);
        }

        public IEnumerable<Question> GetAllQuestionsForATestForAClassroom(int classroomId, int testId)
        {
            var questionIdList = _context.Tests
                .Where(t => t.ClassroomId == classroomId)
                .Where(t => t.TestId == testId)
                .Select(t => t.QuestionId).ToList();

            return questionIdList.Join(_context.Questions,
                                    questionId => questionId,
                                    question => question.QuestionId,
                                    (questionId, question) => question );
        }

        public Question GetQuestionWithId(int questionId)
        {
            return _context.Questions.First( q => q.QuestionId == questionId);
        }

        public bool IsQuestionExist(int questionId)
        {
            return _context.Questions.Any(e => e.QuestionId == questionId);
        }

        public bool UpdateAQuestion(Question question)
        {
            _context.Entry(question).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (IsQuestionExist(question.QuestionId))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }
    }
}
