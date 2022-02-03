#nullable disable
using ClassroomServiceAPI.Entities;
using ClassroomServiceAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomServiceAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IStudentClassroomRepository _studentClassroomRepository;
        private readonly IClassroomRepository _classroomRepository;
        
        public QuestionsController(IQuestionRepository questionRepository,
                                    IStudentClassroomRepository studentClassroomRepository,
                                    IClassroomRepository classroomRepository)
        {
            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
            _studentClassroomRepository = studentClassroomRepository ?? throw new ArgumentNullException(nameof(studentClassroomRepository));
            _classroomRepository = classroomRepository ?? throw new ArgumentNullException(nameof(classroomRepository));
        }        
   
        
        [HttpGet("questions/teachers/{teacherId}/subjects/{subject}/questions")]
        [Authorize(Policy = "IsTeacher")]
        public ActionResult<IEnumerable<Question>> GetAllQuestionsForATeacherForASubject(int teacherId, string subject)
        {
            if (teacherId != int.Parse(User.FindFirst("sub")?.Value))
	        {
                return BadRequest();
	        }
            return Ok(_questionRepository.GetAllQuestionsForATeacherForASubject(teacherId, subject));
        }

       [HttpGet("questions/teachers/{teacherId}/questions")]
        [Authorize(Policy = "IsTeacher")]
        public ActionResult<IEnumerable<Question>> GetAllQuestionsForATeacher(int teacherId)
        {
            if (teacherId != int.Parse(User.FindFirst("sub")?.Value))
	        {
                return BadRequest();
	        }
            return Ok(_questionRepository.GetAllQuestionsForATeacher(teacherId));
        }

        [HttpGet("questions/student/{studentId}/classrooms/{classroomId}/tests/{testId}/questions")]
        [Authorize(Policy = "IsStudent")]
        public ActionResult<IEnumerable<Question>> GetAllQuestionsForAstudentForANewTestForAClassroom(int studentId, int classroomId, int testId)
        {
            if (!_studentClassroomRepository.IsStudentExistsInClassroom(studentId, classroomId))
            {
                return NotFound();
            }
            return Ok(_questionRepository.GetAllQuestionsForATestForAClassroom(classroomId, testId));
        }

        [HttpGet("questions/{studentId}/classrooms/{classroomId}/tests/{testId}/test")]
        [Authorize(Policy = "IsStudent")]
        public ActionResult<IEnumerable<Question>> GetAllQuestionsForAStudentForAClassroomForATest(int studentId, int classroomId, int testId)
        {
            if (studentId != int.Parse(User.FindFirst("sub")?.Value))
	        {
                return BadRequest();
	        }
            if (!_studentClassroomRepository.IsStudentExistsInClassroom(studentId, classroomId))
	        {
                return NotFound();
	        }
            return Ok(_questionRepository.GetAllQuestionsForAStudentForAClassroomForATest(studentId, classroomId, testId));
        }

        // PUT: api/classrooms/{classroomId}/tests/{testId}/questions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("questions/{questionId}")]
        [Authorize(Policy = "IsTeacher")]
        public IActionResult PutQuestion(int questionId, Question question)
        {
            if (questionId != question.QuestionId)
            {
                return BadRequest();
            }
            if (question.TeacherId != int.Parse(User.FindFirst("sub")?.Value))
	        {
                return BadRequest();
	        }
            return _questionRepository.UpdateAQuestion(question) ? 
                NoContent() : 
                StatusCode(500);
        }

        // POST: api/classrooms/{classroomId}/tests/{testId}/questions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("questions")]
        [Authorize(Policy = "IsTeacher" )]
        public ActionResult<Question> PostQuestion(Question question)
        {
            if (question.TeacherId != int.Parse(User.FindFirst("sub")?.Value))
	        {
                return BadRequest();
	        }
            return _questionRepository.CreateAQuestion(question) ?
                CreatedAtAction("GetQuestion", new { id = question.QuestionId }, question) :
                StatusCode(500);
        }

        // DELETE: api/classrooms/{classroomId}/tests/{testId}/questions/5
        [HttpDelete("questions/{questionId}")]
        [Authorize(Policy = "IsTeacher" )]
        public IActionResult DeleteQuestion(int questionId)
        {
            if (_questionRepository.IsQuestionExist(questionId))
            {
                return NotFound();
            }
            var question = _questionRepository.GetQuestionWithId(questionId);

            if (question.TeacherId != int.Parse(User.FindFirst("sub")?.Value))
	        {
                return BadRequest();
	        }

            return _questionRepository.DeleteAQuestion(_questionRepository.GetQuestionWithId(questionId)) ?
                NoContent() :
                StatusCode(500);
        }
    }
}
