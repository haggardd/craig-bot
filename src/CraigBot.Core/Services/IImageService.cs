using System.Threading.Tasks;

namespace CraigBot.Core.Services
{
    public interface IImageService
    {
        Task<string> GetRandomDog();

        Task<string> GetRandomCat();

        Task<string> GetRandomFox();
    }
}