using System.Threading;
using System.Threading.Tasks;
using Plugin.Fingerprint.Abstractions;

namespace Plugin.Fingerprint
{
    internal class FingerprintImplementation : FingerprintImplementationBase
    {
        public override Task<FingerprintAvailability> GetAvailabilityAsync(bool allowAlternativeAuthentication = false)
        {
            return Task.FromResult(FingerprintAvailability.NoImplementation);
        }

        public override Task<AuthenticationType> GetAuthenticationTypeAsync()
        {
            return Task.FromResult(AuthenticationType.None);
        }

        protected override Task<FingerprintAuthenticationResult> NativeAuthenticateAsync(AuthenticationRequestConfiguration authRequestConfig, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(new FingerprintAuthenticationResult
            {
                Status = FingerprintAuthenticationResultStatus.NotAvailable,
                ErrorMessage = "Not implemented for the current platform."
            });
        }

        public override Task<SecureFingerprintAuthenticationResult> NativeSecureAuthenticateAsync(AuthenticationRequestConfiguration authRequestConfig,string key, CancellationToken cancellationToken)
        {
            return Task.FromResult(new SecureFingerprintAuthenticationResult
            {
                Status = FingerprintAuthenticationResultStatus.NotAvailable,
                ErrorMessage = "Not implemented for the current platform."
            });
        }

        public override Task<bool> AddSecureDataAsync(string key, string value)
        {
            throw new System.NotImplementedException();
        }

        public override Task<bool> RemoveSecureDataAsync(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}