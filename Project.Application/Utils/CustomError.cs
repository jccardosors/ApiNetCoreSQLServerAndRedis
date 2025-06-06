using System.Net;

namespace Project.Application.Utils
{
    public sealed record CustomError(string Code, string Message)
    {

        public static readonly CustomError None = new(string.Empty, string.Empty);

        public static CustomError RecordNotFound(string message)
        {
            return new CustomError(((int)HttpStatusCode.NotFound).ToString(), message);
        }

        public static CustomError ExceptionError(string message)
        {
            return new CustomError(((int)HttpStatusCode.InternalServerError).ToString(), message);
        }

        public static CustomError ValidationError(string message)
        {
            return new CustomError(((int)HttpStatusCode.PreconditionRequired).ToString(), message);
        }
    }
}
