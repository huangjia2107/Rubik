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

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(DateTime), typeof(DateTimePicker), new PropertyMetadata(DateTime.Now, OnValuePropertyChanged));
        public DateTime Value
        {
            get { return (DateTime)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as DateTimePicker;
            ctrl.UpdateDateTime();
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
            ctrl.UpdateDate();
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

            if (ctrl.Value.Year != year)
                ctrl.Value = new DateTime(year, ctrl.Value.Month, Math.Min(DateTime.DaysInMonth(year, ctrl.Value.Month), ctrl.Value.Day), ctrl.Value.Hour, ctrl.Value.Minute, ctrl.Value.Second);
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

            if (ctrl.Value.Month != month)
                ctrl.Value = new DateTime(ctrl.Value.Year, month, Math.Min(DateTime.DaysInMonth(ctrl.Value.Year, month), ctrl.Value.Day), ctrl.Value.Hour, ctrl.Value.Minute, ctrl.Value.Second);
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

            if (ctrl.Value.Day != day)
                ctrl.Value = new DateTime(ctrl.Value.Year, ctrl.Value.Month, day, ctrl.Value.Hour, ctrl.Value.Minute, ctrl.Value.Second);
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

            if (ctrl.Value.Hour != hour)
                ctrl.Value = new DateTime(ctrl.Value.Year, ctrl.Value.Month, ctrl.Value.Day, hour, ctrl.Value.Minute, ctrl.Value.Second);
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

            if (ctrl.Value.Minute != minute)
                ctrl.Value = new DateTime(ctrl.Value.Year, ctrl.Value.Month, ctrl.Value.Day, ctrl.Value.Hour, minute, ctrl.Value.Second);
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

            if (ctrl.Value.Second != second)
                ctrl.Value = new DateTime(ctrl.Value.Year, ctrl.Value.Month, ctrl.Value.Day, ctrl.Value.Hour, ctrl.Value.Minute, second);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Value = DateTime.Now;
        }

        private void UpdateDateTime()
        {
            Date = Value;

            Hour = Value.Hour;
            Minute = Value.Minute;
            Second = Value.Second;
        }

        private void UpdateDate()
        {
            Year = Date.Year;
            Month = Date.Month;
            Day = Date.Day;
        }
    }
}
