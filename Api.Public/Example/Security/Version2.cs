
namespace Api.Example.Security
{
    public partial class Login
    {
        public int Version { get; set; }

        public string SecurityAlgorithm { get; set; }

        public Login()
        {
            Version = 2;
        }
    }
}