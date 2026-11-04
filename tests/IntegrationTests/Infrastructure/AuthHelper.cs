using System.Net.Http.Headers;

namespace IntegrationTests.Infrastructure;

public static class AuthHelper
{
    public static void AuthenticateAsStaff(HttpClient client)
    {
        var token = TestJwt.CreateStaffToken();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
