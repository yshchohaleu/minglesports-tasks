namespace Minglesports.Tasks.Web.Services
{
    public class ApiConstants
    {
        public class HttpHeaders
        {
            public const string UserIdHeader = "MS-User-Id";
            public const string TaskIdTransactionHeader = "MS-Task-Id";
        }

        public class ErrorCodes
        {
            public const string NotFound = "NotFound";
            public const string Conflict = "Conflict";
            public const string InternalError = "InternalError";
            public const string BadRequest = "BadRequest";
        }
    }
}