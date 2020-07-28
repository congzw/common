using System;
using Microsoft.AspNetCore.Mvc;

namespace NbSites.Web.Apis
{
    [Route("Api/Test")]
    public class TestApiController : ControllerBase
    {
        [HttpGet("GetDate")]
        public DateTime GetDate()
        {
            return DateTime.Now;
        }

        [HttpGet("GetService")]
        public string GetService([FromServices]TestAppService testAppService)
        {
            return testAppService.Id;
        }
    }

    public class TestAppService
    {
        public TestAppService()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
    }
}
