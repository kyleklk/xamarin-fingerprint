using System;
using System.Threading.Tasks;
using Android.Hardware.Fingerprints;
using Android.Support.V4.Hardware.Fingerprint;
using Java.Lang;
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint.Contract;

namespace Plugin.Fingerprint.Standard
{
    public class SecureFingerprintAuthenticationCallback : FingerprintManagerCompat.AuthenticationCallback, ISecureAuthenticationCallback
    {
        private readonly IAuthenticationFailedListener _listener;
        private readonly string _key;
        private readonly TaskCompletionSource<SecureFingerprintAuthenticationResult> _taskCompletionSource;

        public SecureFingerprintAuthenticationCallback(IAuthenticationFailedListener listener, string key)
        {
            _key = key;
            _listener = listener;
            _taskCompletionSource = new TaskCompletionSource<SecureFingerprintAuthenticationResult>();
        }

        public async Task<SecureFingerprintAuthenticationResult> GetTask()
        {
            return await _taskCompletionSource.Task;
        }

        // https://developer.android.com/reference/android/hardware/fingerprint/FingerprintManager.AuthenticationCallback.html
        public override void OnAuthenticationSucceeded(FingerprintManagerCompat.AuthenticationResult res)
        {
            base.OnAuthenticationSucceeded(res);
            //todo: read the key value from the android keystore
            var result = new SecureFingerprintAuthenticationResult { Status = FingerprintAuthenticationResultStatus.Succeeded };
            SetResultSafe(result);
        }

        public override void OnAuthenticationError(int errMsgId, ICharSequence errString)
        {
            var errorCode = (FingerprintState)errMsgId;
            base.OnAuthenticationError(errMsgId, errString);
            var message = errString != null ? errString.ToString() : string.Empty;
            var result = new SecureFingerprintAuthenticationResult { Status = FingerprintAuthenticationResultStatus.Failed, ErrorMessage = message };

            if (errorCode == FingerprintState.ErrorLockout)
            {
                result.Status = FingerprintAuthenticationResultStatus.TooManyAttempts;
            }

            SetResultSafe(result);
        }

        private void SetResultSafe(SecureFingerprintAuthenticationResult result)
        {
            if (!(_taskCompletionSource.Task.IsCanceled || _taskCompletionSource.Task.IsCompleted || _taskCompletionSource.Task.IsFaulted))
            {
                _taskCompletionSource.SetResult(result);
            }
        }

        public override void OnAuthenticationFailed()
        {
            base.OnAuthenticationFailed();
            _listener?.OnFailedTry();
        }

        public override void OnAuthenticationHelp(int helpMsgId, ICharSequence helpString)
        {
            base.OnAuthenticationHelp(helpMsgId, helpString);
            _listener?.OnHelp(FingerprintAuthenticationHelp.MovedTooFast, helpString?.ToString());
        }

    }
}