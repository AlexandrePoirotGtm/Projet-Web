﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Projet_Web.Data;
using Projet_Web.Models;

namespace Projet_Web.Controllers
{
    public class AgenceVoyagesController : ApiController
    {
        private BoVoyageDbContext db = new BoVoyageDbContext();

        // GET: api/AgenceVoyages
        public IQueryable<AgenceVoyage> GetAgencesVoyages()
        {
            return db.AgencesVoyages.Include(XmlReadMode => XmlReadMode.Voyages);
        }

        // GET: api/AgenceVoyages/5
        [Route("api/AgenceVoyages/{id:int}")]
        [ResponseType(typeof(AgenceVoyage))]
        public IHttpActionResult GetAgenceVoyage(int id)
        {
            AgenceVoyage agenceVoyage = db.AgencesVoyages.Include(XmlReadMode => XmlReadMode.Voyages)
                                                           .SingleOrDefault(x => x.ID == id);
            if (agenceVoyage == null)
            {
                return NotFound();
            }

            return Ok(agenceVoyage);
        }

        // GET: api/AgenceVoyages/Search
        [ResponseType(typeof(AgenceVoyage))]
        [Route("api/AgenceVoyages/{Filter}")]
        [HttpGet]
        public IQueryable<AgenceVoyage> GetAgenceVoyageFilter(string Filter)
        {
            return db.AgencesVoyages.Where(x => x.Nom.Contains(Filter));
        }

        // PUT: api/AgenceVoyages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAgenceVoyage(int id, AgenceVoyage agenceVoyage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != agenceVoyage.ID)
            {
                return BadRequest();
            }

            db.Entry(agenceVoyage).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgenceVoyageExists(id))
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

        // POST: api/AgenceVoyages
        [ResponseType(typeof(AgenceVoyage))]
        public IHttpActionResult PostAgenceVoyage(AgenceVoyage agenceVoyage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AgencesVoyages.Add(agenceVoyage);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = agenceVoyage.ID }, agenceVoyage);
        }

        // DELETE: api/AgenceVoyages/5
        [Route("api/AgenceVoyages/{id}")]
        [ResponseType(typeof(AgenceVoyage))]
        public IHttpActionResult DeleteAgenceVoyage(int id)
        {
            AgenceVoyage agenceVoyage = db.AgencesVoyages.Find(id);
            if (agenceVoyage == null)
            {
                return NotFound();
            }
            db.AgencesVoyages.Remove(agenceVoyage);
            try
            {
                db.SaveChanges();
            }
            catch
            {
                return (BadRequest("L'agence a encore des voyages"));
            }

            return Ok(agenceVoyage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AgenceVoyageExists(int id)
        {
            return db.AgencesVoyages.Count(e => e.ID == id) > 0;
        }
    }
}