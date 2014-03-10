using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace NetCheatMemoryViewer
{
    public partial class OptionsForm : Form
    {
        private Color _foreColor = Color.Black;
        private Color _backColor = Color.White;
        public Color foreColor
        {
            get { return _foreColor; }
            set
            {
                _foreColor = value;

                ForeColor = _foreColor;
                foreach (Control c in Controls)
                {
                    c.ForeColor = _foreColor;
                }
            }

        }

        public Color backColor
        {
            get { return _backColor; }
            set
            {
                _backColor = value;

                BackColor = _backColor;
                foreach (Control c in Controls)
                {
                    c.BackColor = _backColor;
                }
            }

        }

        public OptionsForm()
        {
            InitializeComponent();
        }

        public static void SaveOptions()
        {
            string[] lines = { ctlMain.relBCNDBool.ToString(), ctlMain.startAddrStr, String.Join(";", ctlMain.oldAddrs.ToArray()) };
            File.WriteAllLines(ctlMain.optionsPath, lines);
        }

        private void optSaveButt_Click(object sender, EventArgs e)
        {
            ctlMain.startAddrStr = startAddr.Text;
            ctlMain.relBCNDBool = relBCNDCheckb.Checked;

            SaveOptions();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            startAddr.Text = ctlMain.startAddrStr;
            relBCNDCheckb.Checked = ctlMain.relBCNDBool;
        }

        private void optExitButt_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
    }
}
