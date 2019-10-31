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

namespace api.yilive.com.Controllers.File
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public ImageController() {

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