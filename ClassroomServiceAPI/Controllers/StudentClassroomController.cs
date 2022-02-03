#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomServiceAPI;
using ClassroomServiceAPI.Entities;
using ClassroomServiceAPI.Repository;
using Microsoft.AspNetCore.Authorization;

namespace ClassroomServiceAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class StudentClassroomController : ControllerBase
    {
        private readonly IStudentClassroomRepository _studentClassroomRepository;
        private readonly IResultRepository _resultRepository;
        private readonly IClassroomRepository _classroomRepository;

        public StudentClassroomController(IStudentClassroomRepository studentClassroomRepository,
                                          IResultRepository resultRepository,
                                          IClassroomRepository classroomRepository)
        {
            _studentClassroomRepository = studentClassroomRepository ?? throw new ArgumentNullException(nameof(studentClassroomRepository));
            _resultRepository = resultRepository ?? throw new ArgumentNullException(nameof(resultRepository));
            _classroomRepository = classroomRepository ?? throw new ArgumentNullException(nameof(classroomRepository));
        }

       
        [HttpGet("classrooms/{classroomId}/students")]
        [Authorize(Policy = "IsAdmin")]
        public ActionResult<IEnumerable<StudentClassroomMapper>> GetAllStudentForAClassroom(int classroomId)
        {
            return Ok(_studentClassroomRepository.GetAllStudentsForAClassroomId(classroomId));
        }

        [HttpGet("classrooms/{classroomId}/teacher/{teacherId}/students")]
        [Authorize(Policy = "IsTeacherOrAdmin")]
        public ActionResult<IEnumerable<StudentClassroomMapper>> GetAllStudentForAClassroomForTeacher(int classroomId, int teacherId)
        {
            if (teacherId != int.Parse(User.FindFirst("sub")?.Value))
            {
                return BadRequest();
            }
            //if (!UserExistsById(teacherId))
            //{
            //    return NotFound();
            //}
            return Ok(_studentClassroomRepository.GetAllStudentsForAClassroomId(classroomId));
        }


        [HttpGet("classrooms/students/{studentId}/classrooms")]
        [Authorize(Policy = "IsStudent")]
        public ActionResult<IEnumerable<Classroom>> GetAllClassroomsForAStudent(int studentId)
        {
            if (studentId != int.Parse(User.FindFirst("sub")?.Value))
            {
                return BadRequest();
            }

            //if (!UserExistsById(studentId))
            //{
            //    return NotFound();
            //}

            return Ok(_studentClassroomRepository.GetAllClassroomsForAStudentId(studentId));
        }

        //private bool UserExistsById(int teacherId)
        //{
        //    return _classroomRepository.UserExistsById(teacherId);
        //}

        // GET: api/classrooms/{classroomId}/students/5
        [HttpGet("students/{studentId}/classrooms")]
        [Authorize(Policy = "IsAdmin")]
        public ActionResult<IEnumerable<Classroom>> GetAllClassroomsForAStudentForAdmin(int studentId)
        {
            return Ok(_studentClassroomRepository.GetAllClassroomsForAStudentId(studentId));
        }

        // POST: api/Classrooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("students/{studentId}/classrooms/{classroomId}/classroom")]
        [Authorize(Policy = "IsStudent")]
        public ActionResult<StudentClassroomMapper> JoinAClassroom(int studentId, int classroomId)
        {
            if (studentId != int.Parse(User.FindFirst("sub")?.Value))
            {

            }
            //if (!UserExistsById(studentId))
            //{
            //    return NotFound();
            //}
            return _studentClassroomRepository.JoinAClassroom(studentId, classroomId) ?
                CreatedAtAction("JoinAclassroom", new StudentClassroomMapper { ClassroomId = classroomId, StudentId = studentId }) :
                    StatusCode(500);

        }

        //// PUT: api/classrooms/{classroomId}/students/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public IActionResult PutStudentClassroomMapper(int id, StudentClassroomMapper studentClassroomMapper)
        //{
        //    if (id != studentClassroomMapper.ClassroomId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(studentClassroomMapper).State = EntityState.Modified;

        //    try
        //    {
        //         _context.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!StudentClassroomMapperExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/classrooms/{classroomId}/students
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public ActionResult<StudentClassroomMapper> PostStudentClassroomMapper(StudentClassroomMapper studentClassroomMapper)
        //{
        //    _context.StudentClassroomMappers.Add(studentClassroomMapper);
        //    try
        //    {
        //         _context.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (StudentClassroomMapperExists(studentClassroomMapper.ClassroomId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetStudentClassroomMapper", new { id = studentClassroomMapper.ClassroomId }, studentClassroomMapper);
        //}

        //// DELETE: api/classrooms/{classroomId}/students/5
        //[HttpDelete("{id}")]
        //public IActionResult DeleteStudentClassroomMapper(int id)
        //{
        //    var studentClassroomMapper =  _context.StudentClassroomMappers.Find(id);
        //    if (studentClassroomMapper == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.StudentClassroomMappers.Remove(studentClassroomMapper);
        //     _context.SaveChanges();

        //    return NoContent();
        //}

    }
}
 