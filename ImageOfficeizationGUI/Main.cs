using ImageOfficeizationGUI.Model;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ImageOfficeizationGUI
{
    public partial class Main : Form
    {
        private readonly double ORG_WIDTH_MAIN;
        private readonly double ORG_HIGH_MAIN;
        private static double PRE_RESIZE_WIDTH;
        public Main()
        {
            InitializeComponent();
            InitPageExecDefaultInputValue();
            ORG_WIDTH_MAIN = this.Width;
            ORG_HIGH_MAIN = this.Height;
            PRE_RESIZE_WIDTH = this.Width;
            InitControlsSizeBind(this);
        }

        /// <summary>
        /// �ݹ�������ÿ���ӿؼ���������ߴ磬�ں������촰���Сʱ����Ӧ����
        /// </summary>
        /// <param name="control"></param>
        private void InitControlsSizeBind(Control control)
        {
            if (control.Controls.Count>0)
            {
                foreach (Control item in control.Controls)
                {
                    item.Tag = item.Width + "," + item.Height+","+item.Font.Size;
                    if (item.Controls.Count>0)
                    {
                        InitControlsSizeBind(item);
                    }
                }
            }
        }

        private void InitPageExecDefaultInputValue()
        {
            InitWatermarkPageDefault();
            InitResizePageDefault();
            InitConvertPageDefault();
            InitCompressPageDefault();
        }


        private static List<string> PATHS = new();
        private static string? OUTDIR;


        private void DragDropMethod(DragEventArgs e, Button dropCtrSource)
        {
            PATHS = new List<string>();
            if (e?.Data is null)
            {
                MessageBox.Show("DragDrop DragEventArgs e is cant get data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            object[] dropDataArray = (object[])e.Data.GetData(DataFormats.FileDrop);
            if (dropDataArray.Length > 1)
            {
                MessageBox.Show("���Ϸŵ����ļ��л�ͼƬ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return ;
            }
            string? imageSrcPath = dropDataArray[0]?.ToString();
            if (! CheckSrcPathLogic(imageSrcPath))
            {
                return;
            }

            dropCtrSource.Text = imageSrcPath;
        }

        /// <summary>
        /// ���ͼƬԴ�Ƿ���ڡ���Ч
        /// </summary>
        /// <param name="imageSrcPath"></param>
        /// <returns></returns>
        private bool CheckSrcPathLogic(string? imageSrcPath)
        {
            
            if (imageSrcPath == null)
            {
                MessageBox.Show("cant get image source path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (Directory.Exists(imageSrcPath))
            {
                FileInfo[] imgInfos = new DirectoryInfo(imageSrcPath).GetFiles();
                PATHS = imgInfos.Select(img => img.FullName).ToList();
            }
            else
            {
                PATHS.Add(imageSrcPath);
            }
            PATHS = PATHS.Where(src =>
            {
                if (src.LastIndexOf('.') != -1)
                {
                    string formatName = src.Substring(src.LastIndexOf('.') + 1);
                    return CommonRef.IMG_FORMAT_NAME.Contains(formatName.ToUpper());
                }
                return false;

            }).ToList();
            if (!PATHS.Any())
            {
                MessageBox.Show("���κ���Ч��ͼƬ�ļ���\n(jpg/png/gif..and so on)", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private (int, object?) CollectCmdArgs()
        {
            (int, object?) args;
            // ������Ԫ���ǵ�ǰѡ�е�TabPage index
            var execType = this.tabControl.SelectedIndex;
            switch (execType)
            {
                case 0:
                    args = (0, WatermarkPageArgsDeal());
                    break;
                case 1:
                    args = (1, ResizekPageArgsDeal());
                    break;
                case 2:
                    args = (2, ConvertPageArgsDeal());
                    break;
                case 3:
                    args = (3, CompressPageArgsDeal());
                    break;
                default:
                    args = (-1, null);    
                    break;
            }
            return args;
        }

        /// <summary>
        /// ���������а�ť����� �Ѽ���ǰ������Ԫ�Ŀؼ����� ִ��go
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runBtn_Click(object sender, EventArgs e)
        {
            if (!PATHS.Any() || OUTDIR is null)
            {
                MessageBox.Show("ͼƬԴ�ͱ���Ŀ¼��ѡ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!Directory.Exists(OUTDIR))
            {
                MessageBox.Show("����Ŀ¼�Ѳ����ڣ�", "��ֹ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (! CheckSrcPathLogic(dropCtr.Text))
            {
                return;
            }
 
            // ��ǰ������岻��ͼƬת�������������webp��ʽ���в���
            if (tabControl.SelectedIndex!=2)
            {
                foreach (var item in PATHS)
                {
                    if (item.ToUpper().EndsWith(".WEBP"))
                    {
                        MessageBox.Show($"ͼƬԴ�д���WEBP��ʽ���޷����в�����\n" +
                            $"����ʹ��[ͼƬת��]����תΪ������ʽ���ٽ��в�����\n" +
                            $"{item}", "��ֹ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //dropCtr.Text = "���Ϸŵ��˴���";
                        return;
                    }
                }
            }
            // ��ǰ���������ͼƬת����ԭ��ʽ��Ŀ���ʽ������һ��
            if (tabControl.SelectedIndex == 2)
            {
                int v = CommonRef.SelectValueParse(comboBox4.SelectedValue);
                StringBuilder sameSb = new StringBuilder();
                string[] existPathTmpArr = new string[PATHS.Count];
                PATHS.CopyTo(0, existPathTmpArr, 0, PATHS.Count);
                List<string> existPathTmp = existPathTmpArr.ToList();
                foreach (var item in PATHS)
                {
                    if (CommonRef.IMG_FORMAT_NAME[v] == item.Substring(item.LastIndexOf(".")+1).ToUpper())
                    {
                        sameSb.AppendLine(item);
                        existPathTmp.Remove(item);
                        //MessageBox.Show($"ͼƬԴ�д�ͼƬ��ʽ��Ŀ���ʽһ�£��������ת����\n" +
                        //    $"{item}", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //return;
                    }
                }
                PATHS = existPathTmp;
                if (sameSb.Length>0)
                {
                    MessageBox.Show($"������ʼת������ͼƬԴ����ЩͼƬ�ĸ�ʽ��Ŀ���ʽһ�£��������ԣ�\n" +
                        $"{sameSb}", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (!PATHS.Any())
                {
                    MessageBox.Show("���κ�ͼƬ�ܹ�ת������Ϊ��ͬ��ʽ�ĽԱ����ԡ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            var args = CollectCmdArgs().ToTuple();
            if (args.Item1==-1 || args.Item2 is null)
            {
                return;
            }
            Process process = new();
            var goProcessPath = Path.Combine(Application.StartupPath, "cmd", "image_officeization.exe");
            process.StartInfo.FileName = goProcessPath;
            // ��������в�����ִ�е�Ԫ��ֵ+" "+���ݵ�json���ݣ���ͼƬˮӡ��ִ�е�Ԫ��ֵΪ0
            string? jsonEscapeStrData = args.Item2?.ToString()?.Replace("\"", "\\\"");
            if (jsonEscapeStrData is null)
            {
                MessageBox.Show("�����������������ԡ�\n After Escape Json Data is null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            process.StartInfo.Arguments = args.Item1+" "+ jsonEscapeStrData;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.CreateNoWindow = true;
            process.EnableRaisingEvents = true;
            process.Exited += goProcess_Exited; 
            process.Start();
        }

        private void goProcess_Exited(object? sender, EventArgs e)
        {
            Process? goProcessObj = null;
            if (sender!=null)
            {
                goProcessObj = (Process)sender;
            }
            if (goProcessObj != null) {
                if (goProcessObj.ExitCode!=0)
                {
                    MessageBox.Show($"����ִ�з����������������ԡ�\nGo Exit Code��{goProcessObj.ExitCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// �����ѡ�񱣴�Ŀ¼�İ�ť�����ļ�������Ի���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void outDirCtr_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog outDialog = new();
            outDialog.Description = "���浽��Ŀ¼��";

            if (outDialog.ShowDialog() == DialogResult.OK)
            {
                outDirCtr.Text =  outDialog.SelectedPath;
                OUTDIR = outDialog.SelectedPath;
            }
        }


        private void DropCtr_DragDrop(object sender, DragEventArgs e)
        {
            DragDropMethod(e, this.dropCtr);
        }

        private void DropCtr_DragEnter(object sender, DragEventArgs e)
        {
            if (e?.Data is null)
            {
                MessageBox.Show("DragDrop DragEventArgs e is cant get data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
                return;
            }
            e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// ����Ӧ��������ӿؼ��ĳߴ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Resize(object sender, EventArgs e)
        {
            // ��С������������Ӧ�ؼ�
            if (this.WindowState== FormWindowState.Minimized) 
            { 
                return;
            }
            // ��ԭ
            if (this.WindowState == FormWindowState.Normal)
            {
                if (this.Width == PRE_RESIZE_WIDTH)
                {
                    // ����ǰ��ȵ�����һ�ο�ȣ���ʾ�Ǵ���С��״̬��ԭ����������Ӧ�ؼ�
                    return;
                }
            }
            PRE_RESIZE_WIDTH = this.Width;
            var curWidth = this.Width;
            var curHeight = this.Height;
            // ���㵱ǰ�ߴ����
            var wp = curWidth / this.ORG_WIDTH_MAIN;
            var hp = curHeight / this.ORG_HIGH_MAIN;
            UpdateControlsSizeBind(this, wp, hp);
        }

        /// <summary>
        /// �ݹ�������ÿ���ӿؼ���������ߴ磬����Ӧ���ڴ�С��
        /// </summary>
        /// <param name="control"></param>
        private void UpdateControlsSizeBind(Control control, double wp, double hp)
        {
            if (control.Controls.Count > 0)
            {
                foreach (Control item in control.Controls)
                {
                    Array? wh = item?.Tag?.ToString()?.Split(",") ?? null;
                    if (wh != null)
                    {
                        item.Width = (int)(Convert.ToDouble(wh.GetValue(0)) * wp);
                        item.Height = (int)(Convert.ToDouble(wh.GetValue(1)) * hp);
                        var curFontSize= (int)(Convert.ToDouble(wh.GetValue(2)) * hp);
                        // fontSize+1��������С������ʱ�������С�Զ���Ϊ0ʱ�����쳣
                        item.Font = new Font(item.Font.FontFamily, curFontSize+1, item.Font.Style, item.Font.Unit);
                        if (item.Controls.Count > 0)
                        {
                            UpdateControlsSizeBind(item, wp, hp);
                        }
                    }
                    
                }
            }
        }
    }
}