using cotr.backend.Model;

namespace cotr.backend.test.Moq
{
    public class ApiExceptionMockData
    {
        public static ApiException ApiException401 = new(401, "Unauthorized");
        public static ApiException ApiException404 = new(401, "NotFound");
    }
}
