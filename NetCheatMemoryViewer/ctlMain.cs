using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PluginInterface;
using CWPS3Portable;

namespace NetCheatMemoryViewer
{


    public partial class ctlMain : UserControl
    {
        public static IPluginHost NCInterface;

        static bool _relBCNDBool = false;
        public static bool relBCNDBool
        {
            get { return _relBCNDBool; }
            set
            {
                _relBCNDBool = value;
                CodeWizard.isBranchCompareRelative = _relBCNDBool;
            }
        }
        public static string startAddrStr = "00010000";
        public static string optionsPath = ".ncMemViewer.ini";
        public static List<string> oldAddrs = new List<string>();

        ulong curAddr = 0x00010000;
        int rowCount = 0x20;
        int bytesPerColumn = 4;
        byte[] dispBytes;

        int oldASMSelIndex = -1;

        static CW CodeWizard = new CW();
        Timer timer = new Timer();

        public ctlMain()
        {
            InitializeComponent();

            rowCount = asmBox.Height / asmBox.ItemHeight;

            bytesBox.KeyDown += new KeyEventHandler(rtbKeyDown);
            bytesBox.TextChanged += new EventHandler(rtbTextChanged);
            bytesBox.MouseUp += new MouseEventHandler(rtbMouseUp);
            bytesBox.MouseDown += new MouseEventHandler(rtbOnMouseDown);
            bytesBox.KeyPress += new KeyPressEventHandler(rtbKeyPress);

            textBox.KeyDown += new KeyEventHandler(rtbKeyDown);
            textBox.TextChanged += new EventHandler(rtbTextChanged);
            textBox.MouseUp += new MouseEventHandler(rtbMouseUp);
            textBox.MouseDown += new MouseEventHandler(rtbOnMouseDown);
            textBox.KeyPress += new KeyPressEventHandler(rtbKeyPress);

            asmBox.LostFocus += new EventHandler(listBoxLoseFocus);
            asmBox.KeyDown += new KeyEventHandler(listBoxKeyDown);

            timer.Tick += new EventHandler(AutoRefreshTick);
            timer.Interval = 500;
            timer.Start();

            CodeWizard.DeclareInstructions();
            CodeWizard.DeclareHelpStr();

            /* Add psuedo ops */
            CodeWizard.customPseudos = new CW.pInstruction[3];
            CodeWizard.customPseudos[0].asm = @"ori %rD, %rA, 0";
            CodeWizard.customPseudos[0].format = @"%rD, %rA";
            CodeWizard.customPseudos[0].name = "mr";
            CodeWizard.customPseudos[0].regs = new string[] { @"%rD", @"%rA" };

            CodeWizard.customPseudos[1].asm = @"hexcode 0x4E800420";
            CodeWizard.customPseudos[1].format = @"";
            CodeWizard.customPseudos[1].name = "bctr";
            CodeWizard.customPseudos[1].regs = new string[0];
            
            CodeWizard.customPseudos[2].asm = @"hexcode 0x4E800421";
            CodeWizard.customPseudos[2].format = @"";
            CodeWizard.customPseudos[2].name = "bctrl";
            CodeWizard.customPseudos[2].regs = new string[0];

            CodeWizard.customPseudos[2].asm = @"hexcode 0x44000002";
            CodeWizard.customPseudos[2].format = @"";
            CodeWizard.customPseudos[2].name = "sc";
            CodeWizard.customPseudos[2].regs = new string[0];
        }

        private void ctlMain_Load(object sender, EventArgs e)
        {
            optionsPath = Path.Combine(Application.StartupPath, optionsPath);
            LoadOptions();
            UpdateAddrCB();

            curAddr = Convert.ToUInt64(startAddrStr, 16);

            dispBytes = NCInterface.GetMemory(curAddr, 4 * rowCount);

            UpdateRTBs(true, true, true, true, false);

            watchList.BackgroundColor = BackColor;
            watchList.ForeColor = ForeColor;
            watchList.GridColor = BackColor;
            DataGridViewCellStyle dgvcs = new DataGridViewCellStyle(watchList.RowsDefaultCellStyle);
            dgvcs.BackColor = BackColor;
            dgvcs.ForeColor = ForeColor;
            watchList.RowsDefaultCellStyle = dgvcs;
        }

        void UpdateAddrCB()
        {
            if (oldAddrs.Count > 10)
            {
                List<string> newAddrs = new List<string>();
                for (int x = (oldAddrs.Count - 1); x >= (oldAddrs.Count - 10); x--)
                {
                    newAddrs.Add(oldAddrs[x]);
                }
                oldAddrs = newAddrs;
            }

            gotoAddrCB.Items.Clear();
            for (int x = (oldAddrs.Count - 1); x >= 0; x--)
            {
                gotoAddrCB.Items.Add(oldAddrs[x]);
            }
        }

        void AddToAddrCB(string val)
        {
            oldAddrs.Remove(val);
            oldAddrs.Add(val);
            UpdateAddrCB();
            gotoAddrCB.SelectedItem = val;
        }

        void AutoRefreshTick(object sender, EventArgs e)
        {
            if (autoRefCB.Checked)
            {
                UpdateRTBs(true, true, true, true, true);
                UpdateWatchList();
            }
            else
                timer.Stop();
        }

        private void optionsButt_Click(object sender, EventArgs e)
        {
            OptionsForm opt = new OptionsForm();
            opt.foreColor = ForeColor;
            opt.backColor = BackColor;
            opt.Show();
        }

        #region Misc

        void LoadOptions()
        {
            if (!File.Exists(optionsPath))
                return;

            try
            {
                string[] lines = File.ReadAllLines(optionsPath);

                relBCNDBool = bool.Parse(lines[0]);
                startAddrStr = lines[1];
                string[] oldaddrs = lines[2].Split(';');
                foreach (string addr in oldaddrs)
                    oldAddrs.Add(addr);
            }
            catch
            {

            }
        }

        byte[] FloatToBA(float fltStr)
        {
            try
            {
                byte[] flt = BitConverter.GetBytes(fltStr);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(flt);
                return flt;
            }
            catch { }
            return new byte[0];
        }

        Single HexToFloat(byte[] hexBA)
        {
            string hex = "";
            foreach (byte b in hexBA)
                hex += b.ToString("X2");
            try
            {
                return BitConverter.ToSingle((BitConverter.GetBytes(int.Parse(hex, System.Globalization.NumberStyles.HexNumber))), 0);
            }
            catch { }
            return 0;
        }

        void UpdateRTBs(bool updateAddr, bool updateBytes, bool updateText, bool updateASM, bool color)
        {
            byte[] displayedBytes = NCInterface.GetMemory(curAddr, 4 * rowCount);

            int[] oldStarts = { addrBox.SelectionStart, bytesBox.SelectionStart, textBox.SelectionStart };
            int[] oldLens = { addrBox.SelectionLength, bytesBox.SelectionLength, textBox.SelectionLength };
            bool[] isFocused = { addrBox.Focused, bytesBox.Focused, textBox.Focused };

            if (updateAddr)
            {
                addrBox.Enabled = false;

                string txt = "";
                for (int x = 0; x < rowCount; x++)
                    txt += (curAddr + (ulong)(x * bytesPerColumn)).ToString("X8") + "\n";
                addrBox.Text = txt.Trim('\n');
                addrBox.SelectAll();
                addrBox.SelectionAlignment = HorizontalAlignment.Center;
                addrBox.SelectionLength = 1;

                addrBox.Enabled = true;
                if (isFocused[0])
                    addrBox.Focus();
                addrBox.SelectionStart = oldStarts[0];
                addrBox.SelectionLength = oldLens[0];
            }
            if (updateBytes)
            {
                bytesBox.Enabled = false;

                string oldStr = bytesBox.Text;
                bytesBox.Text = FormatByteArray(displayedBytes); 
                bytesBox.SelectAll();
                bytesBox.SelectionAlignment = HorizontalAlignment.Center;
                bytesBox.SelectionColor = ForeColor;

                //Color changed values
                if (color)
                {
                    byte max = 255;
                    Color changed = Color.FromArgb(max - ForeColor.R, max - ForeColor.G, max - ForeColor.B);
                    invForeColor = changed;
                    for (int cnt = 0; cnt < dispBytes.Length; cnt++)
                    {
                        if (dispBytes[cnt] != displayedBytes[cnt])
                        {
                            int off = cnt * 2;
                            off += off / (bytesPerColumn * 2);
                            //if (bytesBox.Text[off] == '\n')
                            //    off++;
                            //else if (bytesBox.Text[off + 1] == '\n')
                            //    off += 2;
                            bytesBox.SelectionStart = off;
                            bytesBox.SelectionLength = 2;
                            bytesBox.SelectionColor = changed;
                            //bytesBox.SelectionColor = Color.Green;
                        }
                    }
                }

                bytesBox.SelectionLength = 1;
                bytesBox.Enabled = true;
                if (isFocused[1])
                    bytesBox.Focus();
                bytesBox.SelectionStart = oldStarts[1];
                bytesBox.SelectionLength = oldLens[1];
                dispBytes = displayedBytes;
            }
            if (updateText)
            {
                textBox.Enabled = false;

                textBox.Text = FormatText(displayedBytes);
                textBox.SelectAll();
                textBox.SelectionAlignment = HorizontalAlignment.Center;
                textBox.SelectionLength = 1;

                textBox.Enabled = true;
                if (isFocused[2])
                    textBox.Focus();
                textBox.SelectionStart = oldStarts[2];
                textBox.SelectionLength = oldLens[2];
            }
            if (updateASM)
            {
                while (asmBox.Items.Count < rowCount)
                    asmBox.Items.Add("illegal");

                byte[] mem = new byte[4];
                for (int x = 0; x < rowCount; x++)
                {
                    ulong newAddr = curAddr + (ulong)(x * bytesPerColumn);
                    Array.Copy(displayedBytes, x * bytesPerColumn, mem, 0, 4);
                    string ncCode = "0 " + newAddr.ToString("X8") + " " + BitConverter.ToString(mem).Replace("-", "") + "\n";
                    CodeWizard.ASMDisMode = 1; //Disables labels
                    string[] disCodes = CodeWizard.ASMDisassemble(ncCode).Replace("hexcode 0x00000000", "illegal").Split('\n');
                    string disCode = ParseCode(disCodes[disCodes.Length - 2]);
                    asmBox.Items[x] = disCode;
                }
            }

            RefreshAll();
        }

        /*
         * Parses out any pseudo ops (hard-coded)
         */
        string ParseCode(string code)
        {
            if (code == null || code == "" || code == "nop" || code == "illegal")
                return code;

            string[] words = code.Split(' ');
            if (words[0] == "ori" && words[3] == "0x0000")
            {
                return "mr " + words[1] + " " + words[2].Replace(",", "");
            }
            else if (code == "hexcode 0x4E800420")
                return "bctr";
            else if (code == "hexcode 0x4E800421")
                return "bctrl";
            else if (code == "hexcode 0x44000002")
                return "sc";

            return code;
        }

        /* Brings up the Input Box with the arguments of a */
        public InputBox.IBArg[] CallIBox(InputBox.IBArg[] a)
        {
            InputBox ib = new InputBox();

            ib.Arg = a;
            ib.fmHeight = this.Height;
            ib.fmWidth = this.Width;
            ib.fmLeft = this.Left;
            ib.fmTop = this.Top;
            ib.TopMost = true;
            ib.fmForeColor = ForeColor;
            ib.fmBackColor = BackColor;
            ib.ShowDialog();

            a = ib.Arg;

            if (ib.ret == 1)
                return a;
            else if (ib.ret == 2)
                return null;

            return null;
        }

        string FormatText(byte[] ba)
        {
            string ret = "";

            for (int y = 0; y < ba.Length; y += bytesPerColumn)
            {
                for (int x = 0; x < bytesPerColumn; x++)
                {
                    char chr = (char)ba[x + y];
                    if (chr < 0x20 || chr > 0x80)
                        chr = '.';
                    ret += chr.ToString();
                }
                ret += "\n";
            }

            return ret.Trim('\n'); 
        }

        string FormatByteArray(byte[] ba)
        {
            string ret = "";

            for (int y = 0; y < ba.Length; y += bytesPerColumn)
            {
                for (int x = 0; x < bytesPerColumn; x++)
                    ret += ba[x + y].ToString("X2");
                ret += "\n";
            }

            return ret.Trim('\n').ToUpper();
        }

        void CalculateChange(bool isText, char newChar, RichTextBox rtb)
        {
            int off = rtb.SelectionStart;
            bool isFocused = rtb.Focused;

            //Apply changes to window
            if (!isText && newChar > (int)'F')
                newChar -= (char)0x20;
            char[] chr = rtb.Text.ToCharArray();
            chr[off] = newChar;
            string text = "";
            foreach (char c in chr)
                text += c.ToString();

            rtb.Enabled = false;
            rtb.Text = text;
            rtb.SelectAll();
            rtb.SelectionAlignment = HorizontalAlignment.Center;
            rtb.SelectionLength = 1;
            rtb.Enabled = true;
            if (isFocused)
                rtb.Focus();
            rtb.SelectionStart = off;

            //Apply changes to memory
            if (!isText)
            {
                int offLine = off - (off / (bytesPerColumn * 2 + 1));
                string byteStr = "";
                if ((offLine % 2) == 1) //End of byte
                    byteStr = chr[off - 1].ToString() + chr[off].ToString();
                else
                    byteStr = chr[off].ToString() + chr[off + 1].ToString();

                ulong addr = curAddr + (ulong)(offLine / 2);
                NCInterface.SetMemory(addr, StringBAToBA(byteStr));
            }
            else
            {
                int byteOff = off - (off / (bytesPerColumn + 1));
                ulong addr = curAddr + (ulong)byteOff;
                NCInterface.SetMemory(addr, new byte[] { (byte)newChar });
            }

            UpdateRTBs(false, isText, !isText, true, true);
        }

        /*
         * Converts a string byte array to byte array representing its hexadecimal form
         * "01020304" = { 0x01, 0x02, 0x03, 0x04 }
         */
        public static byte[] StringBAToBA(string str)
        {
            if (str == null || (str.Length % 2) == 1)
                return new byte[0];

            byte[] ret = new byte[str.Length / 2];
            for (int x = 0; x < str.Length; x += 2)
                ret[x / 2] = byte.Parse(NCInterface.sMid(str, x, 2), System.Globalization.NumberStyles.HexNumber);

            return ret;
        }

        #endregion

        #region Handlers

        private void copyRaw_Click(object sender, EventArgs e)
        {
            string res = "";
            if (addrBox.SelectionLength > 1)
            {
                res = NCInterface.sMid(addrBox.Text, addrBox.SelectionStart, addrBox.SelectionLength);
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, res));
            }
            else if (bytesBox.SelectionLength > 1)
            {
                res = NCInterface.sMid(bytesBox.Text, bytesBox.SelectionStart, bytesBox.SelectionLength);
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, res));
            }
            else if (textBox.SelectionLength > 1)
            {
                res = NCInterface.sMid(textBox.Text, textBox.SelectionStart, textBox.SelectionLength);
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, res));
            }
            else if (asmBox.Focused)
            {
                res = asmBox.Items[asmBox.SelectedIndex].ToString();
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, res));
            }
        }

        void listBoxLoseFocus(object sender, EventArgs e)
        {
            ListBox lb = sender as ListBox;
            oldASMSelIndex = lb.SelectedIndex;
            lb.SelectedIndex = -1;
        }

        void rtbMouseUp(object sender, MouseEventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;

            if (rtb.SelectionLength < 1)
                rtb.SelectionLength = 1;
        }

        void HandleRTBMove(RichTextBox rtb, int off)
        {
            if ((rtb.SelectionStart + off) == rtb.Text.Length)
                rtb.SelectionStart--;
            if (rtb.Text[(rtb.SelectionStart + off)] == '\n' && (rtb.SelectionStart + off) == (rtb.Text.Length - 1))
                rtb.SelectionStart--;
            if (rtb.Text[(rtb.SelectionStart + off)] == '\n' && (rtb.SelectionStart + off) < (rtb.Text.Length - 1))
                rtb.SelectionStart++;
        }

        void CopySelected()
        {
            string res = "";
            if (addrBox.Focused)
            {
                res = NCInterface.sMid(addrBox.Text, addrBox.SelectionStart, addrBox.SelectionLength);
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, res));
            }
            else if (bytesBox.Focused)
            {
                int off = bytesBox.SelectionStart, len = bytesBox.SelectionLength;
                int byteOff = off - (off / (bytesPerColumn * 2));
                if ((byteOff % 2) != 0)
                {
                    off--;
                    len++;
                }
                if ((len / (bytesPerColumn * 2)) % 2 == 0)
                    len++;

                byteOff = off - (off / (bytesPerColumn * 2));
                ulong addr = curAddr + (ulong)(byteOff / 2);

                res = "0 " + addr.ToString("X8") + " " + NCInterface.sMid(bytesBox.Text, off, len).Replace("\n", "");
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, res));
            }
            else if (textBox.Focused)
            {
                int byteOff = textBox.SelectionStart - (textBox.SelectionStart / (bytesPerColumn + 1));
                ulong addr = curAddr + (ulong)(byteOff);
                res = "1 " + addr.ToString("X8") + " " + NCInterface.sMid(textBox.Text, textBox.SelectionStart, textBox.SelectionLength).Replace("\n", "");

                Clipboard.SetDataObject(new DataObject(DataFormats.Text, res));
            }
            else if (asmBox.Focused)
            {
                res = asmBox.Items[asmBox.SelectedIndex].ToString();
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, res));
            }
        }

        void rtbOnMouseDown(object sender, MouseEventArgs e)
        {
            (sender as RichTextBox).AutoWordSelection = true;
            (sender as RichTextBox).AutoWordSelection = false;
        }

        void rtbKeyDown(object sender, KeyEventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            HandleRTBMove(rtb, 0);
            e.Handled = true;

            int colSize = bytesPerColumn;
            bool isText = rtb.Tag.ToString() == "TEXT";
            if (isText)
                colSize /= 2;

            if (e.KeyCode == Keys.C && e.Control)
                CopySelected();
            if (e.Control)
                return;

            char newChar = char.ConvertFromUtf32((int)e.KeyCode)[0];
            if (newChar >= 0x61 && newChar <= (int)'z' && !e.Shift)
                newChar -= (char)0x20;
            if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9 || ((e.KeyCode >= Keys.A && e.KeyCode <= Keys.F)) && !isText)
            {
                CalculateChange(isText, newChar, rtb);

                if (rtb.SelectionStart == (rtb.Text.Length - 1))
                {
                    curAddr += 4;
                    rtb.SelectionStart -= bytesPerColumn * 2 + 1;
                    UpdateRTBs(true, true, true, true, true);
                }
                rtb.SelectionStart++;
                HandleRTBMove(rtb, 0);


                return;
            }

            if (e.KeyCode == Keys.Left)
            {
                if (rtb.SelectionStart > 0)
                {
                    rtb.SelectionStart--;
                    if (rtb.Text[rtb.SelectionStart] == '\n')
                        rtb.SelectionStart--;
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (rtb.SelectionStart < (rtb.Text.Length - 1))
                {
                    rtb.SelectionStart++;
                    if (rtb.Text[rtb.SelectionStart] == '\n')
                        rtb.SelectionStart++;
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (rtb.SelectionStart > (colSize * 2))
                {
                    rtb.SelectionStart -= (colSize * 2 + 1);
                }
                else
                {
                    curAddr -= (ulong)bytesPerColumn;
                    UpdateRTBs(true, true, true, true, false);
                    Application.DoEvents();
                    //rtb.SelectionStart -= (colSize * 2 + 1);
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (rtb.SelectionStart < (rtb.Text.Length - (colSize * 2 + 1)))
                {
                    rtb.SelectionStart += (colSize * 2 + 1);
                }
                else
                {
                    curAddr += (ulong)bytesPerColumn;
                    UpdateRTBs(true, true, true, true, false);
                    Application.DoEvents();
                }
            }
            else if (isText && (newChar > 0x20 && newChar < 0x80))
            {
                CalculateChange(isText, newChar, rtb);

                if (rtb.SelectionStart == (rtb.Text.Length - 1))
                {
                    curAddr += 4;
                    rtb.SelectionStart -= bytesPerColumn + 1;
                    UpdateRTBs(true, true, true, true, true);
                }
                rtb.SelectionStart++;

                HandleRTBMove(rtb, 0);
            }

            (sender as RichTextBox).SelectionLength = 1;
        }

        void rtbTextChanged(object sender, EventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            HandleRTBMove(rtb, 0);
            rtb.SelectionLength = 1;
            
        }

        void rtbKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        void listBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && asmBox.SelectedIndex == (rowCount - 1))
            {
                curAddr += (ulong)bytesPerColumn;
                UpdateRTBs(true, true, true, true, false);
            }
            else if (e.KeyCode == Keys.Up && asmBox.SelectedIndex == 0)
            {
                curAddr -= (ulong)bytesPerColumn;
                UpdateRTBs(true, true, true, true, false);
            }
            else if (e.KeyCode == Keys.C && e.Control)
                CopySelected();
        }

        void RefreshAll()
        {
            addrBox.Refresh();
            bytesBox.Refresh();
            textBox.Refresh();
            asmBox.Refresh();
            watchList.Refresh();
        }

        private void asmBox_DoubleClick(object sender, EventArgs e)
        {
            int selIndex = (sender as ListBox).SelectedIndex;
            oldASMSelIndex = selIndex;
            InputBox.IBArg[] ib = new InputBox.IBArg[1];
            ib[0].defStr = asmBox.Items[asmBox.SelectedIndex].ToString();
            ib[0].label = "Edit the Assembly:";
            ib = CallIBox(ib);

            if (ib != null && ib[0].retStr != null && ib[0].retStr != "")
            {
                ulong addr = curAddr + (ulong)(selIndex * bytesPerColumn);
                byte[][] ret = CodeWizard.ASMAssembleToByte("address 0x" + addr.ToString("X8") + "\r\n" + ib[0].retStr + "\r\n", "");
                if (CodeWizard.debugString != "")
                {
                    MessageBox.Show(CodeWizard.debugString);
                    return;
                }
                else if (ret == null || ret.Length == 0)
                {
                    MessageBox.Show("Unknown error occured during assembly!\n");
                    return;
                }

                NCInterface.SetMemory(addr, ret[0]);
                if (selIndex < 0)
                {
                    UpdateRTBs(false, true, true, true, true);
                }
                else
                {
                    asmBox.Items[selIndex] = ib[0].retStr;
                    UpdateRTBs(false, true, true, false, true);
                }
                
                
            }
        }

        private void gotoAddr_Click(object sender, EventArgs e)
        {
            curAddr = Convert.ToUInt64(gotoAddrCB.Text, 16);
            UpdateRTBs(true, true, true, true, false);
        }

        private void refreshButt_Click(object sender, EventArgs e)
        {
            UpdateRTBs(true, true, true, true, true);
            UpdateWatchList();
        }

        #endregion

        #region Watch

        Color invForeColor;
        void UpdateWatchList()
        {
            foreach (DataGridViewRow row in watchList.Rows)
            {
                //Make sure it isn't being edited
                //Would be annoying to have it update on you
                if (!row.Cells[2].IsInEditMode)
                {
                    ulong addr = Convert.ToUInt64(row.Cells[0].Value.ToString(), 16);
                    string oldStr = row.Cells[2].Value.ToString();
                    byte[] data;

                    switch (row.Cells[1].Value.ToString())
                    {
                        case "char":
                            data = NCInterface.GetMemory(addr, 1);
                            row.Cells[2].Value = BitConverter.ToString(data).Replace("-", "");
                            break;
                        case "short":
                            data = NCInterface.GetMemory(addr, 2);
                            row.Cells[2].Value = BitConverter.ToString(data).Replace("-", "");
                            break;
                        case "int":
                            data = NCInterface.GetMemory(addr, 4);
                            row.Cells[2].Value = BitConverter.ToString(data).Replace("-", "");
                            break;
                        case "long":
                            data = NCInterface.GetMemory(addr, 8);
                            row.Cells[2].Value = BitConverter.ToString(data).Replace("-", "");
                            break;
                        case "string":
                            byte b;
                            string res = "";
                            while ((b = NCInterface.GetMemory(addr, 1)[0]) != 0)
                            {
                                if (res.Length > 255)
                                    break;
                                res += ((char)b).ToString();
                                addr++;
                            }
                            row.Cells[2].Value = res;
                            break;
                        case "float":
                            data = NCInterface.GetMemory(addr, 4);
                            row.Cells[2].Value = HexToFloat(data).ToString("0.000000");
                            break;
                    }

                    if (oldStr != row.Cells[2].Value.ToString())
                        row.DefaultCellStyle.ForeColor = invForeColor;
                    else
                        row.DefaultCellStyle.ForeColor = ForeColor;
                }
            }
        }

        private void watchAdd_Click(object sender, EventArgs e)
        {
            //DataGridViewComboBoxCell colType = new DataGridViewComboBoxCell();
            //colType.Value = watchList.Columns[1].
            watchList.Rows.Add("00000000", "int", "00000000");
        }

        private void watchRem_Click(object sender, EventArgs e)
        {
            if (watchList.SelectedCells.Count <= 0)
                return;

            foreach (DataGridViewCell row in watchList.SelectedCells)
            {
                watchList.Rows.Remove(row.OwningRow);
            }
        }

        private void watchLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "NetCheat Watch List files (*.ncwl)|*.ncwl|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                string[] res = File.ReadAllLines(fd.FileName);
                foreach (string line in res)
                {
                    if (line != "")
                    {
                        string[] split = line.Split(';');
                        watchList.Rows.Add(split[0], split[1], "");
                    }
                }
                UpdateWatchList();
            }
        }

        private void watchSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "NetCheat Watch List files (*.ncwl)|*.ncwl|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                string res = "";
                foreach (DataGridViewRow row in watchList.Rows)
                    res += row.Cells[0].Value.ToString() + ";" + row.Cells[1].Value.ToString() + Environment.NewLine;
                File.WriteAllText(fd.FileName, res);
            }
        }

        private void watchList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView a = (sender as DataGridView);

            if (e.ColumnIndex == 2)
            {
                DataGridViewRow row = a.Rows[e.RowIndex];
                byte[] data;
                ulong addr = Convert.ToUInt64(row.Cells[0].Value.ToString(), 16);

                switch (row.Cells[1].Value.ToString())
                {
                    case "char":
                        data = StringBAToBA(NCInterface.sLeft(a.Rows[e.RowIndex].Cells[2].Value.ToString(), 2));
                        NCInterface.SetMemory(addr, data);
                        break;
                    case "short":
                        data = StringBAToBA(NCInterface.sLeft(a.Rows[e.RowIndex].Cells[2].Value.ToString(), 4));
                        NCInterface.SetMemory(addr, data);
                        break;
                    case "int":
                        data = StringBAToBA(NCInterface.sLeft(a.Rows[e.RowIndex].Cells[2].Value.ToString(), 8));
                        NCInterface.SetMemory(addr, data);
                        break;
                    case "long":
                        data = StringBAToBA(NCInterface.sLeft(a.Rows[e.RowIndex].Cells[2].Value.ToString(), 16));
                        NCInterface.SetMemory(addr, data);
                        break;
                    case "string":
                        data = NCInterface.StringToByteA(a.Rows[e.RowIndex].Cells[2].Value.ToString());
                        Array.Resize(ref data, data.Length + 1);
                        NCInterface.SetMemory(addr, data);
                        break;
                    case "float":
                        data = FloatToBA(Single.Parse(a.Rows[e.RowIndex].Cells[2].Value.ToString()));
                        NCInterface.SetMemory(addr, data);
                        break;
                }
            }
        }

        #endregion

        private void autoRefCB_CheckedChanged(object sender, EventArgs e)
        {
            if (autoRefCB.Checked)
                timer.Start();
            else
                timer.Stop();

        }

        private void ctlMain_Resize(object sender, EventArgs e)
        {

        }

        private void gotoAddrCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ulong addr = Convert.ToUInt64(gotoAddrCB.SelectedItem.ToString(), 16);
            if (addr == curAddr)
                return;
            curAddr = addr;

            AddToAddrCB(gotoAddrCB.Text);
            OptionsForm.SaveOptions();
            UpdateRTBs(true, true, true, true, true);
        }

        private void gotoAddrCB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddToAddrCB(gotoAddrCB.Text);
                OptionsForm.SaveOptions();
                curAddr = Convert.ToUInt64(gotoAddrCB.Text, 16);
                UpdateRTBs(true, true, true, true, true);
            }
        }

    }
}
