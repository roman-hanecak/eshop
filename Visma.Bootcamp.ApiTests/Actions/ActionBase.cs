using RestSharp;

namespace Visma.Bootcamp.ApiTests.Actions
{
    public abstract class ActionBase
    {
        private readonly string _resource;

        protected ActionBase(string resource)
        {
            _resource = resource;
        }

        protected RestRequest CreateRequest(Method httpMethod, string methodName)
        {
            var request = new RestRequest
            {
                Resource = $"{_resource}/{methodName}",
                RequestFormat = DataFormat.Json,
                Method = httpMethod
            };
            
            return request;
        }

        protected abstract RestResponse<T> Execute<T>(RestRequest request);
    }
}
