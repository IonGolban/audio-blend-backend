
using System.ComponentModel.DataAnnotations;

namespace AudioBlend.Core.Shared.Responses
{
    public class Response<T> : BaseResponse where T : class
    {
        public Response() : base()
        {

        }
        public T Data { get; set; } = default!;
        public Dictionary<string, string>? ErrorsKeyMessage { get; set; }


        //public static Response<T> ErrorResponseFromKeyMessage(string errorMessage, string errorKey)
        //{
        //    return new Response<T>
        //    {
        //        Success = false,
        //        ValidationErrors = new Dictionary<string, string> { { errorKey, errorMessage } }
        //    };
        //}

    }
}
