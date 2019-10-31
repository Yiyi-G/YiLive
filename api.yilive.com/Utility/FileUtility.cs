using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;

namespace api.yilive.com.Utility
{
    public static class FileUtility
    {
        public static string SaveBase64ToCacheImage(string base64)
        {
            var parts = base64.Split(',');
            var formatePart = parts[0].ToLower();
            var dataPart = parts[1];
            System.Drawing.Imaging.ImageFormat format;
            var extend = "";
            if (formatePart.Contains("jpeg") || formatePart.Contains("jpg"))
            {
                format = System.Drawing.Imaging.ImageFormat.Jpeg;
                extend = ".jpg";
            }
            else if (formatePart.Contains("bmp"))
            {
                format = System.Drawing.Imaging.ImageFormat.Bmp;
                extend = ".bmp";
            }
            else if (formatePart.Contains("gif"))
            {
                format = System.Drawing.Imaging.ImageFormat.Gif;
                extend = ".gif";
            }
            else if (formatePart.Contains("png"))
            {
                format = System.Drawing.Imaging.ImageFormat.Png;
                extend = ".png";
            }
            else
            {
                throw new System.Exception("图片格式不支持");
            }
            byte[] arr = Convert.FromBase64String(dataPart);
            var relativePath = "cache/"+( Guid.NewGuid().ToString() + extend).ToLower();
            var absloteUrl =Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,("Resource/Img/"+ relativePath).Replace("/","\\"));
            using (MemoryStream ms = new MemoryStream(arr))
            {
                using (Bitmap bmp = new Bitmap(ms))
                {
                    bmp.Save(absloteUrl, format);
                    bmp.Dispose();
                }
            }
            return relativePath;
        }
        public static bool CheckIsTemple(string url)
        {
            if (String.IsNullOrWhiteSpace(url))
                return false;
            return url.StartsWith("cache/");
        }
        public static string SaveTempleFile(string fid,string path)
        {
            if(CheckIsTemple(fid)) 
            {
                var abslotePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory+ "Resource\\Img\\", path.Replace("/","\\"));
                if (!Directory.Exists(abslotePath))
                    Directory.CreateDirectory(abslotePath);
                var absloteCacheUrl = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory , "Resource\\Img\\" + fid.Replace("/","\\"));
                Image cacheImage = System.Drawing.Image.FromFile(absloteCacheUrl);
                var fileName = Guid.NewGuid().ToString() + Regex.Replace(fid, "(.*)(\\..+)", "$2").ToLower();
                var relaviteurl = (path ?? "") +fileName;
                var absloteUrl = abslotePath+fileName;
                using (MemoryStream ms = new MemoryStream())
                {
                    cacheImage.Save(ms, cacheImage.RawFormat);
                    using (Bitmap bmp = new Bitmap(ms))
                    {
                        bmp.Save(absloteUrl, cacheImage.RawFormat);
                        bmp.Dispose();
                    }
                }
                return relaviteurl;
            }
            return fid;
        }
    }
}
