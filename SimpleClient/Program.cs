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

            Console.WriteLine("Example Successful Login:");
            var request = new Login() {Username = "chris", Password = "sirhc"};
            var user = sericeClient.Post(request);
            user.PrintDump();

            try {
                Console.WriteLine("Example Invalid Login");
                request = new Login() {Username = "chris", Password = "xxx"};
                user = sericeClient.Post(request);
            }
            catch (WebServiceException webEx) {
                Console.WriteLine("Caught Exception:");
                Console.WriteLine(webEx.ToString());
            }

            Console.WriteLine("Login successfully using Login2:");
            var response = sericeClient.Post<Login2Response>("/security/login2",new Login2 {Username = "chris",Password = "sirhc"});
            response.PrintDump();

            Console.WriteLine("Bad login with HttpError return:");
            try {
                Console.WriteLine("Example Invalid Login");
                response = sericeClient.Post<Login2Response>("/security/login2",new Login2 {Username = "chris",Password = "xxx"});
            }
            catch (WebServiceException webEx) {
                Console.WriteLine("Caught Exception:");
                Console.WriteLine(webEx.ToString());
            }

             Console.WriteLine("Bad login with thrown exception:");
            try {
                Console.WriteLine("Example Invalid Login");
                response = sericeClient.Post<Login2Response>("/security/login2",new Login2 {Username = "Bchris",Password = "xxx"});
            }
            catch (WebServiceException webEx) {
                Console.WriteLine("Caught Exception:");
                Console.WriteLine(webEx.ToString());
            }

          
            Console.WriteLine("Press [enter] to exit");
            Console.ReadLine();
            
            if (inProcHost != null)
            {
                inProcHost.Stop();
                inProcHost.Dispose();
            }
        }
    }

}
