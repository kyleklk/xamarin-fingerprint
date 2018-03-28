using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Hardware.Fingerprints;
using Android.OS;
using Android.Support.V4.Hardware.Fingerprint;
using Android.Util;
using Java.Lang;
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint.Contract;
using Plugin.Fingerprint.Utils;

namespace Plugin.Fingerprint.Standard
{
    public class StandardFingerprintImplementation : AndroidFingerprintImplementationBase
    {
        public override async Task<FingerprintAuthenticationResult> AuthenticateNoDialogAsync(IAuthenticationFailedListener failedListener, CancellationToken cancellationToken)
        {
            using (var cancellationSignal = new CancellationSignal())
            using (cancellationToken.Register(() => cancellationSignal.Cancel()))
            {
                var callback = new FingerprintAuthenticationCallback(failedListener);
                GetService().Authenticate(null, cancellationSignal, FingerprintAuthenticationFlags.None, callback, null);
                return await callback.GetTask();
            }
        }

        private static FingerprintManager GetService()
        {
            return (FingerprintManager)Application.Context.GetSystemService(Class.FromType(typeof(FingerprintManager)));
        }

        private static FingerprintManagerCompat GetServiceCompat()
        {
            return (FingerprintManagerCompat)Application.Context.GetSystemService(Class.FromType(typeof(FingerprintManagerCompat)));
        }

        public override async Task<FingerprintAvailability> GetAvailabilityAsync(bool allowAlternativeAuthentication = false)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.M)
                return FingerprintAvailability.NoApi;

            var context = Application.Context;
            if (context.CheckCallingOrSelfPermission(Manifest.Permission.UseFingerprint) != Permission.Granted)
                return FingerprintAvailability.NoPermission;

            try
            {
                // service can be null certain devices #83
                var fpService = GetService();
                if (fpService == null)
                    return FingerprintAvailability.NoApi;

                if (!fpService.IsHardwareDetected)
                    return FingerprintAvailability.NoSensor;

                if (!fpService.HasEnrolledFingerprints)
                    return FingerprintAvailability.NoFingerprint;

                return FingerprintAvailability.Available;
            }
            catch(Throwable e)
            {
                // ServiceNotFoundException can happen on certain devices #83
                Log.Error(nameof(StandardFingerprintImplementation), e, "Could not create Android service");
                return FingerprintAvailability.Unknown;
            }
        }

        public override async Task<bool> AddSecureDataAsync(string key, string value)
        {
            //TODO: write data to the android keystore
            throw new System.NotImplementedException();
        }

        public override async Task<bool> RemoveSecureDataAsync(string key)
        {
            //TODO: remove data from android keystore
            throw new System.NotImplementedException();
        }

        public override async Task<SecureFingerprintAuthenticationResult> AuthenticateSecureNoDialogAsync(IAuthenticationFailedListener failedListener, string key, CancellationToken cancellationToken)
        {
            using (var cancellationSignal = new Android.Support.V4.OS.CancellationSignal())
            using (cancellationToken.Register(() => cancellationSignal.Cancel()))
            {
                CryptoObjectHelper crypto = new CryptoObjectHelper();
                var callback = new SecureFingerprintAuthenticationCallback(failedListener, key);
                GetServiceCompat().Authenticate(crypto.BuildCryptoObject(), (int)FingerprintAuthenticationFlags.None, cancellationSignal, callback, null);
                return await callback.GetTask();
            }
        }
    }
}