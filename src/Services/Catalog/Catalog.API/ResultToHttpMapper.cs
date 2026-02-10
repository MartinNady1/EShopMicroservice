using ResultPattern;
using System.Net;

namespace Catalog.API
{
    public class ResultToHttpMapper
    {
        public static IResult Map<T>(Result<T> result)
        {
            
            var error = result.Error!;

            return error.Code switch
            {
                HttpStatusCode.BadRequest =>
                    Results.BadRequest(new
                    {
                        error = error.Message,
                        validationErrors = error.ValidationErrors
                    }),

                HttpStatusCode.NotFound =>
                    Results.NotFound(new
                    {
                        error = error.Message
                    }),

                HttpStatusCode.Unauthorized =>
                    Results.Unauthorized(),

                HttpStatusCode.Forbidden =>
                    Results.StatusCode(403),

                HttpStatusCode.Conflict =>
                    Results.Conflict(new
                    {
                        error = error.Message
                    }),

                _ =>
                    Results.StatusCode(500)
            };
        }
    }
}
