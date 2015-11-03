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
        public static string AlarmTime;

        private TextView _GoodMorningTime;
        private TextView _LeftTime;
        private int hour;
        private int minute;
        const int TIME_DIALOG_ID = 0;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            //現在時刻
            _GoodMorningTime = FindViewById<TextView>(Resource.Id.GoodMorningTime);
            _GoodMorningTime.Click += ((o, e) =>
            {
                ShowDialog(TIME_DIALOG_ID);
            });
            hour = DateTime.Now.Hour;
            minute = DateTime.Now.Minute;

            //残り時間
            _LeftTime = FindViewById<TextView>(Resource.Id.LeftTime);


            //お休みボタン
            var goodNightButton = FindViewById<Button>(Resource.Id.GoodNightButton);
            goodNightButton.Click += ((o, e) => {
                var intent = new Intent(this, typeof(GoodMorningActivity));
                StartActivity(intent);
            });

            UpdateDisplay();
        }

        private string GetLeftTime(string nowTime, string alarmTime)
        {
            var now = DateTime.Parse(nowTime);
            var alarm = DateTime.Parse(alarmTime);
            var span = alarm - now;
            var leftTime = "";
            if (span.Ticks > 0)
            {
                leftTime = span.ToString();
            }
            else
            {
                leftTime = "00:00:00";
            }
            return leftTime;
        }

        private void UpdateDisplay()
        {
            string time = string.Format("{0}:{1}", hour, minute.ToString().PadLeft(2, '0'));
            _GoodMorningTime.Text = time;
            AlarmTime = time;

            _LeftTime.Text = GetLeftTime(DateTime.Now.ToString("HH:mm"), AlarmTime);
        }

        private void TimePickerCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            hour = e.HourOfDay;
            minute = e.Minute;
            UpdateDisplay();
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

