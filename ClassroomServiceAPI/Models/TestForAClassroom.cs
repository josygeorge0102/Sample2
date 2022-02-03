using System.ComponentModel.DataAnnotations;

namespace ClassroomServiceAPI.Models
{
    public class TestForAclassroom
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public DateTimeOffset? TestDate { get; set; }
       
    }
}

