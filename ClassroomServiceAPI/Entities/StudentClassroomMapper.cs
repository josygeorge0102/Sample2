namespace ClassroomServiceAPI.Entities
{
    public class StudentClassroomMapper
    {
        //configured the composite key { ClassroomId, StudentId } using Model builder in ClassroomDbContext.cs

        public int StudentId { get; set; }
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
    }
}
