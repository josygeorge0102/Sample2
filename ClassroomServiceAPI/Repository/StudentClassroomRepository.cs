using AutoMapper;
using ClassroomServiceAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassroomServiceAPI.Repository
{
    public class StudentClassroomRepository : IStudentClassroomRepository
    {
        private readonly ClassroomServiceDbContext _context;

        public StudentClassroomRepository(ClassroomServiceDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //because we dont have a student object we send the student ids
        public IEnumerable<int> GetAllStudentsForAClassroomId(int classroomId)
        {
            return _context.StudentClassroomMappers
                .Where(student => student.ClassroomId == classroomId)
                .Select(student => student.StudentId);
        }

        public bool IsStudentExistsInClassroom(int studentId, int classroomId)
        {
            return _context.StudentClassroomMappers
                .Any(r => r.ClassroomId == classroomId && r.StudentId == studentId);
        }

        public IEnumerable<Classroom> GetAllClassroomsForAStudentId(int studentId)
        {
            var classroomList = _context.StudentClassroomMappers
                .Where(student => student.StudentId == studentId)
                .Select(classroom => classroom.ClassroomId);

            return _context.Classrooms.Where(classroom => classroomList.Contains(classroom.ClassroomId));
        }

        public bool JoinAClassroom(int studentId, int classroomId)
        {
            _context.StudentClassroomMappers.Add(new StudentClassroomMapper { StudentId = studentId, ClassroomId = classroomId});
            _context.SaveChanges();

            return IsStudentExistsInClassroom(studentId, classroomId);
        }
    }
}
