using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ImagenRepoEntities.Entities;
using ImagenRepoEntities.Models;
using ImagenRepoServices.IServices;
using ImagenRepoDomain.Dtos;
using AutoMapper;

namespace ImagenRepositorio.Controllers
{
    public class TagsController : ApiController
    {
        private ModelContainer db = new ModelContainer();

        private ITagService tagService;

        public TagsController(ITagService tagService)
        {
            this.tagService = tagService;
        }

        // GET: api/Tags
        public IEnumerable<TagDto> GetTags()
        {
            var tags = this.tagService.GetAll().Select(ConvertToDto).ToList();
            return tags;
        }

        // GET: api/Tags/5
        [ResponseType(typeof(Tag))]
        public IHttpActionResult GetTag(int id)
        {
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        // PUT: api/Tags/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTag(int id, TagDto tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tag.Id)
            {
                return BadRequest();
            }

            var originalTag = ConvertFromDto(tag);
            this.tagService.Update(originalTag);
            return Ok(ConvertToDto(originalTag));

            //db.Entry(tag).State = EntityState.Modified;

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!TagExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Tags
        [ResponseType(typeof(Tag))]
        public IHttpActionResult PostTag(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tags.Add(tag);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tag.Id }, tag);
        }

        // DELETE: api/Tags/5
        [ResponseType(typeof(Tag))]
        public IHttpActionResult DeleteTag(int id)
        {
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }

            db.Tags.Remove(tag);
            db.SaveChanges();

            return Ok(tag);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }

        private bool TagExists(int id)
        {
            return db.Tags.Count(e => e.Id == id) > 0;
        }

        private static TagDto ConvertToDto(Tag tag)
        {
            return Mapper.Map<TagDto>(tag);
        }

        private static Tag ConvertFromDto(TagDto tagDto)
        {
            return Mapper.Map<Tag>(tagDto);
        }
    }
}