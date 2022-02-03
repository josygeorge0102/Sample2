using ClassroomServiceAPI.Entities;
using ClassroomServiceAPI.Models;

namespace ClassroomServiceAPI.Repository
{
    public interface ITestRepository
    {
        bool IsTestExist(int testId);

        //get all the Tests according to the USER
        IEnumerable<Test> GetAllTests();
        IEnumerable<TestForAclassroom> GetAllTestsForAClassroom(int classroomId);
        IEnumerable<TestForAclassroom> GetAllUnAttemptedTestsForAClassroomForAStudent(int classroomId,int studentId);
        Test GetTestWithId(int testId); // for convenience in update method
        bool CreateATest(Test test);

        bool UpdateATest(Test test);
        bool DeleteATest(int testId);

        bool CreateAEmptyTest(string testName);

    }
}
