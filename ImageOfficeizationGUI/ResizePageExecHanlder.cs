using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageOfficeizationGUI
{
    public partial class Main
    {
        /// <summary>
        /// 初始化图片缩放面板默认值
        /// </summary>
        private void InitResizePageDefault()
        {
            this.textBox7.TextChanged += ResizeHWValueTextChanged;
            this.textBox12.TextChanged += ResizeHWValueTextChanged;
        }

        /// <summary>
        /// 获取图片的宽高像素并填入输入框
        /// </summary>
        private void InputCurrentImgSizeData()
        {
            // 单个图片，PATHS数量一定是1
            string currentImgPath = PATHS[0];
            Bitmap bitmap = CommonRef.GetImgWH(PATHS[0]);
            this.textBox7.Text = bitmap.Width.ToString();
            this.textBox12.Text = bitmap.Height.ToString();
        }



        /// <summary>
        /// 图片缩放 约束比例自动计算宽高
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ResizeHWValueTextChanged(object? sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                return;
            }
            
            // todo 约束比例应该
            if (sender!=null)
            {
                TextBox sourceCtr =  (TextBox)sender;
                if (String.IsNullOrWhiteSpace(sourceCtr.Text))
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 点击了约束比例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                return;
            }
            bool v1 = PreRunCommonCheck();
            if (!v1)
            {
                checkBox1.Checked = false;
                return;
            }
            if (PATHS.Count>1)
            {
                MessageBox.Show("约束比例：图片源只能是单张图片，而不能是文件夹。", "提示", MessageBoxButtons.OK);
                checkBox1.Checked = false;
                return;
            }
        }

        public string? ResizekPageArgsDeal()
        {
            var resizeInputParams = new
            {
                WH = new { X = Convert.ToInt32(textBox7.Text), Y = Convert.ToInt32(textBox12.Text) },
                // 原图片绝对路径(目录和单一图片)
                Paths = PATHS,
                // 输出目录
                OutDir = OUTDIR,
            };
            return CommonRef.JSON_ENCODE(resizeInputParams);
        }
    }
}
