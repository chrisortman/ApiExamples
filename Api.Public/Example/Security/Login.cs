﻿using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Api.Example.Security {
    [Route("/security/login")]
    public partial class Login : IReturn<SecurityUser> {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SecurityUser {
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public int Timezone { get; set; }
    }

    [Route("/security/login2")]
    public class Login2 {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Login2Response : IHasResponseStatus {
        public ResponseStatus ResponseStatus { get; set; }
        public SecurityUser LoggedInUser { get; set; }
    }
}