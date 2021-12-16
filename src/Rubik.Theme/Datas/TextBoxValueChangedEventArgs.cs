using System.Windows;

namespace Rubik.Theme.Datas
{
    public class TextBoxValueChangedEventArgs<T> : RoutedPropertyChangedEventArgs<T>
    {
        public bool IsManual { get; private set; }
        public bool IsBusy {get; private set; }

        public TextBoxValueChangedEventArgs(T oldValue, T newValue, bool isManual, bool isBusy)
            : base(oldValue, newValue)
        {
            IsManual = isManual;
            IsBusy = isBusy;
        }

        public TextBoxValueChangedEventArgs(T oldValue, T newValue, bool isManual, bool isBusy, RoutedEvent routedEvent)
            : base(oldValue, newValue, routedEvent)
        {
            IsManual = isManual;
            IsBusy = isBusy;
        }
    }
}
