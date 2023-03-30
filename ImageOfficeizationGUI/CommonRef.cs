using ImageOfficeizationGUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace ImageOfficeizationGUI
{
    internal class CommonRef
    {

        /// <summary>
        /// 可以选择的图片格式，用于拖拽选择图片源时进行验证筛选..
        /// </summary>
        public static List<string> IMG_FORMAT_NAME = new()
        {
            "PNG",
            "JPEG",
            "JPG",
            "GIF",
            "BMP",
            "TIF",
            "WEBP",
        };

        /// <summary>
        /// 用来图片转换绑定的去转换的图片格式
        /// </summary>
        
        public static List<TextValue> ImgFormatToConvertBindSource = new()
        {
            new TextValue{Text="PNG",Value= 0},
            new TextValue{Text="JPEG",Value= 1},
            new TextValue{Text="JPG",Value= 2},
            new TextValue{Text="GIF",Value= 3},
            new TextValue{Text="BMP",Value= 4},
            new TextValue{Text="TIF",Value= 5},
            new TextValue{Text="WEBP",Value= 6},
        };

        public static List<TextValue> WatermarkAnchorBindSource = new() { 
            new TextValue{Text="中间",Value= 0},
            new TextValue{Text="左上",Value= 1},
            new TextValue{Text="右上",Value= 2},
            new TextValue{Text="左下",Value= 3},
            new TextValue{Text="右下",Value= 4},
            new TextValue{Text="-取消选中-",Value= -1},
        };

        public static List<TextValue> WatermarkTypeBindSource = new() {
            new TextValue{Text="文字水印",Value= 0},
            new TextValue{Text="图片水印",Value= 1},
        };

        
        public static List<TextValue> WatermarkTextColorBindSource = new() {
            new TextValue{Text="黑色",Value= 0},
            new TextValue{Text="白色",Value= 1},
            new TextValue{Text="红色",Value= 2},
            new TextValue{Text="蓝色",Value= 3},
            new TextValue{Text="绿色",Value= 4},
            new TextValue{Text="黄色",Value= 5},
            new TextValue{Text="紫色",Value= 6},
        };

        public static List<TextValue> CompressSizeQuarityBindSource = new() {
            new TextValue{Text="较高",Value= 0},
            new TextValue{Text="良好",Value= 1},
            new TextValue{Text="较低",Value= 2},
        };

        public static List<TextValue> InstalledAllFontNameAndPaths = Main.GetInstalledFontPaths();

        public static Dictionary<int, dynamic> WatermarkTextColorValueToRGBA = new() {
            { 0, new { R=0, G=0, B=0, A=255} },
            { 1, new { R=255, G=255, B=255, A=255} },
            { 2, new { R=255, G=0, B=0, A=255} },
            { 3, new { R=0, G=0, B=255, A=255} },
            { 4, new { R=0, G=255, B=0, A=255} },
            { 5, new { R=255, G=255, B=0, A=255} },
            { 6, new { R=255, G=0, B=255, A=255} },
        };


        public static string JSON_ENCODE(object data) {
            
            return JsonSerializer.Serialize(data, typeof(object), JSONSerializerOptions);
        }
        public static JsonSerializerOptions JSONSerializerOptions =  new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        public static int SelectValueParse(object selectValueObj)
        {
            var v = selectValueObj;
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

            return value;
        }
    }

}
