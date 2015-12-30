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
    public class ChatRoomsController : ApiController
    {
        private IUnitOfWork unitOfWork;

        public ChatRoomsController(Ilog log)
        {
            unitOfWork = new ChangeTrackingEFUnitOfWork(new DatabaseContext(), log);
        }

        [Route("api/ChatRooms/{id}/ChatMessages")]
        public async Task<IEnumerable<ChatMessage>> GetChatroomMessages(int id)
        {
            return await unitOfWork.ChatMessageRepository.GetAll(x => x.ChatRoomId == id);
        }

        // GET: api/ChatRooms
        public async Task<IEnumerable<ChatRoom>> GetChatRooms()
        {
            return await unitOfWork.ChatRoomRepository.GetAll();
        }

        // GET: api/ChatRooms/5
        [ResponseType(typeof(ChatRoom))]
        public async Task<IHttpActionResult> GetChatRoom(long id)
        {
            ChatRoom chatRoom = await unitOfWork.ChatRoomRepository.Get(x => x.Id == id, x => x.ChatMessages);
            if (chatRoom == null)
            {
                return NotFound();
            }

            return Ok(chatRoom);
        }

        // PUT: api/ChatRooms/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutChatRoom(long id, ChatRoom chatRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != chatRoom.Id)
            {
                return BadRequest();
            }

            await unitOfWork.ChatRoomRepository.Update(chatRoom);

            try
            {
                unitOfWork.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatRoomExists(id))
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

        // POST: api/ChatRooms
        [ResponseType(typeof(ChatRoom))]
        public async Task<IHttpActionResult> PostChatRoom(ChatRoom chatRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await unitOfWork.ChatRoomRepository.Add(chatRoom);
            unitOfWork.Commit();

            return CreatedAtRoute("DefaultApi", new { id = chatRoom.Id }, chatRoom);
        }

        // DELETE: api/ChatRooms/5
        [ResponseType(typeof(ChatRoom))]
        public async Task<IHttpActionResult> DeleteChatRoom(long id)
        {
            ChatRoom chatRoom = await unitOfWork.ChatRoomRepository.Get(id);
            if (chatRoom == null)
            {
                return NotFound();
            }

            await unitOfWork.ChatRoomRepository.Delete(chatRoom);
            unitOfWork.Commit();

            return Ok(chatRoom);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Commit();
            }
            base.Dispose(disposing);
        }

        private bool ChatRoomExists(long id)
        {
            var model = unitOfWork.ChatRoomRepository.Get(id);
            return model != null;
        }
    }
}