using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Example.Models;

namespace WebApi.Example.Controllers
{
    public class GroupsController : ApiController
    {
        private ITContext db = new ITContext();

        // GET: api/Groups
        public IQueryable<Group> GetGroups()
        {
            return db.Groups
                .Include(u => u.Users);
        }

        // GET: api/Groups/5
        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> GetGroup(Guid id)
        {
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        // PUT: api/Groups/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGroup(Guid id, Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != group.Id)
            {
                return BadRequest();
            }

            db.Entry(group).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
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

        // POST: api/Groups
        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> PostGroup(Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Groups.Add(group);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GroupExists(group.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = group.Id }, group);
        }

        // DELETE: api/Groups/5
        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> DeleteGroup(Guid id)
        {
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            db.Groups.Remove(group);
            await db.SaveChangesAsync();

            return Ok(group);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GroupExists(Guid id)
        {
            return db.Groups.Count(e => e.Id == id) > 0;
        }
    }
}