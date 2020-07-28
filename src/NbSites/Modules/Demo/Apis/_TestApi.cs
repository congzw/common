using System;
using Microsoft.AspNetCore.Mvc;

namespace NbSites.Areas.Web.Demo.Apis
{
    [ApiController]
    [Route("Api/Demo/Test")]
    public class _TestApi : ControllerBase
    {
        [HttpGet("GetDate")]
        public DateTime GetDate()
        {
            return DateTime.Now;
        }
    }
}
