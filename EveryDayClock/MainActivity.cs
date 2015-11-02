using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;
using System.Timers;

namespace EveryDayClock
{
    [Activity(Label = "EveryDayClock", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private TextView time_display;
        private int hour;
        private int minute;
        private bool _TimePickDialogPopup;
        const int TIME_DIALOG_ID = 0;

        MediaPlayer _player;

        Timer _timer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _player = MediaPlayer.Create(this, Resource.Raw.test);

            var clock = time_display = FindViewById<TextClock>(Resource.Id.textClock1);
            time_display = FindViewById<TextView>(Resource.Id.textAram);
            time_display.Click += ((o, e) =>
            {
                _TimePickDialogPopup = true;
                ShowDialog(TIME_DIALOG_ID);
            });
            hour = DateTime.Now.Hour;
            minute = DateTime.Now.Minute;

            //止める
            var stop = FindViewById<Button>(Resource.Id.Stop);
            stop.Click += ((o, e) => {
                _player?.Stop();
                _player = MediaPlayer.Create(this, Resource.Raw.test);
            });

            //タイマー
            _timer = new Timer()
            {
                Enabled = true,
                Interval = 1000
            };
            _timer.Elapsed += ((o, e) =>
            {
                if (_TimePickDialogPopup) return;

                DateTime c = DateTime.Parse(clock.Text);
                DateTime t = DateTime.Parse(time_display.Text);
                if (_player?.IsPlaying == false && c.Hour == t.Hour && c.Minute == t.Minute)
                {
                    //_player?.Start();
                }
            });
            _timer.Start();

            UpdateDisplay();
            _TimePickDialogPopup = false;
        }
        private void UpdateDisplay()
        {
            string time = string.Format("{0}:{1}", hour, minute.ToString().PadLeft(2, '0'));
            time_display.Text = time;
        }

        private void TimePickerCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            hour = e.HourOfDay;
            minute = e.Minute;
            UpdateDisplay();
            _TimePickDialogPopup = false;
        }

        protected override Dialog OnCreateDialog(int id)
        {
            if (id == TIME_DIALOG_ID)
            {
                return new TimePickerDialog(this, TimePickerCallback, hour, minute, true);
            }
            return null;
        }
    }
}

