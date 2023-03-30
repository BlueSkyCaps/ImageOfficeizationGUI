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
        /// 初始化图片压缩面板默认值
        /// </summary>
        private void InitCompressPageDefault()
        {
            // 默认选择图片压缩清晰度最高的一项
            comboBox5.DataSource = CommonRef.CompressSizeQuarityBindSource;
            comboBox5.DisplayMember = "Text";
            comboBox5.ValueMember = "Value";
            comboBox5.SelectedValue = 0;
            // 设置提示文本
            compressMsgToolTip.SetToolTip(this.label19, "压缩质量越高，图片保留清晰度的同时体积变小。压缩质量越低清晰度越低。");
        }


        public string? CompressPageArgsDeal()
        {
            var cmpressInputParams = new
            {
                ImgCompressQuality = Convert.ToInt32(comboBox5.SelectedValue),
                // 原图片绝对路径(目录和单一图片)
                Paths = PATHS,
                // 输出目录
                OutDir = OUTDIR,
            };
            return CommonRef.JSON_ENCODE(cmpressInputParams);
        }
    }
}
