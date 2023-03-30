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
        /// 递归主窗口每个子控件，保存其尺寸，在后续拉伸窗体大小时自适应设置
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
                MessageBox.Show("请拖放单个文件夹或图片", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// 检测图片源是否存在、有效
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
                MessageBox.Show("无任何有效的图片文件！\n(jpg/png/gif..and so on)", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private (int, object?) CollectCmdArgs()
        {
            (int, object?) args;
            // 操作单元就是当前选中的TabPage index
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
        /// 操作栏运行按钮被点击 搜集当前操作单元的控件数据 执行go
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runBtn_Click(object sender, EventArgs e)
        {
            if (!PATHS.Any() || OUTDIR is null)
            {
                MessageBox.Show("图片源和保存目录请选定！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!Directory.Exists(OUTDIR))
            {
                MessageBox.Show("保存目录已不存在！", "中止", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (! CheckSrcPathLogic(dropCtr.Text))
            {
                return;
            }
 
            // 当前操作面板不是图片转换，不允许存在webp格式进行操作
            if (tabControl.SelectedIndex!=2)
            {
                foreach (var item in PATHS)
                {
                    if (item.ToUpper().EndsWith(".WEBP"))
                    {
                        MessageBox.Show($"图片源中存在WEBP格式，无法进行操作。\n" +
                            $"可以使用[图片转换]将其转为其它格式后再进行操作。\n" +
                            $"{item}", "中止", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //dropCtr.Text = "（拖放到此处）";
                        return;
                    }
                }
            }
            // 当前操作面板是图片转换，原格式和目标格式不允许一致
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
                        //MessageBox.Show($"图片源中此图片格式和目标格式一致，无需进行转换：\n" +
                        //    $"{item}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //return;
                    }
                }
                PATHS = existPathTmp;
                if (sameSb.Length>0)
                {
                    MessageBox.Show($"即将开始转换，但图片源中这些图片的格式和目标格式一致，将被忽略：\n" +
                        $"{sameSb}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (!PATHS.Any())
                {
                    MessageBox.Show("无任何图片能够转换，因为相同格式的皆被忽略。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            // 传入的运行参数：执行单元的值+" "+传递的json数据，如图片水印，执行单元的值为0
            string? jsonEscapeStrData = args.Item2?.ToString()?.Replace("\"", "\\\"");
            if (jsonEscapeStrData is null)
            {
                MessageBox.Show("程序引发错误，请重试。\n After Escape Json Data is null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show($"程序执行发生错误，请重启尝试。\nGo Exit Code：{goProcessObj.ExitCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 点击了选择保存目录的按钮，打开文件夹浏览对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void outDirCtr_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog outDialog = new();
            outDialog.Description = "保存到此目录：";

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
        /// 自适应窗体何其子控件的尺寸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Resize(object sender, EventArgs e)
        {
            // 最小化，无需自适应控件
            if (this.WindowState== FormWindowState.Minimized) 
            { 
                return;
            }
            // 还原
            if (this.WindowState == FormWindowState.Normal)
            {
                if (this.Width == PRE_RESIZE_WIDTH)
                {
                    // 若当前宽度等于上一次宽度，表示是从最小化状态还原，无需自适应控件
                    return;
                }
            }
            PRE_RESIZE_WIDTH = this.Width;
            var curWidth = this.Width;
            var curHeight = this.Height;
            // 计算当前尺寸比列
            var wp = curWidth / this.ORG_WIDTH_MAIN;
            var hp = curHeight / this.ORG_HIGH_MAIN;
            UpdateControlsSizeBind(this, wp, hp);
        }

        /// <summary>
        /// 递归主窗口每个子控件，更新其尺寸，自适应窗口大小比
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
                        // fontSize+1，避免最小化窗口时，字体大小自动变为0时引发异常
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