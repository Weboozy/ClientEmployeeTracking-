using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTraker.Models.Server
{
    public class ApiImplementation : IApiClient
    {
        private JwtAuthenticator _authenticator;
        private readonly RestClient _client;
        public ApiImplementation(string url)
        {
            RestClientOptions options = new RestClientOptions(url);
            _client = new RestClient(options);
        }
        public ApiImplementation(string url,string token)
        {
            _authenticator = new JwtAuthenticator(token);
            RestClientOptions options = new RestClientOptions(url)
            {
                Authenticator = _authenticator
            };
            _client = new RestClient(options);
        }
        public async Task<RestResponse> CreateProfile(Profile payload)
        {
            var request = new RestRequest(EndPoints.CreateProfile, Method.Post);
            request.AddBody(payload);
            return await _client.ExecuteAsync(request);
        }

        public Task<RestRequest> GetProfileById(Guid profileId)
        {
            throw new NotImplementedException();
        }

        public async Task<RestResponse> Authenticate(string login, string password, string lastName, string firstName)
        {
            var request = new RestRequest(EndPoints.Login,Method.Get);
            request.AddBody(new { LastName = lastName,FirstName = firstName,Email= login,Password = password });
            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> GetAllActivity()
        {
            var request = new RestRequest(EndPoints.AllActivity, Method.Get);
            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> GetAllPrograms()
        {
            var request = new RestRequest(EndPoints.AllPrograms, Method.Get);
            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> GetAllProfiles()
        {
            var request = new RestRequest(EndPoints.CreateProfile, Method.Get);
            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> GetActivityById(string id)
        {
            var request = new RestRequest(EndPoints.AllActivity + $"/{id}", Method.Get);
            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> GetProfileById(string id)
        {
            var request = new RestRequest(EndPoints.CreateProfile+$"/{id}", Method.Get);
            return await _client.ExecuteAsync(request);
        }
    }
}
