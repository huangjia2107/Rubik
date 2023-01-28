using System;
using System.Windows;
using System.Windows.Controls;

using Rubik.Theme.Datas;

namespace Rubik.Theme.Controls
{
    [TemplatePart(Name = YearBoxTemplateName, Type = typeof(NumericBox))]
    [TemplatePart(Name = MonthBoxTemplateName, Type = typeof(NumericBox))]
    [TemplatePart(Name = DayBoxTemplateName, Type = typeof(NumericBox))]
    [TemplatePart(Name = HourBoxTemplateName, Type = typeof(NumericBox))]
    [TemplatePart(Name = MinuteBoxTemplateName, Type = typeof(NumericBox))]
    [TemplatePart(Name = SecondBoxTemplateName, Type = typeof(NumericBox))]
    [TemplatePart(Name = CalendarTemplateName, Type = typeof(Calendar))]
    public class DateTimePicker : Control
    {
        private const string YearBoxTemplateName = "PART_YearBox";
        private const string MonthBoxTemplateName = "PART_MonthBox";
        private const string DayBoxTemplateName = "PART_DayBox";
        private const string HourBoxTemplateName = "PART_HourBox";
        private const string MinuteBoxTemplateName = "PART_MinuteBox";
        private const string SecondBoxTemplateName = "PART_SecondBox";
        private const string CalendarTemplateName = "PART_Calendar";

        private NumericBox _yearBox;
        private NumericBox _monthBox;
        private NumericBox _dayBox;
        private NumericBox _hourBox;
        private NumericBox _minuteBox;
        private NumericBox _secondBox;
        private Calendar _calendar;

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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_yearBox != null)
                _yearBox.ValueChanged -= _yearBox_ValueChanged;

            if (_monthBox != null)
                _monthBox.ValueChanged -= _monthBox_ValueChanged;

            if (_dayBox != null)
                _dayBox.ValueChanged -= _dayBox_ValueChanged;

            if (_hourBox != null)
                _hourBox.ValueChanged -= _hourBox_ValueChanged;

            if (_minuteBox != null)
                _minuteBox.ValueChanged -= _minuteBox_ValueChanged;

            if (_secondBox != null)
                _secondBox.ValueChanged -= _secondBox_ValueChanged;

            if (_calendar != null)
                _calendar.SelectedDatesChanged -= _calendar_SelectedDatesChanged;

            _yearBox = GetTemplateChild(YearBoxTemplateName) as NumericBox;
            _monthBox = GetTemplateChild(MonthBoxTemplateName) as NumericBox;
            _dayBox = GetTemplateChild(DayBoxTemplateName) as NumericBox;
            _hourBox = GetTemplateChild(HourBoxTemplateName) as NumericBox;
            _minuteBox = GetTemplateChild(MinuteBoxTemplateName) as NumericBox;
            _secondBox = GetTemplateChild(SecondBoxTemplateName) as NumericBox;
            _calendar = GetTemplateChild(CalendarTemplateName) as Calendar;

            if (_yearBox != null)
                _yearBox.ValueChanged += _yearBox_ValueChanged;

            if (_monthBox != null)
                _monthBox.ValueChanged += _monthBox_ValueChanged;

            if (_dayBox != null)
                _dayBox.ValueChanged += _dayBox_ValueChanged;

            if (_hourBox != null)
                _hourBox.ValueChanged += _hourBox_ValueChanged;

            if (_minuteBox != null)
                _minuteBox.ValueChanged += _minuteBox_ValueChanged;

            if (_secondBox != null)
                _secondBox.ValueChanged += _secondBox_ValueChanged;

            if (_calendar != null)
                _calendar.SelectedDatesChanged += _calendar_SelectedDatesChanged;

            UpdateDateTime();
        }

        private void _calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_calendar.SelectedDate.HasValue)
            {
                if (_yearBox != null)
                    _yearBox.Value = _calendar.SelectedDate.Value.Year;

                if (_monthBox != null)
                    _monthBox.Value = _calendar.SelectedDate.Value.Month;

                if (_dayBox != null)
                    _dayBox.Value = _calendar.SelectedDate.Value.Day;

                Value = new DateTime(_calendar.SelectedDate.Value.Year, _calendar.SelectedDate.Value.Month, _calendar.SelectedDate.Value.Day, Value.Hour, Value.Minute, Value.Second);
            }
        }

        private void _yearBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var args = e as TextBoxValueChangedEventArgs<double>;

            if (!args.IsManual)
                return;

            var year = (int)args.NewValue;

            if (Value.Year != year)
                Value = new DateTime(year, Value.Month, Math.Min(DateTime.DaysInMonth(year, Value.Month), Value.Day), Value.Hour, Value.Minute, Value.Second);
        }

        private void _monthBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var args = e as TextBoxValueChangedEventArgs<double>;

            if (!args.IsManual)
                return;

            var month = (int)args.NewValue;

            if (Value.Month != month)
                Value = new DateTime(Value.Year, month, Math.Min(DateTime.DaysInMonth(Value.Year, month), Value.Day), Value.Hour, Value.Minute, Value.Second);
        }

        private void _dayBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var args = e as TextBoxValueChangedEventArgs<double>;

            if (!args.IsManual)
                return;

            var day = (int)args.NewValue;

            if (Value.Day != day)
                Value = new DateTime(Value.Year, Value.Month, day, Value.Hour, Value.Minute, Value.Second);
        }

        private void _hourBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var args = e as TextBoxValueChangedEventArgs<double>;

            if (!args.IsManual)
                return;

            var hour = (int)args.NewValue;

            if (Value.Hour != hour)
                Value = new DateTime(Value.Year, Value.Month, Value.Day, hour, Value.Minute, Value.Second);
        }

        private void _minuteBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var args = e as TextBoxValueChangedEventArgs<double>;

            if (!args.IsManual)
                return;

            var minute = (int)args.NewValue;

            if (Value.Minute != minute)
                Value = new DateTime(Value.Year, Value.Month, Value.Day, Value.Hour, minute, Value.Second);
        }

        private void _secondBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var args = e as TextBoxValueChangedEventArgs<double>;

            if (!args.IsManual)
                return;

            var second = (int)args.NewValue;

            if (Value.Second != second)
                Value = new DateTime(Value.Year, Value.Month, Value.Day, Value.Hour, Value.Minute, second);
        }

        private void UpdateDateTime()
        {
            if (_calendar != null)
                _calendar.SelectedDate = Value;

            if (_hourBox != null)
                _hourBox.Value = Value.Hour;

            if (_minuteBox != null)
                _minuteBox.Value = Value.Minute;

            if (_secondBox != null)
                _secondBox.Value = Value.Second;
        }
    }
}