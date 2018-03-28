using System.Threading;
using System.Threading.Tasks;
using Plugin.Fingerprint.Abstractions;

namespace Plugin.Fingerprint.Contract
{
    /// <summary>
    /// Base implementation for the Android implementations.
    /// </summary>
    public abstract class AndroidFingerprintImplementationBase : FingerprintImplementationBase, IAndroidFingerprintImplementation
    {
        protected override async Task<FingerprintAuthenticationResult> NativeAuthenticateAsync(AuthenticationRequestConfiguration authRequestConfig, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (authRequestConfig.UseDialog)
            {
                var fragment = CrossFingerprint.CreateDialogFragment();
                return await fragment.ShowAsync(authRequestConfig, this, cancellationToken);
            }

            return await AuthenticateNoDialogAsync(new DeafAuthenticationFailedListener(), cancellationToken);
        }

        public override async Task<SecureFingerprintAuthenticationResult> NativeSecureAuthenticateAsync(AuthenticationRequestConfiguration authRequestConfig,string key, CancellationToken cancellationToken)
        {
            if (authRequestConfig.UseDialog)
            {
                var fragment = CrossFingerprint.CreateDialogFragment();
                return await fragment.ShowSecureAsync(authRequestConfig, key,this, cancellationToken);
            }

            return await AuthenticateSecureNoDialogAsync(new DeafAuthenticationFailedListener(), key, cancellationToken);
        }

        public abstract Task<FingerprintAuthenticationResult> AuthenticateNoDialogAsync(IAuthenticationFailedListener failedListener, CancellationToken cancellationToken);

        public abstract Task<SecureFingerprintAuthenticationResult> AuthenticateSecureNoDialogAsync(IAuthenticationFailedListener failedListener,string key, CancellationToken cancellationToken);

        public override async Task<AuthenticationType> GetAuthenticationTypeAsync()
        {
            var availability = await GetAvailabilityAsync(false);
            if (availability == FingerprintAvailability.NoFingerprint ||
                availability == FingerprintAvailability.NoPermission ||
                availability == FingerprintAvailability.Available)
            {
                return AuthenticationType.Fingerprint;
            }

            return AuthenticationType.None;
        }

    }
}