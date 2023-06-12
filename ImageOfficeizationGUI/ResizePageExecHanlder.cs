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
