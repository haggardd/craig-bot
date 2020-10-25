using System.Threading.Tasks;

// TODO: See if its possible to restructure the projects into actual folders, rather than just solution folders
namespace CraigBot.Bot
{
    public static class Program
    {
        public static Task Main(string[] args)
            => Startup.RunAsync(args);
    }
}