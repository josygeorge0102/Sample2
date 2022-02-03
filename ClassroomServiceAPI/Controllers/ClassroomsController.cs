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
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace ClassroomServiceAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class ClassroomsController : ControllerBase
    {
        private readonly IClassroomRepository _classroomRepository;
        private readonly IStudentClassroomRepository _studentClassroomRepository;
        private readonly IMapper _mapper;

        public ClassroomsController(IClassroomRepository classroomRepository,
                                    IStudentClassroomRepository studentClassroomRepository,
                                    IMapper mapper)
        {
            _studentClassroomRepository = studentClassroomRepository ?? throw new ArgumentNullException(nameof(studentClassroomRepository));
            _classroomRepository = classroomRepository ?? throw new ArgumentNullException(nameof(classroomRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
        }

        // GET: api/Classrooms
        [HttpGet("classrooms")]
        // [Authorize(Policy = "IsAdmin")]
        public ActionResult<IEnumerable<Classroom>> GetAllClassroomsForAdmin()
        {
            return Ok(_classroomRepository.GetAllClassrooms());
        }
        // GET: api/Classrooms
        [HttpGet("teachers/{teacherId}/classrooms")]
        [Authorize(Policy = "IsAdmin")]
        public ActionResult<IEnumerable<Classroom>> GetAllClassroomsForTeacherForAdmin(int teacherId)
        {
            //if (!UserExistsById(teacherId))
            //{
            //    return NotFound();
            //}

            return Ok(_classroomRepository.GetAllClassroomsForATeacherId(teacherId));
        }

        // GET: api/Classrooms
        [HttpGet("classrooms/teachers/{teacherId}/classrooms")]
        [Authorize(Policy = "IsTeacherOrAdmin")]
        public ActionResult<IEnumerable<Classroom>> GetAllClassroomsForTeacher(int teacherId)
        {
            if (teacherId != int.Parse(User.FindFirst("sub")?.Value))
            {
                return BadRequest();
            }

            //if (!UserExistsById(teacherId))
            //{
            //    return NotFound();
            //}
            
            return Ok(_classroomRepository.GetAllClassroomsForATeacherId(teacherId));
        }

        //private bool UserExistsById(int teacherId)
        //{
        //    return _classroomRepository.UserExistsById(teacherId);
        //}

        // GET: api/Classrooms/5
        [HttpGet("classrooms/{classroomId}")]
        [Authorize(Policy = "IsAdmin")]
        public ActionResult<Classroom> GetClassroomByIdForAdmin(int classroomId)
        {

            if (!_classroomRepository.IsClassroomExist(classroomId))
            {
                return NotFound();
            }

            return Ok(_classroomRepository.GetAClassroomWithId(classroomId));
        }

        [HttpGet("classrooms/{classroomId}/students/{studentId}/classroom")]
        [Authorize(Policy = "IsStudentOrAdmin")]
        public ActionResult<Classroom> GetClassroomByIdForStudent(int classroomId, int studentId)
        {
            if (studentId != int.Parse(User.FindFirst("sub")?.Value))
            {
                return BadRequest();
            }

            if (!_classroomRepository.IsClassroomExist(classroomId))
            {
                return NotFound();
            }
            if (!_studentClassroomRepository.IsStudentExistsInClassroom(studentId, classroomId))
            {
                return NotFound();
            }
            return Ok(_classroomRepository.GetAClassroomWithId(classroomId));
        }

        ///classrooms/teachers/{teacherId}/classrooms
        [HttpGet("classrooms/{classroomId}/teachers/{teacherId}/classroom")]
        [Authorize(Policy = "IsTeacherOrAdmin")]
        public ActionResult<Classroom> GetClassroomByIdForTeacher(int classroomId, int teacherId)
        {
            if (teacherId != int.Parse(User.FindFirst("sub")?.Value))
            {
                return BadRequest();
            }

            if (!_classroomRepository.IsClassroomExist(classroomId))
            {
                return NotFound();
            }
            if(!_classroomRepository.DoesTeacherTeachClassroom(teacherId, classroomId))
            {
                return BadRequest();
            }
            return Ok(_classroomRepository.GetAClassroomWithId(classroomId));
        }

        // PUT: api/Classrooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("classrooms/{classroomId}/teachers/{teacherId}/classroom")] //change later to use PATCH
        [Authorize(Policy = "IsTeacher")]
        public IActionResult UpdateClassroomForTeacher(int classroomId,int teacherId, Classroom classroom)
        {
            if(teacherId != int.Parse(User.FindFirst("sub")?.Value))
            {
                return BadRequest();
            }

            if(classroomId != classroom.ClassroomId)
            {
                return BadRequest();
            }

            return _classroomRepository.UpdateAClassroom(classroom) ? 
                NoContent() : 
                StatusCode(500);
        }

        // POST: api/Classrooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("classrooms")]
        [Authorize(Policy = "IsTeacher")]
        public ActionResult<Classroom> CreateAClassroom(Classroom classroom)
        {
            //if (!UserExistsById(classroom.TeacherId))
            //{
            //    return BadRequest();
            //}
            if (classroom.TeacherId != int.Parse(User.FindFirst("sub")?.Value))
            {
                return BadRequest();
            }

            return _classroomRepository.CreateAClassroom(classroom) ?
                CreatedAtAction("Create a Classroom", new { id = classroom.ClassroomId }, classroom) :
                    StatusCode(500);

        }

        [HttpDelete("classrooms/{classroomId}")]
        [Authorize(Policy = "IsAdmin")]
        public IActionResult DeleteClassroomForAdmin(int classroomId)
        {
            if (_classroomRepository.IsClassroomExist(classroomId))
            {
                return NotFound();
            }

            return _classroomRepository.DeleteAClassroom(_classroomRepository.GetAClassroomWithId(classroomId)) ?
                NoContent() :
                StatusCode(500);
        }

        // DELETE: api/Classrooms/5
        [HttpDelete("classrooms/{classroomId}/teachers/{teacherId}")]
        [Authorize(Policy = "IsTeacher")]
        public IActionResult DeleteClassroomForATeacher(int classroomId, int teacherId)
        {
            if (teacherId != int.Parse(User.FindFirst("sub")?.Value))
            {
                return BadRequest();
            }

            if (!_classroomRepository.IsClassroomExist(classroomId))
            {
                return NotFound();
            }
            if (!_classroomRepository.DoesTeacherTeachClassroom(teacherId, classroomId))
            {
                return BadRequest();
            }

            return _classroomRepository.DeleteAClassroom(_classroomRepository.GetAClassroomWithId(classroomId)) ? 
                NoContent() : 
                StatusCode(500);
        }

    }
}
