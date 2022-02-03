using Microsoft.EntityFrameworkCore;
namespace ClassroomServiceAPI
{
    public static class SeedData
    {
        public static void SeedClassroomService(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ClassroomServiceDbContext>())
                {
                    System.Console.WriteLine("!!!!!!!!!!!!!!!!!!!!MIGRATING DATABASE!!!!!!!!!!!!!!!!!!!");
                    context.Database.Migrate();
                }
            }
        }
    }
}