using System.IO;
using SWF = System.Windows.Forms;

namespace Rubik.Toolkit.UI
{
    public static class DialogUtil
    {
        public static string ShowSaveFileDialog(string fileName, string filter)
        {
            var sfd = new SWF.SaveFileDialog { Filter = filter, FileName = Path.GetFileNameWithoutExtension(fileName) };
            if (sfd.ShowDialog() == SWF.DialogResult.OK)
                return sfd.FileName;

            return null;
        }

        public static string ShowOpenFileDialog(string filter)
        {
            var ofd = new SWF.OpenFileDialog { Filter = filter };
            if (ofd.ShowDialog() == SWF.DialogResult.OK)
                return ofd.FileName;

            return null;
        }

        public static string ShowFolderBrowserDialog()
        {
            var ofd = new SWF.FolderBrowserDialog();
            if (ofd.ShowDialog() == SWF.DialogResult.OK)
                return ofd.SelectedPath;

            return null;
        }
    }
}
