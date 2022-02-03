using ClassroomServiceAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassroomServiceAPI.Repository
{
    public class ClassroomRepository : IClassroomRepository
    {
        private readonly ClassroomServiceDbContext _context;

        public ClassroomRepository(ClassroomServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool CreateAClassroom(Classroom classroom)
        {
            _context.Classrooms.Add(classroom);
            _context.SaveChanges();

            return IsClassroomExist(classroom.ClassroomId);
        }

        public bool DeleteAClassroom(Classroom classroom)
        {
            _context.Classrooms.Remove(classroom);
            _context.SaveChanges();

            return !IsClassroomExist(classroom.ClassroomId);
        }

        public Classroom GetAClassroomWithId(int classroomId)
        {
            return _context.Classrooms.First(classroom => classroom.ClassroomId == classroomId);
        }

        public IEnumerable<Classroom> GetAllClassrooms()
        {
            return _context.Classrooms.ToList();
        }

        

        public IEnumerable<Classroom> GetAllClassroomsForATeacherId(int teacherId)
        {
            return _context.Classrooms.Where(classroom => classroom.TeacherId == teacherId);
        }

        public Classroom GetClassroomWithId(int classroomId)
        {
            return _context.Classrooms.First(classroom => classroom.ClassroomId == classroomId);    
        }

        public bool DoesTeacherTeachClassroom(int teacherId, int classroomId)
        {
            return _context.Classrooms
                .Any(classroom => classroom.ClassroomId == classroomId && classroom.TeacherId == teacherId);
        }

        public bool IsClassroomExist(int classroomId)
        {
            return _context.Classrooms.Any(e => e.ClassroomId == classroomId);
        }

        public bool UpdateAClassroom(Classroom classroom)
        {
            //https://stackoverflow.com/questions/30987806/dbset-attachentity-vs-dbcontext-entryentity-state-entitystate-modified
            _context.Entry(classroom).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsClassroomExist(classroom.ClassroomId))
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

        ////TODO: IMPLEMENT CALLING THE OTHER SERVICE
        //public bool UserExistsById(int Id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}