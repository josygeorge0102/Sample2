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
using ClassroomServiceAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace ClassroomServiceAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly ITestRepository _testRepository;
        public readonly IQuestionRepository _questionRepository;
        private readonly IClassroomRepository _classroomRepository;
        private readonly IStudentClassroomRepository _studentClassroomRepository;

        public TestsController(ITestRepository testRepository,
                                IQuestionRepository questionRepository,
                                IStudentClassroomRepository studentClassroomRepository,
                                IClassroomRepository classroomRepository)
        {
            _studentClassroomRepository = studentClassroomRepository ?? throw new ArgumentNullException(nameof(studentClassroomRepository));
            _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
            _classroomRepository = classroomRepository ?? throw new ArgumentNullException(nameof(classroomRepository)); 
        }

        // GET: api/classrooms/{classroomId}/tests
        [HttpGet("tests/teachers/{teacherId}/classrooms/{classroomId}/tests")]
        [Authorize(Policy = "IsTeacher")]
        public ActionResult<IEnumerable<TestForAclassroom>> GetAllTestsForAClassroomForATeacher(int classroomId, int teacherId)
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
            return Ok(_testRepository.GetAllTestsForAClassroom(classroomId));
        }

        [HttpGet("tests/students/{studentId}/classrooms/{classroomId}/tests")]
        [Authorize(Policy = "IsStudent")]
        //wont show unscheduled test
        public ActionResult<IEnumerable<TestForAclassroom>> GetAllTestsForAClassroomForAStudent(int classroomId, int studentId)
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
            return Ok(_testRepository.GetAllTestsForAClassroom(classroomId));
        }

        //// GET: api/classrooms/{classroomId}/tests
        //[HttpGet]
        //public ActionResult<IEnumerable<TestForAclassroom>> GetAllTestsForClassroomForAStudent(int classroomId, [FromBody]int studentId)
        //{
        //    return Ok(_testRepository.GetAllUnAttemptedTestsForAClassroomForAStudent(classroomId, studentId));
        //}

        // GET: api/classrooms/{classroomId}/tests/5
        [HttpGet("tests/students/{studentId}/classrooms/{classroomId}/tests/{testId}/questions")]
        [Authorize(Policy = "IsStudent")]
        public ActionResult<Entities.Question> GetAllQuestionsForATestForStudent(int classroomId,int testId, int studentId)
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
            if (!_testRepository.IsTestExist(testId))
            {
                return NotFound();
            }

            return Ok(_questionRepository.GetAllQuestionsForATestForAClassroom(classroomId, testId));
        }

        [HttpGet("tests/teachers/{teacherId}/classrooms/{classroomId}/tests/{testId}/questions")]
        [Authorize(Policy = "IsTeacher")]
        public ActionResult<Entities.Question> GetAllQuestionsForATestForTeacher(int classroomId, int testId, int teacherId)
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
            if (!_testRepository.IsTestExist(testId))
            {
                return NotFound();
            }

            return Ok(_questionRepository.GetAllQuestionsForATestForAClassroom(classroomId, testId));
        }

        //[HttpGet("{testId}")]
        //public ActionResult<Entities.Question> GetAllQuestionsForATest(int classroomId, int testId)
        //{
        //    if (!_testRepository.IsTestExist(testId))
        //    {
        //        return NotFound();
        //    }

        //    return Ok(_questionRepository.GetAllQuestionsForATestForAClassroom(classroomId, testId));
        //}


        //// PUT: api/classrooms/{classroomId}/tests/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public IActionResult PutTest(int id, Test test)
        //{
        //    if (id != test.TestId)
        //    {
        //        return BadRequest();
        //    }

        //    return _testRepository.UpdateATest(test) ?
        //        NoContent() :
        //        StatusCode(500);
        //}

        //Create a test
        //// POST: api/classrooms/{classroomId}/tests
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public ActionResult<Test> CreateATest(string testName)
        //{
        //    return _testRepository.CreateAEmptyTest(testName) ?
        //        CreatedAtAction("GetTest", new { id = test.TestId }, test) :
        //        StatusCode(500);
        //}

        //Add questions to a test
        // POST: api/classrooms/{classroomId}/tests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("tests")]
        [Authorize(Policy = "IsTeacher")]
        public ActionResult<Test> CreateATest(Test test)
        {

            if (!_classroomRepository.IsClassroomExist(test.ClassroomId))
            {
                return BadRequest();
            }
            if (!_classroomRepository.DoesTeacherTeachClassroom(int.Parse(User.FindFirst("sub")?.Value), test.ClassroomId))
            {
                return BadRequest();
            }
            if (!_questionRepository.IsQuestionExist(test.QuestionId))
            {
                return BadRequest();
            }
            return _testRepository.CreateATest(test) ?
                CreatedAtAction("GetTest", new { id = test.TestId }, test) :
                StatusCode(500);
        }


        // DELETE: api/classrooms/{classroomId}/tests/5
        [HttpDelete("tests/{testId}")]
        [Authorize(Policy = "IsTeacher")]
        public IActionResult DeleteATest(int testId)
        {
            if (!_testRepository.IsTestExist(testId))
            {
                return NotFound();
            }

            var test = _testRepository.GetTestWithId(testId);
            if (!_classroomRepository.DoesTeacherTeachClassroom(int.Parse(User.FindFirst("sub")?.Value), test.ClassroomId))
            {
                return BadRequest();
            }

            return _testRepository.DeleteATest(testId) ?
                NoContent() :
                StatusCode(500);
        }
    }
}
