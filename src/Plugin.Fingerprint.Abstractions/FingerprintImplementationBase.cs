using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin.Fingerprint.Abstractions
{
    public abstract class FingerprintImplementationBase : IFingerprint
    {
        public async Task<FingerprintAuthenticationResult> AuthenticateAsync(string reason, CancellationToken cancellationToken = default)
        {
            return await AuthenticateAsync(new AuthenticationRequestConfiguration(reason), cancellationToken);
        }

        public async Task<FingerprintAuthenticationResult> AuthenticateAsync(AuthenticationRequestConfiguration authRequestConfig, CancellationToken cancellationToken = default)
        {
            if(!await IsAvailableAsync(authRequestConfig.AllowAlternativeAuthentication))
                return new FingerprintAuthenticationResult { Status = FingerprintAuthenticationResultStatus.NotAvailable };

            return await NativeAuthenticateAsync(authRequestConfig, cancellationToken);
        }

        public async Task<bool> IsAvailableAsync(bool allowAlternativeAuthentication = false)
        {
            return await GetAvailabilityAsync(allowAlternativeAuthentication) == FingerprintAvailability.Available;
        }

        public abstract Task<FingerprintAvailability> GetAvailabilityAsync(bool allowAlternativeAuthentication = false);

        public abstract Task<AuthenticationType> GetAuthenticationTypeAsync();

        protected abstract Task<FingerprintAuthenticationResult> NativeAuthenticateAsync(AuthenticationRequestConfiguration authRequestConfig, CancellationToken cancellationToken);

        public async Task<SecureFingerprintAuthenticationResult> AuthenticateSecureAsync(string reason, string key,CancellationToken cancellationToken = default)
        {
            return await AuthenticateSecureAsync(new AuthenticationRequestConfiguration(reason),key, cancellationToken);
        }

        public async Task<SecureFingerprintAuthenticationResult> AuthenticateSecureAsync(AuthenticationRequestConfiguration authRequestConfig, string key,CancellationToken cancellationToken = default)
        {
            if (!await IsAvailableAsync(authRequestConfig.AllowAlternativeAuthentication))
                return new SecureFingerprintAuthenticationResult { Status = FingerprintAuthenticationResultStatus.NotAvailable };

            return await NativeSecureAuthenticateAsync(authRequestConfig, key, cancellationToken);
        }

        public abstract Task<SecureFingerprintAuthenticationResult> NativeSecureAuthenticateAsync(AuthenticationRequestConfiguration authRequestConfig, string key, CancellationToken cancellationToken);

        public abstract Task<bool> AddSecureDataAsync(string key, string value);

        public abstract Task<bool> RemoveSecureDataAsync(string key);
    }
}