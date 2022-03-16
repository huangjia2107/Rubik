using System;
using System.Windows;
using System.Windows.Controls;

namespace Rubik.Theme.Controls
{
    public class DateTimePicker : Control    
    {
        static DateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimePicker), new FrameworkPropertyMetadata(typeof(DateTimePicker)));
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(DateTimePicker), new PropertyMetadata(DateTime.Now, OnDatePropertyChanged));
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        static void OnDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as DateTimePicker;
            ctrl.Init();
        }

        public static readonly DependencyProperty YearProperty =
            DependencyProperty.Register("Year", typeof(int), typeof(DateTimePicker), new PropertyMetadata(DateTime.Now.Year, OnYearPropertyChanged));
        public int Year
        {
            get { return (int)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }

        static void OnYearPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as DateTimePicker;
            var year = (int)e.NewValue;

            if (ctrl.Date.Year != year)
                ctrl.Date = new DateTime(year, ctrl.Date.Month, Math.Min(DateTime.DaysInMonth(year, ctrl.Date.Month), ctrl.Date.Day), ctrl.Date.Hour, ctrl.Date.Minute, ctrl.Date.Second);
        }

        public static readonly DependencyProperty MonthProperty =
            DependencyProperty.Register("Month", typeof(int), typeof(DateTimePicker), new PropertyMetadata(DateTime.Now.Month, OnMonthPropertyChanged));
        public int Month
        {
            get { return (int)GetValue(MonthProperty); }
            set { SetValue(MonthProperty, value); }
        }

        static void OnMonthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as DateTimePicker;
            var month = (int)e.NewValue;

            if (ctrl.Date.Month != month)
                ctrl.Date = new DateTime(ctrl.Date.Year, month, Math.Min(DateTime.DaysInMonth(ctrl.Date.Year, month), ctrl.Date.Day) , ctrl.Date.Hour, ctrl.Date.Minute, ctrl.Date.Second);
        }

        public static readonly DependencyProperty DayProperty =
            DependencyProperty.Register("Day", typeof(int), typeof(DateTimePicker), new PropertyMetadata(DateTime.Now.Day, OnDayPropertyChanged));
        public int Day
        {
            get { return (int)GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

        static void OnDayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as DateTimePicker;
            var day = (int)e.NewValue;

            if (ctrl.Date.Day != day)
                ctrl.Date = new DateTime(ctrl.Date.Year, ctrl.Date.Month, day, ctrl.Date.Hour, ctrl.Date.Minute, ctrl.Date.Second);
        }

        public static readonly DependencyProperty HourProperty =
            DependencyProperty.Register("Hour", typeof(int), typeof(DateTimePicker), new PropertyMetadata(DateTime.Now.Hour, OnHourPropertyChanged));
        public int Hour
        {
            get { return (int)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }

        static void OnHourPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as DateTimePicker;
            var hour = (int)e.NewValue;

            if (ctrl.Date.Hour != hour)
                ctrl.Date = new DateTime(ctrl.Date.Year, ctrl.Date.Month, ctrl.Date.Day, hour, ctrl.Date.Minute, ctrl.Date.Second);
        }

        public static readonly DependencyProperty MinuteProperty =
            DependencyProperty.Register("Minute", typeof(int), typeof(DateTimePicker), new PropertyMetadata(DateTime.Now.Minute, OnMinutePropertyChanged));
        public int Minute
        {
            get { return (int)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); }
        }

        static void OnMinutePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as DateTimePicker;
            var minute = (int)e.NewValue;

            if (ctrl.Date.Minute != minute)
                ctrl.Date = new DateTime(ctrl.Date.Year, ctrl.Date.Month, ctrl.Date.Day, ctrl.Date.Hour, minute, ctrl.Date.Second);
        }

        public static readonly DependencyProperty SecondProperty =
            DependencyProperty.Register("Second", typeof(int), typeof(DateTimePicker), new PropertyMetadata(DateTime.Now.Second, OnSecondPropertyChanged));
        public int Second
        {
            get { return (int)GetValue(SecondProperty); }
            set { SetValue(SecondProperty, value); }
        }

        static void OnSecondPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as DateTimePicker;
            var second = (int)e.NewValue;

            if (ctrl.Date.Second != second)
                ctrl.Date = new DateTime(ctrl.Date.Year, ctrl.Date.Month, ctrl.Date.Day, ctrl.Date.Hour, ctrl.Date.Minute, second);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Date = DateTime.Now;

            Hour = Date.Hour;
            Minute = Date.Minute;
            Second = Date.Second;
        }

        private void Init()
        {
            Year = Date.Year;
            Month = Date.Month;
            Day = Date.Day;
        }
    }
}
