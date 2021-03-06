﻿using System;
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
using GedPiDev.Domain.Entities;
using GedPiDev.Service.Interfaces;
using GedPiDev.Service.Implementation;

namespace GedPiDev.RestAPI.Controllers
{
    [Authorize]
    public class AttachementsController : ApiController
    {
        private IAttachementService attachmentService = new AttachmentService();

        [Authorize(Roles = "user")]
        // GET: api/Attachements
        public Task<List<Attachement>> GetAttachements()
        {
            return attachmentService.GetAllAsync();
        }
        [Authorize(Roles = "user")]
        // GET: api/Attachements/5
        [ResponseType(typeof(Attachement))]
        public async Task<IHttpActionResult> GetAttachement(string id)
        {
            Attachement attachement = attachmentService.GetById(id);
            if (attachement == null)
            {
                return NotFound();
            }

            return Ok(attachement);
        }
        [Authorize(Roles = "canEdit,CanAdd")]
        // PUT: api/Attachements/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAttachement(string id, Attachement attachement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != attachement.AttachementId)
            {
                return BadRequest();
            }

            attachmentService.Update(attachement);
            attachmentService.CommitAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
        [Authorize(Roles = "canAdd,canEdit")]
        // POST: api/Attachements
        [ResponseType(typeof(Attachement))]
        public async Task<IHttpActionResult> PostAttachement(Attachement attachement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            attachmentService.Add(attachement);
            attachmentService.CommitAsync();

            return CreatedAtRoute("DefaultApi", new { id = attachement.AttachementId }, attachement);
        }
        [Authorize(Roles = "Admin")]
        // DELETE: api/Attachements/5
        [ResponseType(typeof(Attachement))]
        public async Task<IHttpActionResult> DeleteAttachement(string id)
        {
            Attachement attachement = attachmentService.GetById(id);
            if (attachement == null)
            {
                return NotFound();
            }

            attachmentService.Delete(attachement);
            attachmentService.CommitAsync(); 

            return Ok(attachement);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                attachmentService.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AttachementExists(string id)
        {
            return (attachmentService.GetById(id) != null);
        }
    }
}