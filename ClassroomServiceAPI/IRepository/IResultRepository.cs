using ClassroomServiceAPI.Entities;

namespace ClassroomServiceAPI.Repository
{
    public interface IResultRepository
    {
        bool IsResultExist(int resultId);

        //get all the Results according to the USER
        IEnumerable<Result> GetAllResults();
        IEnumerable<Result> GetAllResultsForAStudent(int studentId);
        IEnumerable<Result> GetAllResultsForAStudentForAClassroom(int studentId, int classroomId);
        int GetAResultForAStudentForAClassroomForATest(int studentId, int classroomId, int testId);
        IEnumerable<Models.ResultForAstudentForAClassoomForATest> GetResultsForAllStudentForAClassroomForATest(int classroomId, int testId);

        //get the Result with id, homepage for teacher and student
        Result GetResultWithId(int resultId);
        bool CreateAResult(Result Result);
        bool UpdateAResult(Result Result);
        bool DeleteAResult(Result Result);
    }
}
