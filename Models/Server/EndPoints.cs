using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTraker.Models.Server
{
    public class EndPoints
    {
        public static string CreateProfile = "api/profile";
        public static string Login = "api/login";
        public static string AllActivity = "api/activity";
        public static string AllPrograms = "api/program";
        public static string BaseUrl = "http://localhost:8050/";
    }
}
