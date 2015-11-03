using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Timers;
using Android.Media;

namespace EveryDayClock
{
    [Activity(Label = "GoodMorningActivity")]
    public class GoodMorningActivity : Activity
    {
        TextView _LeftTime;
        MediaPlayer _AlarmMediPlayer;
        Handler mHandler = new Handler();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.GoodMorning);

            _AlarmMediPlayer = MediaPlayer.Create(this, Resource.Raw.alarm);

            //アラート時間
            var alarmTime = FindViewById<TextView>(Resource.Id.alarmTime);
            alarmTime.Text = DateTime.Parse(MainActivity.AlarmTime).ToString("HH:mm");

            //ストップボタン
            var stopButton = FindViewById<Button>(Resource.Id.StopButton);
            stopButton.Click += ((o, e) => {
                _AlarmMediPlayer?.Stop();

                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                return;
            });
            stopButton.Visibility = ViewStates.Invisible;

            //キャンセルボタン
            var cancelButton = FindViewById<Button>(Resource.Id.CancelButton);
            cancelButton.Click += ((o, e) => {
                _AlarmMediPlayer?.Stop();

                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                return;
            });

            //残り時間
            var leftTimer = new Timer()
            {
                Enabled = true,
                Interval = 100
            };
            _LeftTime = FindViewById<TextView>(Resource.Id.leftTime);
            var textClock = FindViewById<TextClock>(Resource.Id.textClock);
            leftTimer.Elapsed += ((o, e) =>
            {
                mHandler.Post(() => {
                    var c = DateTime.Parse(textClock.Text);
                    var a = DateTime.Parse(MainActivity.AlarmTime);
                    var span = a - c;
                    if (span.Ticks > 0)
                    {
                        _LeftTime.Text = span.ToString();
                    }
                    else
                    {
                        _LeftTime.Text = "00:00:00";
                        if (_AlarmMediPlayer?.IsPlaying == false)
                        {
                            _AlarmMediPlayer?.Start();
                            stopButton.Visibility = ViewStates.Visible;
                            cancelButton.Visibility = ViewStates.Visible;
                        }
                    }
                });
                
            });
            leftTimer.Start();



        }
    }
}