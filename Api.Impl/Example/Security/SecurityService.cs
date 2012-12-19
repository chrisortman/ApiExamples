using System;
using Api.Example.Security;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Linq;

namespace Api.Example.Security {
    public class SecurityService : IService {
        public object Post(Login request)
        {
            //V1 of service did not allow selection of password algorithm
            //now if we get an old version request, then we will just pick the correct default
            if (request.Version < 2)
            {
                request.SecurityAlgorithm = "reverse";
            }

            Func<string, string> passwordEncrypt = null;
            switch (request.SecurityAlgorithm)
            {
            case "reverse":
                passwordEncrypt = Reverse;
                break;
            case "upper":
                passwordEncrypt = s => s.ToUpper();
                break;
            default:
                throw new HttpError("Invalid password algorithm");
            }



            if (request.Password == passwordEncrypt(request.Username))
            {
                return new SecurityUser()
                       {
                           Username = request.Username,
                           EmailAddress = request.Username + "@example.local",
                           Timezone = -6
                       };
            }
            else
            {
                return new HttpError("Invalid username and password");
            }
        }

        public static string Reverse(string s) {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }


    public class SecurityService2 : IService {
        public object Post(Login2 request) {
             if (request.Username == SecurityService.Reverse(request.Password)) {
                 var user =  new SecurityUser() {
                     Username = request.Username,
                     EmailAddress = request.Username + "@example.local",
                     Timezone = -6
                 };

                 return new Login2Response {LoggedInUser = user};
             } else if (request.Username.StartsWith("B")) {
                 throw new Exception("No usernames start with B");
             }
             else {
                 return new HttpError("Invalid username and password");
             }
        }
    }
}