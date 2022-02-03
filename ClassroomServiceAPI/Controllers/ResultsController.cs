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
    public class ResultsController : ControllerBase
    {
        private readonly IResultRepository _resultRepository;
        private readonly IStudentClassroomRepository _studentClassroomRepository;
        private readonly IClassroomRepository _classroomRepository;

        public ResultsController(IResultRepository resultRepository,
                                IStudentClassroomRepository studentClassroomRepository,
                                IClassroomRepository classroomRepository)
        {
            _resultRepository = resultRepository ?? throw new ArgumentNullException(nameof(resultRepository));
            _studentClassroomRepository = studentClassroomRepository ?? throw new ArgumentNullException(nameof(studentClassroomRepository));
            _classroomRepository = classroomRepository ?? throw new ArgumentNullException(nameof(classroomRepository));
        }

        // GET: api/Results
        [HttpGet("results/classrooms/{classroomId}/tests/{testId}/results")]
        [Authorize(Policy = "IsTeacher")]
        public ActionResult<IEnumerable<Result>> GetResultsForAllStudentForAClassroomForATestForATeacher(int classroomId, int testId)
        {
            var teacherId = int.Parse(User.FindFirst("sub")?.Value);
            if (!_classroomRepository.DoesTeacherTeachClassroom(teacherId, classroomId))
            {
                BadRequest();
            }

            return  Ok(_resultRepository.GetResultsForAllStudentForAClassroomForATest(classroomId, testId));
        }

        // GET: api/Results/5
        [HttpGet("results/classrooms/{classroomId}/{studentId}/results")]
        [Authorize(Policy = "IsTeacher")]
        public ActionResult<Result> GetAllResultsForAStudentForAClassroomForATeacher(int classroomId, int studentId)
        {
            if (!_studentClassroomRepository.IsStudentExistsInClassroom(studentId, classroomId))
            {
                return NotFound();
            }

            var teacherId = int.Parse(User.FindFirst("sub")?.Value);
            if (!_classroomRepository.DoesTeacherTeachClassroom(teacherId, classroomId))
            {
                return BadRequest();
            }

            return Ok(_resultRepository.GetAllResultsForAStudentForAClassroom(studentId, classroomId));
        }

        // GET: api/classrooms/{classroomId}/students/5
        // get that students past tests for that classroom
        [HttpGet("results/{classroomId}/{studentId}/results")]
        [Authorize(Policy = "IsAdmin")]
        public ActionResult<int> GetAllResultsForAstudentForAClassroomForAdmin(int classroomId, int studentId)
        {
            if (!_studentClassroomRepository.IsStudentExistsInClassroom(studentId, classroomId))
            {
                return NotFound();
            }

            return Ok(_resultRepository.GetAllResultsForAStudentForAClassroom(studentId, classroomId));
        }

        [HttpGet("results/classrooms/{classroomId}/students/{studentId}/results")]
        [Authorize(Policy = "IsStudent")]
        public ActionResult<int> GetAllResultsForAstudentForAClassroom(int classroomId, int studentId)
        {
            if (studentId != int.Parse(User.FindFirst("sub")?.Value))
            {
                return BadRequest();
            }
            if (!_studentClassroomRepository.IsStudentExistsInClassroom(studentId, classroomId))
            {
                return NotFound();
            }

            return Ok(_resultRepository.GetAllResultsForAStudentForAClassroom(studentId, classroomId));
        }

        [HttpGet("results/classrooms/{classroomId}/tests/{testId}/students/{studentId}/results")]
        [Authorize(Policy = "IsTeacher")]
        public ActionResult<Result> GetAResultForAStudentForAClassroomForATestForTeacher(int testId, int classroomId, int studentId)
        {
            if (!_studentClassroomRepository.IsStudentExistsInClassroom(studentId, classroomId))
            {
                return NotFound();
            }

            var teacherId = int.Parse(User.FindFirst("sub")?.Value);
            if (!_classroomRepository.DoesTeacherTeachClassroom(teacherId, classroomId))
            {
                return BadRequest();
            }

            return Ok(_resultRepository.GetAResultForAStudentForAClassroomForATest(studentId, classroomId, testId));
        }

        //// PUT: api/Results/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public IActionResult PutResult(int id, Result result)
        //{
        //    if (id != result.ResultId)
        //    {
        //        return BadRequest();
        //    }

        //    return _resultRepository.UpdateAResult(result) ?
        //              NoContent() : 
        //              StatusCode(500);
        //}

        // POST: api/Results
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("results/classrooms/")]
        [Authorize(Policy = "IsStudent")]
        public ActionResult<Result> PostResult(Result result)
        {
            if (result.StudentId != int.Parse(User.FindFirst("sub")?.Value))
            {
                return BadRequest();
            }
            if (_studentClassroomRepository.IsStudentExistsInClassroom(result.StudentId, result.ClassroomId))
            {
                return NotFound();
            }

        //allow one time posting only
        //make multiple request or make a single request with huge body ?????
        return _resultRepository.CreateAResult(result) ?
                CreatedAtAction("GetResult", new { id = result.ResultId }, result) :
                StatusCode(500);
        }

        ////usecase ???
        //// DELETE: api/Results/5
        //[HttpDelete("{id}")]
        //public IActionResult DeleteResult(int id)
        //{
        //    if (_resultRepository.IsResultExist(id))
        //    {
        //        return NotFound();
        //    }

            

        //    return _resultRepository.DeleteAResult(_resultRepository.GetResultWithId(id)) ?
        //        NoContent() :
        //        StatusCode(500);
        //}

    }
}
