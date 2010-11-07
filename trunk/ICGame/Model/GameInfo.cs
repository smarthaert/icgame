using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ICGame
{
    public class GameInfo
    {
        private Form info;
        private TextBox tb;
        public GameInfo()
        {
            int size = 300;
            info = new Form();
            info.Size = new System.Drawing.Size(size, size);
            info.StartPosition = FormStartPosition.Manual;
            info.Location = new System.Drawing.Point(SystemInformation.PrimaryMonitorMaximizedWindowSize.Width - info.Size.Width, 0);
            tb = new TextBox();
            tb.Multiline = true;
            tb.Location = new System.Drawing.Point(0, 0);
            tb.Size = new System.Drawing.Size(size, size);
            info.Controls.Add(tb);
            info.Show();
        }
        public void ShowInfo(string text)
        {
            tb.Text = text;
        }
    }
}
