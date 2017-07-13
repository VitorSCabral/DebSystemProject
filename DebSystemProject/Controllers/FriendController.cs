using System.Web.Http;
using DebSystemProject.Models.Repository;
using System;
using DebSystemProject.Models;
using System.Net;
using System.Net.Http;
using static DebSystemProject.Models.Repository.FriendRepository;

namespace DebSystemProject.Controllers
{
    [RoutePrefix("api/Friend")]
    public class FriendController : ApiController
    {
        public readonly IFriendRepository friendRepository;

        public FriendController() { }

        public FriendController(FriendRepository.IFriendRepository friendRepository)
        {
            this.friendRepository = friendRepository;
        }        

        // POST api/Friend
        /// <summary>
        /// Cadastra um novo amigo. 
        /// </summary>
        /// <param name="friendAttribute">Objeto amigo.</param>
        [AcceptVerbs("POST")]
        [Route("CreateFriend")]
        public HttpResponseMessage CreateFriend([FromBody]FriendViewModel friendAttribute)
        {
            Validates validates = new Validates();
            MessageViewModel messageAttribute = new MessageViewModel();
            var Message = validates.ValidateFriend(friendAttribute);
            if (Message == String.Empty)
            {              
                if (friendRepository == null)
                {
                    FriendRepository friendRepository = new FriendRepository();                    
                }

                Friend friend = new Friend();
                friend.Name = friendAttribute.Name.Trim();
                friend.Sex = friendAttribute.Sex;
                friend.Age = Convert.ToInt32(friendAttribute.Age);

                Boolean sucess = friendRepository.Insert(friend);

                if (sucess)
                {                        
                    return Request.CreateResponse(HttpStatusCode.Created, messageAttribute.Create());
                }                                                  
            }                       
            return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, Message);
        }

        // GET api/Friend/FriendId
        /// <summary>
        /// Recupera amigo pelo Id da tabela. 
        /// </summary>
        /// <param name="friendId">Id do amigo.</param>
        [AcceptVerbs("GET")]
        [Route("FriendId")]
        public HttpResponseMessage GetFriendById(int friendId)
        {
            Friend getFriend = new Friend();

            if (friendRepository == null)
            {
                FriendRepository friendRepository = new FriendRepository();
                getFriend = friendRepository.GetFriendById(friendId);
            }
            else
            {
                getFriend = friendRepository.GetFriendById(friendId);
            }

            if (getFriend != null)
            {
                FriendViewModel friend = new FriendViewModel();

                friend.Id = getFriend.Id;
                friend.Name = getFriend.Name.Trim();
                friend.Sex = getFriend.Sex;
                friend.Age = getFriend.Age.ToString();

                return Request.CreateResponse(HttpStatusCode.OK, friend);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "null");
        }

        // GET api/Friend/FriendName/Name/
        /// <summary>
        /// Recupera amigo pelo nome cadastrado na tabela. 
        /// </summary>
        /// <param name="name">Nome do amigo.</param>
        [AcceptVerbs("GET")]
        [Route("FriendName")]
        public HttpResponseMessage GetByName(String name)
        {
            Friend getFriend = new Friend();

            if (friendRepository == null)
            {
                FriendRepository friendRepository = new FriendRepository();
                getFriend = friendRepository.GetFriendByName(name);
            }
            else
            {
                getFriend = friendRepository.GetFriendByName(name);
            }

            if (getFriend != null)
            {
                FriendViewModel friend = new FriendViewModel();

                friend.Id = getFriend.Id;
                friend.Name = getFriend.Name.Trim();
                friend.Sex = getFriend.Sex;
                friend.Age = getFriend.Age.ToString();

                return Request.CreateResponse(HttpStatusCode.OK, friend);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "null");
        }

        /// <summary>
        /// Atualiza o cadastro do amigo. 
        /// </summary>
        /// <param name="friendAttribute">Objeto amigo.</param>
        /// <param name="friendId">Id do amigo.</param>        
        [AcceptVerbs("PUT")]
        [Route("UpdateFriend")]
        public HttpResponseMessage UpdateFriend([FromBody]FriendViewModel friendAttribute, [FromUri]int friendId)
        {
            Validates validates = new Validates();
            MessageViewModel messageAttribute = new MessageViewModel();
            var Message = validates.ValidateUpdateFriend(friendAttribute, friendId);
            if (Message == String.Empty)
            {
                Friend friend = new Friend();

                if (friendRepository == null)
                {
                    FriendRepository friendRepository = new FriendRepository();
                    friend = friendRepository.GetFriendById(friendId);
                }            

                if (friend != null)
                {
                    friendAttribute.Id = friendId;
                    friend.Name = friendAttribute.Name.Trim();
                    friend.Sex = friendAttribute.Sex;
                    friend.Age = Convert.ToInt32(friendAttribute.Age);

                    Boolean sucess = false;

                    if (friendRepository == null)
                    {
                        FriendRepository friendRepository = new FriendRepository();
                        sucess = friendRepository.Update(friend, friendId);
                    }
                    else
                    {
                        friend = friendRepository.GetFriendById(friendId);
                        sucess = friendRepository.Update(friend, friendId);
                    }

                    if (sucess)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, friendAttribute);
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find friend.");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find friend.");
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, Message);
            }
        }        

        // DELETE api/Friend
        /// <summary>
        /// Deleta o amigo da tabela (físicamente).
        /// </summary>
        /// <param name="friendId">Id do amigo.</param>
        [AcceptVerbs("DELETE")]
        [Route("DeleteFriend/{friendId}")]        
        public HttpResponseMessage DeleteFriendById([FromUri]int friendId)
        {
            Validates validates = new Validates();
            MessageViewModel messageAttribute = new MessageViewModel();
            String Message = validates.ValidateDelete(friendId);
            if (Message == String.Empty)
            {
                Friend friend = new Friend();

                if (friendRepository == null)
                {
                    FriendRepository friendRepository = new FriendRepository();
                    friend = friendRepository.GetFriendById(friendId);

                    if (friend != null)
                    {
                        Boolean sucess = friendRepository.DeleteObject(friend, true);

                        if (sucess)
                        {
                            return new HttpResponseMessage(HttpStatusCode.NoContent);
                        }
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not delete record.");
                    }              
                }
                else
                {
                    friend = friendRepository.GetFriendById(friendId);
                    friendRepository.DeleteObject(friend, true);
                }
                return new HttpResponseMessage(HttpStatusCode.NoContent);                
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Message);
        }
    }
}

