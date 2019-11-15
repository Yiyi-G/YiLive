using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tgnet.Api.Mvc;
using System.IO;
using Tgnet.Api;
using Tgnet;
using api.yilive.com.Models.Request;
using Microsoft.AspNetCore.StaticFiles;

namespace api.yilive.com.Controllers.File
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public ImageController() {

        }
        public object Get(string fid)
        {
            var absloteUrl = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, ("Resource/Img/" + fid).Replace("/", "\\"));
            FileInfo fi = new FileInfo(absloteUrl);
            if (!fi.Exists)
            {
                return null;
            }
            FileStream fs = fi.OpenRead();
            byte[] buffer = new byte[fi.Length];
            //读取图片字节流
            //从流中读取一个字节块，并在给定的缓冲区中写入数据。
            fs.Read(buffer, 0, Convert.ToInt32(fi.Length));
            var resource = File(buffer, Utility.MimeTypeHelper.GetMimeType(fid));
            fs.Close();
            return resource;
        }
        [HttpPost]
        public object UploadTempleImage([FromBody]UploadImgRequest request)
        {
            ExceptionHelper.ThrowIfNull(request, nameof(request));
            ExceptionHelper.ThrowIfNullOrEmpty(request.base64, "base64");
            var fid =  Utility.FileUtility.SaveBase64ToCacheImage(request.base64);
            return this.JsonApiResult(ErrorCode.None, new { fid = fid });
        }
    }
}