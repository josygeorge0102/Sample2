using System.ComponentModel.DataAnnotations;

namespace ClassroomServiceAPI.Entities
{
    public class Classroom
    {
        [Key]
        public int ClassroomId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Subject { get; set; }
        public string ImageUrl { get; set; }
        //configuring the default value {false} using Model builder in ClassroomDbContext.cs
        public bool IsArchived { get; set; }
        public int TeacherId { get; set; }

    }
}
