using System;
using System.Collections.Generic;

namespace Plugin.Fingerprint.Abstractions
{
    public class SecureFingerprintAuthenticationResult : FingerprintAuthenticationResult
    {
        public Dictionary<string, string> SecureData { get; set; }
    }
}
