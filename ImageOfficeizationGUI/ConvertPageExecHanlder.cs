
namespace ImageOfficeizationGUI
{
    public partial class Main
    {
        /// <summary>
        /// 初始化图片转换面板默认值
        /// </summary>
        private void InitConvertPageDefault()
        {
            // 目标转换格式列表
            comboBox4.DataSource = CommonRef.ImgFormatToConvertBindSource;
            comboBox4.DisplayMember = "Text";
            comboBox4.ValueMember = "Value";
            comboBox4.SelectedValue = 1;
        }

        public string? ConvertPageArgsDeal()
        {
            var convertInputParams = new
            {
                ImageFormatType = Convert.ToInt32(comboBox4.SelectedValue),
                // 原图片绝对路径(目录和单一图片)
                Paths = PATHS,
                // 输出目录
                OutDir = OUTDIR,
            };
            return CommonRef.JSON_ENCODE(convertInputParams);
        }
    }
}
