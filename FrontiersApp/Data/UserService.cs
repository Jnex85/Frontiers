namespace FrontiersApp.Data
{
    public class UserService
    {
        public async Task<UserModel[]> GetRegisteredUsersAsync()
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            var result = await client.GetFromJsonAsync<List<UserModel>>("http://localhost:5050/api/user");
            return result.ToArray();
        }

        public async Task<string> InviteReviewer(int id)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            var result = await client.PutAsJsonAsync("http://localhost:5050/api/User/InviteReviewer", id);
            return await result.Content.ReadAsStringAsync();
        }
    }
}