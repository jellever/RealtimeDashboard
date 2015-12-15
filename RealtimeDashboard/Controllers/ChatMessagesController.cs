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
using RealtimeDashboard.Core.Database;
using RealtimeDashboard.Core.Logging;
using RealtimeDashboard.Database;
using RealtimeDashboard.Database.Models;
using RealtimeDashboard.Server.Database;

namespace RealtimeDashboard.Controllers
{
    public class ChatMessagesController : ApiController
    {
        private IUnitOfWork unitOfWork;

        public ChatMessagesController(Ilog log)
        {
            unitOfWork = new ChangeTrackingEFUnitOfWork(new DatabaseContext(), log);
        }

        // GET: api/ChatMessages
        public async Task<IEnumerable<ChatMessage>> GetChatMessages()
        {
            return await unitOfWork.ChatMessageRepository.GetAll();
        }

        // GET: api/ChatMessages/5
        [ResponseType(typeof(ChatMessage))]
        public async Task<IHttpActionResult> GetChatMessage(long id)
        {
            ChatMessage chatMessage = await unitOfWork.ChatMessageRepository.Get(id);
            if (chatMessage == null)
            {
                return NotFound();
            }

            return Ok(chatMessage);
        }

        // PUT: api/ChatMessages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutChatMessage(long id, ChatMessage chatMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != chatMessage.Id)
            {
                return BadRequest();
            }

            await unitOfWork.ChatMessageRepository.Update(chatMessage);

            try
            {
                unitOfWork.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatMessageExists(id))
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

        // POST: api/ChatMessages
        [ResponseType(typeof(ChatMessage))]
        public async Task<IHttpActionResult> PostChatMessage(ChatMessage chatMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await unitOfWork.ChatMessageRepository.Add(chatMessage);
            unitOfWork.Commit();

            return CreatedAtRoute("DefaultApi", new { id = chatMessage.Id }, chatMessage);
        }

        // DELETE: api/ChatMessages/5
        [ResponseType(typeof(ChatMessage))]
        public async Task<IHttpActionResult> DeleteChatMessage(long id)
        {
            ChatMessage chatMessage = await unitOfWork.ChatMessageRepository.Get(id);
            if (chatMessage == null)
            {
                return NotFound();
            }

            await unitOfWork.ChatMessageRepository.Delete(chatMessage);
            unitOfWork.Commit();

            return Ok(chatMessage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChatMessageExists(long id)
        {
            var model = unitOfWork.ChatMessageRepository.Get(id);
            return model != null;
        }
    }
}