using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace GPSImageTag.Core.Interfaces
{
    public interface IAuthentication
    {
        Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider);
        void ClearCookies();
    }
}
