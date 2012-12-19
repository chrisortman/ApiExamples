using System;
using Api.Example.Security;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Linq;

namespace Api.Example.Security {
    public class SecurityService : IService {
         public object Post(Login request) {
             if (request.Username == Reverse(request.Password)) {
                 return new SecurityUser() {
                     Username = request.Username,
                     EmailAddress = request.Username + "@example.local",
                     Timezone = -6
                 };
             }
             else {
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