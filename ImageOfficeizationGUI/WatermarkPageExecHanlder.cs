using ImageOfficeizationGUI.Model;
using System.Drawing.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ImageOfficeizationGUI
{
    public partial class Main
    {
        /// <summary>
        /// 初始化水印面板默认值
        /// </summary>
        private void InitWatermarkPageDefault()
        {
            // 锚点
            watermarkPageAnchorBox.DataSource = CommonRef.WatermarkAnchorBindSource;
            watermarkPageAnchorBox.DisplayMember = "Text";
            watermarkPageAnchorBox.ValueMember = "Value";
            watermarkPageAnchorBox.SelectedValue = 4;

            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "0";
            textBox4.Text = "0";
            // 字体大小
            textBox5.Text = "32";

            // 水印类型
            comboBox1.DataSource = CommonRef.WatermarkTypeBindSource;
            comboBox1.DisplayMember = "Text";
            comboBox1.ValueMember = "Value";
            comboBox1.SelectedValue = 0;

            // 字体列表
            comboBox2.DataSource = CommonRef.InstalledAllFontNameAndPaths;
            comboBox2.DisplayMember = "Text";
            comboBox2.ValueMember = "Value";
            comboBox2.SelectedValue = CommonRef.InstalledAllFontNameAndPaths
                .FirstOrDefault(tv => tv.Text.Contains("黑体"))?.Value??0;

            // 水印文字颜色
            comboBox3.DataSource = CommonRef.WatermarkTextColorBindSource;
            comboBox3.DisplayMember = "Text";
            comboBox3.ValueMember = "Value";
            comboBox3.SelectedValue = 0;
        }

        /// <summary>
        /// 获取系统安装的字体路径列表
        /// </summary>
        /// <returns></returns>
        public static List<TextValue> GetInstalledFontPaths()
        {
            string fontsfolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            DirectoryInfo fontDirInfo = new(fontsfolder);
            CommonRef.InstalledAllFontNameAndPaths = fontDirInfo.GetFiles()
                .Where(fontInfo => fontInfo.Name.ToUpper().EndsWith(".TTF"))
                .Select(fontInfo => {
                    PrivateFontCollection tmpPrivateFontCle = new();
                    tmpPrivateFontCle.AddFontFile(fontInfo.FullName);
                    if (tmpPrivateFontCle.Families.Any())
                    {
                        return new TextValue
                        {
                            Text = tmpPrivateFontCle.Families[0].Name,
                            Value = fontInfo.FullName
                        };
                    }
                    return  new TextValue
                    {
                        Text = "",
                        Value = null
                    };;
                }).Where(tv=>tv.Value!=null).ToList();
            
            return CommonRef.InstalledAllFontNameAndPaths;
        }


        /// <summary>
        /// 文字水印颜色选项改变时，更新RGBA输入框的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            var v = comboBox3.SelectedValue;
            int key = 0;
            if (v is TextValue)
            {
                TextValue v2 = (TextValue)v;
                key = (int)v2.Value;
            }
            else
            {
                key = Convert.ToInt32(v);
            }
            var rgbaObj = CommonRef.WatermarkTextColorValueToRGBA[key];
            textBox8.Text = Convert.ToString(rgbaObj.R);
            textBox9.Text = Convert.ToString(rgbaObj.G);
            textBox10.Text = Convert.ToString(rgbaObj.B);
            textBox11.Text = Convert.ToString(rgbaObj.A);
        }

        public string? WatermarkPageArgsDeal()
        {
            if (Convert.ToInt32(comboBox1.SelectedValue) ==0 && String.IsNullOrWhiteSpace(textBox6.Text.Trim()))
            {
                MessageBox.Show("水印文字为空。请输入文本", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            var waterInputParams = new
            {
                Anchor = watermarkPageAnchorBox.SelectedValue,
                Point = new { X = Convert.ToInt64(textBox1.Text), Y = Convert.ToInt64(textBox2.Text) },
                // 偏移量（像素）
                Offset = new { X = Convert.ToInt64(textBox3.Text), Y = Convert.ToInt64(textBox4.Text) },
                // 原图片绝对路径(目录和单一图片)
                Paths = PATHS,
                // 输出目录
                OutDir = OUTDIR,
                // 水印类型
                WatermarkType = comboBox1.SelectedValue,
                TextWaterInputParams = new {
                    FontPath = comboBox2.SelectedValue,
			        // 字体大小
			        FontSize= Convert.ToInt32(textBox5.Text),
			        // RGBA颜色与透明度
			        RGBA= new { 
                        R= Convert.ToInt32(textBox8.Text),
                        G= Convert.ToInt32(textBox9.Text), 
                        B= Convert.ToInt32(textBox10.Text), 
                        A= Convert.ToInt32(textBox11.Text)
                    },
			        // 文字水印值
                    // 多行文本 去除windows平台的\r，以\n做统一
			        Text= textBox6.Text.Replace("\r", ""),
                },
                ImageWaterInputParams = new { },
            };
            return CommonRef.JSON_ENCODE(waterInputParams);
        }

        /// <summary>
        /// 水印锚点更改时，自定义位置无法修改。除非锚点未选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void watermarkPageAnchorBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var v = watermarkPageAnchorBox.SelectedValue;
            int value = 0;
            if (v is TextValue)
            {
                TextValue v2 = (TextValue)v;
                value = (int)v2.Value;
            }
            else
            {
                value = Convert.ToInt32(v);
            }
            if (value != -1)
            {
                flowLayoutPanel2.Enabled = false;
            }
            else
            {
                flowLayoutPanel2.Enabled = true;
            }
        }
        /// <summary>
        /// 水印类型更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            var v = comboBox1.SelectedValue;
            int value = 0;
            if (v is TextValue)
            {
                TextValue v2 = (TextValue)v;
                value = (int)v2.Value;
            }
            else
            {
                value = Convert.ToInt32(v);
            }
            if (value != 0)
            {
                runBtn.Enabled = false;
                MessageBox.Show("图片水印功能暂不可用", "Coming soon..", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                runBtn.Enabled = true;
            }
        }
    }
}
