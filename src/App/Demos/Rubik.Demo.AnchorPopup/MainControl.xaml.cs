using System.Windows;
using System.Windows.Controls;

namespace Rubik.Demo.AnchorPopup
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        public MainControl()
        {
            InitializeComponent();
        }

        private void ShowDefaultPopup_Click(object sender, RoutedEventArgs e)
        {
            Rubik.Theme.Primitives.AnchorPopup.Show(DefaultStyleButton, "This is a message");
        }

        private void ShowCustomPopup_Click(object sender, RoutedEventArgs e)
        {
            Rubik.Theme.Primitives.AnchorPopup.Show(CustomStyleButton, "This is a message", background: "#404040", foreground: "#FFFFFF");
        }

        private void ShowMorePopup_Click(object sender, RoutedEventArgs e)
        {
            Rubik.Theme.Primitives.AnchorPopup.Show(MoreStyleButton, new MessageControl(), background: "#404040", foreground: "#FFFFFF");
        }
    }
}