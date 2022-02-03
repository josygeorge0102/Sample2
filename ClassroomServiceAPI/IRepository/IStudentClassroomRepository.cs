using ClassroomServiceAPI.Entities;

namespace ClassroomServiceAPI.Repository
{
    public interface IStudentClassroomRepository
    {
        IEnumerable<int> GetAllStudentsForAClassroomId(int classroomId);
        bool IsStudentExistsInClassroom(int studentId, int classroomId);
        IEnumerable<Classroom> GetAllClassroomsForAStudentId(int studentId);
        bool JoinAClassroom(int studentId, int classroomId);
    }
}
