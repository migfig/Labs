using System;
using System.IO;
using System.Web.Http;

namespace Common.Controllers
{
    [RoutePrefix("api/stories")]
    public class StoriesController: ApiController
    {
        [Route(""), HttpGet]
        public string GetStories()
        {
            return "[" + GetStoryContent() + "]";
        }

        [Route("{id}"), HttpGet]
        public string GetStory(string id)
        {
            return GetStoryContent();
        }

        private string GetStoryContent()
        {
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "story.json"));
        }
    }
}
