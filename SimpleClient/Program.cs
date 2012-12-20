using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Example.Security;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;

namespace SimpleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new[] {"LOCAL"};

            IRestClient sericeClient;
            InProcessAppHost inProcHost = null;
            if (args.Length > 0 && args[0] == "LOCAL")
            {
                inProcHost = new InProcessAppHost();
                inProcHost.Init();
                inProcHost.Start("http://localhost:8888/");
                sericeClient = new JsonServiceClient("http://localhost:8888");
            }
            else
            {
                sericeClient = new JsonServiceClient("http://localhost:32284");

            }

            EnableRequestTiming((JsonServiceClient) sericeClient);

            SuccessfulLogin(sericeClient);

            //InvalidLogin(sericeClient);

            //SuccessfulLogin2ShowsAlternativeReturnFormat(sericeClient);

            //BadLoginWithHttpErrorReturn(sericeClient);

            //BadLoginWithThrownException(sericeClient);


            Finish(inProcHost);
        }

        private static void EnableRequestTiming(JsonServiceClient sericeClient)
        {
            DateTime start = DateTime.MinValue;
            string requestUrl = "";
            sericeClient.LocalHttpWebRequestFilter = request =>
            {
                start = DateTime.UtcNow;
                requestUrl = request.RequestUri.ToString();
            };

            sericeClient.LocalHttpWebResponseFilter = response =>
            {
                Console.WriteLine("Web Service Call to " + requestUrl + " took: " + DateTime.UtcNow.Subtract(start));
            };
        }

        private static void Finish(InProcessAppHost inProcHost)
        {
            Console.WriteLine("Press [enter] to exit");
            Console.ReadLine();

            if (inProcHost != null)
            {
                inProcHost.Stop();
                inProcHost.Dispose();
            }
        }

        private static void BadLoginWithThrownException(IRestClient sericeClient)
        {
            Console.WriteLine("Bad login with thrown exception:");
            try
            {
                Console.WriteLine("Example Invalid Login");
                var response = sericeClient.Post<Login2Response>("/security/login2",
                                                             new Login2 {Username = "Bchris", Password = "xxx"});
            }
            catch (WebServiceException webEx)
            {
                Console.WriteLine("Caught Exception:");
                Console.WriteLine(webEx.ToString());
            }
        }

        private static void BadLoginWithHttpErrorReturn(IRestClient sericeClient)
        {
            Console.WriteLine("Bad login with HttpError return:");
            try
            {
                Console.WriteLine("Example Invalid Login");
                var response = sericeClient.Post<Login2Response>("/security/login2",
                                                             new Login2 {Username = "chris", Password = "xxx"});
            }
            catch (WebServiceException webEx)
            {
                Console.WriteLine("Caught Exception:");
                Console.WriteLine(webEx.ToString());
            }
        }

        private static void SuccessfulLogin2ShowsAlternativeReturnFormat(IRestClient sericeClient)
        {
            Console.WriteLine("Login successfully using Login2:");
            var response = sericeClient.Post<Login2Response>("/security/login2",
                                                             new Login2 {Username = "chris", Password = "sirhc"});
            response.PrintDump();
        }

        private static void InvalidLogin(IRestClient sericeClient)
        {
            try
            {
                Console.WriteLine("Example Invalid Login");
                var request = new Login() {Username = "chris", Password = "xxx"};
                var user = sericeClient.Post<SecurityUser>(request);
            }
            catch (WebServiceException webEx)
            {
                Console.WriteLine("Caught Exception:");
                Console.WriteLine(webEx.ToString());
            }
        }

        private static void SuccessfulLogin(IRestClient sericeClient)
        {
            Console.WriteLine("Example Successful Login:");
            var request = new Login() {Username = "chris", Password = "sirhc"};
            var user = sericeClient.Post(request);
            user.PrintDump();
        }
    }

}
