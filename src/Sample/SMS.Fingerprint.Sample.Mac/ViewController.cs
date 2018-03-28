using System;
using System.Threading;
using System.Threading.Tasks;
using AppKit;
using CoreFoundation;
using Foundation;
using Plugin.Fingerprint.Abstractions;
using Security;

namespace SMS.Fingerprint.Sample.Mac
{
	public partial class ViewController : NSViewController
	{
		private CancellationTokenSource _cancel;

		public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

		}

		public override NSObject RepresentedObject
		{
			get
			{
				return base.RepresentedObject;
			}
			set
			{
				base.RepresentedObject = value;
			}
		}

		async partial void AuthenticateClicked(NSButton sender)
		{
			//_cancel = swAutoCancel.State == NSCellStateValue.On ? new CancellationTokenSource(TimeSpan.FromSeconds(10)) : new CancellationTokenSource();
			//lblStatus.StringValue = "";
			//var result = await Plugin.Fingerprint.CrossFingerprint.Current.AuthenticateAsync("Prove you have fingers!", _cancel.Token);

			//SetResult(result);

            var secObject = new SecAccessControl(SecAccessible.WhenPasscodeSetThisDeviceOnly, SecAccessControlCreateFlags.TouchIDCurrentSet);

            if (secObject == null)
            {
                string message = "Sec object broken";
                lblStatus.StringValue += message;
            }

            var securityRecord = new SecRecord(SecKind.Key)
            {
                Service = "test",
                ValueData = new NSString("Secret Data").Encode(NSStringEncoding.UTF8),
                AccessControl = secObject
            };

            DispatchQueue.MainQueue.DispatchAsync(() => {
                SecStatusCode status = SecKeyChain.Add(securityRecord);

                lblStatus.StringValue = status.ToString();
            });
		}

		async partial void AddClicked(NSButton sender)
		{
			//_cancel = swAutoCancel.State == NSCellStateValue.On ? new CancellationTokenSource(TimeSpan.FromSeconds(10)) : new CancellationTokenSource();
			//lblStatus.StringValue = "";

			//var dialogConfig = new AuthenticationRequestConfiguration("Beweise, dass du Finger hast!")
			//{
			//	CancelTitle = "Abbrechen",
			//	FallbackTitle = "Anders!"
			//};

			//var result = await Plugin.Fingerprint.CrossFingerprint.Current.AuthenticateAsync(dialogConfig, _cancel.Token);
			//SetResult(result);

            TaskCompletionSource<bool> test = new TaskCompletionSource<bool>();

            DispatchQueue.MainQueue.DispatchAsync(() => {
                test.SetResult(true); 
            });
            var data = await test.Task;
            if (data)
            {
                var alert = new NSAlert
                {
                    AlertStyle = NSAlertStyle.Informational,
                    MessageText = "Success",
                    InformativeText = "it worked!"
                };

                alert.BeginSheet(this.View.Window);
            }
		}

        async partial void RemoveClicked(NSButton sender)
        {
            var alert = new NSAlert
            {
                AlertStyle = NSAlertStyle.Informational,
                MessageText = "Success",
                InformativeText = "Remove clicked!"
            };

            alert.BeginSheet(this.View.Window);
        }

		private void SetResult(FingerprintAuthenticationResult result)
		{
			if (result.Authenticated)
			{
				var alert = new NSAlert
				{
					AlertStyle = NSAlertStyle.Informational,
					MessageText = "Success",
					InformativeText = "You have Fingers!"
				};

				alert.BeginSheet(this.View.Window);
			}
			else
			{
				lblStatus.StringValue = $"{result.Status}: {result.ErrorMessage}";
			}
		}
	}
}