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
using InventoryService.Models;

namespace InventoryService.Controllers
{
    public class ProductInventoryEntriesController : ApiController
    {
        private InventoryServiceContext db = new InventoryServiceContext();

        // GET: api/ProductInventoryEntries
        public IQueryable<ProductInventoryEntry> GetProductInventoryEntries()
        {
            return db.ProductInventoryEntries;
        }

        // GET: api/ProductInventoryEntries/5
        [ResponseType(typeof(ProductInventoryEntry))]
        public async Task<IHttpActionResult> GetProductInventoryEntry(int id)
        {
            ProductInventoryEntry productInventoryEntry = await db.ProductInventoryEntries.FindAsync(id);
            if (productInventoryEntry == null)
            {
                return NotFound();
            }

            return Ok(productInventoryEntry);
        }

        // PUT: api/ProductInventoryEntries/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProductInventoryEntry(int id, ProductInventoryEntry productInventoryEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productInventoryEntry.Id)
            {
                return BadRequest();
            }

            db.Entry(productInventoryEntry).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductInventoryEntryExists(id))
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

        // POST: api/ProductInventoryEntries
        [ResponseType(typeof(ProductInventoryEntry))]
        public async Task<IHttpActionResult> PostProductInventoryEntry(ProductInventoryEntry productInventoryEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProductInventoryEntries.Add(productInventoryEntry);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = productInventoryEntry.Id }, productInventoryEntry);
        }

        // DELETE: api/ProductInventoryEntries/5
        [ResponseType(typeof(ProductInventoryEntry))]
        public async Task<IHttpActionResult> DeleteProductInventoryEntry(int id)
        {
            ProductInventoryEntry productInventoryEntry = await db.ProductInventoryEntries.FindAsync(id);
            if (productInventoryEntry == null)
            {
                return NotFound();
            }

            db.ProductInventoryEntries.Remove(productInventoryEntry);
            await db.SaveChangesAsync();

            return Ok(productInventoryEntry);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductInventoryEntryExists(int id)
        {
            return db.ProductInventoryEntries.Count(e => e.Id == id) > 0;
        }
    }
}