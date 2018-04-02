/***
    * ===========================================================
	* 上海沃中宝软件技术有限公司
    * 创建人：强成
    * 创建时间：2018/01/18 16:25:28
    * 说明：
    * ==========================================================
    * */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IClipboard
{
    public class ImageType
    {
        public static string GetType(Image image)
        {
            string FormetType = "PNG";
            try
            {
                if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Tiff.Guid)
                    FormetType = "TIFF";
                else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid)
                    FormetType = "GIF";
                else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
                    FormetType = "JPG";
                else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Bmp.Guid)
                    FormetType = "BMP";
                else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid)
                    FormetType = "PNG";
                else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Icon.Guid)
                    FormetType = "ICO";
                else
                    FormetType = "PNG";
            }
            catch (Exception ex)
            {
                FormetType = "PNG";
            }
            return FormetType;
        }
    }
}
