using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.LocalNotifications;
using Android.Content;

namespace NotificationPOC.Droid
{
    [Activity(Label = "NotificationPOC", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            StartService(new Intent(this, typeof(TaskEndService)));
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
    }

    [Service(Label = "TaskEndService")]
    [IntentFilter(new String[] { "com.sushihangover.TaskEndService" })]
    public class TaskEndService : Service
    {
        IBinder binder;

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            binder = new TaskEndServiceBinder(this);
            return binder;
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            CrossLocalNotifications.Current.Show("CLOSED", "Notification APP Killed");
            base.OnTaskRemoved(rootIntent);
        }
    }

    public class TaskEndServiceBinder : Binder
    {
        readonly TaskEndService service;

        public TaskEndServiceBinder(TaskEndService service)
        {
            this.service = service;
        }

        public TaskEndService GetTaskEndService()
        {
            return service;
        }
    }
}