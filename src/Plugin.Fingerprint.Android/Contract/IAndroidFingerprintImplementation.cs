using System.Threading;
using System.Threading.Tasks;
using Plugin.Fingerprint.Abstractions;

namespace Plugin.Fingerprint.Contract
{
    public interface IAndroidFingerprintImplementation
    {
        Task<FingerprintAuthenticationResult> AuthenticateNoDialogAsync(IAuthenticationFailedListener failedListener, CancellationToken cancellationToken);

        Task<SecureFingerprintAuthenticationResult> AuthenticateSecureNoDialogAsync(IAuthenticationFailedListener failedListener, string key, CancellationToken cancellationToken);
    }
}