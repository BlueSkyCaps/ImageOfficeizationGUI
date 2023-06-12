//using System;
//using System.Collections.Generic;
//using System.Drawing.Imaging;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ImageOfficeizationGUI
//{
//    internal class Test
//    {
//        internal static void RunTest()
//        {
//            string inputFilePath = @"C:\Users\BlueSkyCarry\Desktop\1680   1895 03-liuyif''''''''ei.jpeg";
//            string outputFilePath = @"C:\Users\BlueSkyCarry\Desktop\cccc.gif";
//            var file = new FileInfo(inputFilePath);

//            MessageBox.Show("Bytes before: " + file.Length);

//            var optimizer = new ImageOptimizer();
//            var i2 = optimizer.Compress(file);
//            if (i2)
//            {
//                MessageBox.Show(i2.ToString());

//            }

//            file.Refresh();
//            MessageBox.Show("Bytes : " + file.Length);
//            return;
//            // 加载GIF文件
//            using (Image inputImage = Image.FromFile(inputFilePath))
//            {
//                // 获取GIF编码器
//                ImageCodecInfo gifEncoder = GetEncoder(ImageFormat.Gif);

//                // 获取GIF帧数
//                FrameDimension dimension = new FrameDimension(inputImage.FrameDimensionsList[0]);
//                int frameCount = inputImage.GetFrameCount(dimension);

//                // 压缩每个帧
//                EncoderParameters encoderParams = new EncoderParameters(1);
//                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 8);
//                using (Bitmap compressedImage = new Bitmap(inputImage.Width, inputImage.Height))
//                {
//                    compressedImage.SetResolution(inputImage.HorizontalResolution, inputImage.VerticalResolution);
//                    using (Graphics g = Graphics.FromImage(compressedImage))
//                    {
//                        for (int i = 0; i < frameCount; i++)
//                        {
//                            inputImage.SelectActiveFrame(dimension, i);
//                            using (Bitmap frame = new Bitmap(inputImage))
//                            {
//                                // 使用压缩算法压缩帧
//                                using (MemoryStream ms = new MemoryStream())
//                                {
//                                    EncoderParameters frameEncoderParams = new EncoderParameters(1);
//                                    frameEncoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, 8);
//                                    frame.Save(ms, gifEncoder, frameEncoderParams);
//                                    ms.Position = 0;

//                                    // 解压缩帧
//                                    using (Image decompressedFrame = Image.FromStream(ms))
//                                    {
//                                        g.DrawImage(decompressedFrame, 0, 0);
//                                    }
//                                }
//                            }
//                        }
//                    }

//                    // 保存压缩后的GIF文件
//                    compressedImage.Save(outputFilePath, gifEncoder, encoderParams);
//                }
//            }
//        }

//        static ImageCodecInfo GetEncoder(ImageFormat format)
//        {
//            // 获取指定格式的编码器
//            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
//            foreach (ImageCodecInfo codec in codecs)
//            {
//                if (codec.FormatID == format.Guid)
//                {
//                    return codec;
//                }
//            }
//            return null;
//        }
//    }
//}
