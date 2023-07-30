using System.Net;

namespace IGWebApiClient
{
    public class IgResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T Response { get; set; }

        public static implicit operator bool(IgResponse<T> inst) =>
            inst.StatusCode == HttpStatusCode.OK;

        public static implicit operator HttpStatusCode(IgResponse<T> inst) =>
            inst.StatusCode;

        public static implicit operator T(IgResponse<T> inst) =>
            inst.Response;
    }
}