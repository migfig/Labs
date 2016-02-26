using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Interviewer.Data;

namespace Interviewer.Services.Web.Controllers
{
    public class PlatformsController : ApiController
    {
        private InterViewerContext db = new InterViewerContext();

        // GET: api/Platforms
        public IQueryable<Platform> GetPlatforms()
        {
            return db.Platforms;
        }

        // GET: api/Platforms/5
        [ResponseType(typeof(Platform))]
        public async Task<IHttpActionResult> GetPlatform(int id)
        {
            Platform platform = await db.Platforms.FindAsync(id);
            if (platform == null)
            {
                return NotFound();
            }

            return Ok(platform);
        }

        // PUT: api/Platforms/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPlatform(int id, Platform platform)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != platform.Id)
            {
                return BadRequest();
            }

            db.Entry(platform).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlatformExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Platforms
        [ResponseType(typeof(Platform))]
        public async Task<IHttpActionResult> PostPlatform(Platform platform)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Platforms.Add(platform);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = platform.Id }, platform);
        }

        // DELETE: api/Platforms/5
        [ResponseType(typeof(Platform))]
        public async Task<IHttpActionResult> DeletePlatform(int id)
        {
            Platform platform = await db.Platforms.FindAsync(id);
            if (platform == null)
            {
                return NotFound();
            }

            db.Platforms.Remove(platform);
            await db.SaveChangesAsync();

            return Ok(platform);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlatformExists(int id)
        {
            return db.Platforms.Count(e => e.Id == id) > 0;
        }
    }
}