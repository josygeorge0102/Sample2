namespace ClassroomServiceAPI.Repository
{
    public class HelperCustomService
    {
        private readonly HttpClient _client;

        public HelperCustomService(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> IsUserExists(int userId)
        {
            var response = await _client.GetAsync("");
            return false;
        }
    }
}
