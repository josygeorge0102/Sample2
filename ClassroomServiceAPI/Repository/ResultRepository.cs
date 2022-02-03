using ClassroomServiceAPI;
using ClassroomServiceAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassroomServiceAPI.Repository
{
    public class ResultRepository : IResultRepository
    {
        private readonly ClassroomServiceDbContext _context;

        public ResultRepository(ClassroomServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool CreateAResult(Result result)
        {
            _context.Results.Add(result);
            _context.SaveChanges();

            return IsResultExist(result.ResultId);
        }

        public bool DeleteAResult(Result result)
        {
            _context.Results.Remove(result);
            _context.SaveChanges();

            return !IsResultExist(result.ResultId);
        }

        public IEnumerable<Result> GetAllResults()
        {
            return _context.Results.ToList();
        }

        public IEnumerable<Result> GetAllResultsForAStudent(int studentId)
        {
            return _context.Results.Where(r => r.StudentId == studentId);
        }

        public IEnumerable<Result> GetAllResultsForAStudentForAClassroom(int studentId, int classroomId)
        {
            return _context.Results
                .Where(r => r.ClassroomId == classroomId)
                .Where(r => r.StudentId == studentId);
        }

        public IEnumerable<Models.ResultForAstudentForAClassoomForATest> GetResultsForAllStudentForAClassroomForATest(int classroomId, int testId)
        {
            return (IEnumerable<Models.ResultForAstudentForAClassoomForATest>)_context.Results
                .Where(r => r.ClassroomId == classroomId)
                .Where(r => r.TestId == testId)
                .Where(r => r.TestDate > DateTime.UtcNow)
                .GroupBy(r => r.StudentId)
                .Select(r => new { Id = r.Key, Score = r.Sum(t => t.StudentScore) });
        }

        public int GetAResultForAStudentForAClassroomForATest(int studentId, int classroomId, int testId)
        {
            return _context.Results
                .Where(r => r.ClassroomId == classroomId)
                .Where(r => r.TestId == testId)
                .Where(r => r.StudentId == studentId)
                .Sum(r => r.StudentScore);
        }

        public Result GetResultWithId(int resultId)
        {
            return _context.Results.First(result => result.ResultId == resultId);
        }

        public bool IsResultExist(int resultId)
        {
            return _context.Results.Any(e => e.ResultId == resultId);
        }

        public bool UpdateAResult(Result result)
        {
            _context.Entry(result).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsResultExist(result.ResultId))
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
