using ClassroomServiceAPI.Entities;
using ClassroomServiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassroomServiceAPI.Repository
{
    public class TestRepository : ITestRepository
    {
        private readonly ClassroomServiceDbContext _context;

        public TestRepository(ClassroomServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool CreateATest(Test test)
        {
            _context.Tests.Add(test);
            _context.SaveChanges();

            return IsTestExist(test.TestId);
        }

        public bool DeleteATest(int testId)
        {
            //loop over all the test and question pair
            foreach (var test in _context.Tests.Where(t =>t.TestId == testId))
            {
                _context.Tests.Remove(test);
                _context.SaveChanges();
            }

            return !IsTestExist(testId);
        }

        public IEnumerable<Test> GetAllTests()
        {
            return _context.Tests.ToList();
        }

        //return test id and test name for each of the test
        public IEnumerable<TestForAclassroom> GetAllTestsForAClassroom(int classroomId)
        {
            return _context.Tests
                .Where(t => t.ClassroomId == classroomId)
                .GroupBy(t => t.TestId)
                .Select(t => new TestForAclassroom { TestId = t.Key, TestName = t.First().Name, TestDate = (DateTimeOffset)t.First().TestDate });
        }

        public Test GetTestWithId(int testId)
        {
            return _context.Tests.First(test => test.TestId == testId);
        }

        public bool IsTestExist(int testId)
        {
            return _context.Tests.Any(e => e.TestId == testId);
        }

        public bool CreateAEmptyTest(string testName)
        {

            _context.Tests.Add(new Test { Name = testName});
            _context.SaveChanges();

            return _context.Tests.Any(t => t.Name == testName);
        }
        public bool UpdateATest(Test test)
        {
            _context.Entry(test).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsTestExist(test.TestId))
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

        public IEnumerable<TestForAclassroom> GetAllUnAttemptedTestsForAClassroomForAStudent(int classroomId, int studentId)
        {
            return _context.Tests
                .Where(t => t.ClassroomId == classroomId)
                .Where(t => t.TestDate > DateTimeOffset.UtcNow )
                .GroupBy(t => t.TestId)
                .Select(t => new TestForAclassroom { TestId = t.Key, TestName = t.First().Name, TestDate = (DateTimeOffset)t.First().TestDate });
        }
    }
}
