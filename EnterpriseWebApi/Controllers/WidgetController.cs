using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Enterprise.DTO;
using Enterprise.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WidgetsController : ControllerBase
    {
        private IWidgetServices _widgetServices;
        private readonly IHttpContextAccessor _contextAccessor;
        string contentFolderPath = @"Content\Images\";
        string contentUrlPath = @"Content\Images\";

        public WidgetsController(IWidgetServices widgetServices, IHttpContextAccessor contextAccessor)

        {
            _widgetServices = widgetServices;
            _contextAccessor = contextAccessor;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<List<WidgetGridDTO>>> Get()
        {
            var result = await _widgetServices.GetAll();
            return Ok(result);
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<ActionResult> UploadWidget([FromForm] WidgetDTO newWidget)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!Directory.Exists(contentFolderPath))
                    {
                        Directory.CreateDirectory(contentFolderPath);
                    }
                    using (var stream = new FileStream(contentFolderPath, FileMode.Create))
                    {
                        await newWidget.FeatureImage.CopyToAsync(stream);
                        newWidget.WidgetResources = new WidgetResourcesDTO();
                        newWidget.WidgetResources.FeatureIamagePath = $"{contentFolderPath}\\{newWidget.FeatureImage.FileName}";
                    }
                    await _widgetServices.UploadWidget(newWidget);
                    return Ok();
                }
                catch(Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest("Not valid");
        }
        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> DeleteWidget([FromBody] int id)
        {
            try
            {
                await _widgetServices.Delete(id);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }
        // GET api/values/5
        [HttpGet("GetWidget/{id}")]
        public async Task<IActionResult> GetWidget(int id)
        {
            try
            {
                var featuredImage = await _widgetServices.GetFeaturedImagePath(id);
                var featuredImageUrl = $"{_contextAccessor.HttpContext.Request.Host.Value}\\{contentFolderPath + featuredImage}";
                return Ok(featuredImageUrl);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }

        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
