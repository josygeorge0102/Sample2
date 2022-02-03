using ClassroomServiceAPI.Entities;

namespace ClassroomServiceAPI.Repository
{
    public interface IClassroomRepository
    {
        bool IsClassroomExist(int classroomId);

        //get all the classrooms according to the USER
        IEnumerable<Classroom> GetAllClassrooms();
        IEnumerable<Classroom> GetAllClassroomsForATeacherId(int teacherId);

        //get the classroom with id, homepage for teacher and student
        Classroom GetClassroomWithId(int classroomId);// for logging purpose
        bool DoesTeacherTeachClassroom(int teacherId, int classroomId);
        Classroom GetAClassroomWithId(int classroomId);

        bool CreateAClassroom(Classroom classroom);

        bool UpdateAClassroom(Classroom classroom); 
        bool DeleteAClassroom(Classroom classroom);
        //bool UserExistsById(int Id);
    }
}
