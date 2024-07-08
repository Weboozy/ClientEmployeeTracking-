using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTraker.Models
{
    public interface IApiClient
    {
        Task<RestResponse> CreateProfile(Profile profile);
        Task<RestResponse> Authenticate(string login,string password,string lastName,string firstName);
        Task<RestResponse> GetAllActivity();
        Task<RestResponse> GetAllPrograms();
        Task<RestResponse> GetAllProfiles();
        Task<RestResponse> GetActivityById(string id);
        Task<RestResponse> GetProfileById(string id);
    }
}
