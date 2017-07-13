using System.Web.Http;
using DebSystemProject.Models.Repository;
using System;
using System.Collections.Generic;
using DebSystemProject.Models;
using static DebSystemProject.Models.Repository.DebtRepository;
using System.Net.Http;
using System.Net;

namespace DebSystemProject.Controllers
{
    [RoutePrefix("api/Debt")]
    public class DebtController : ApiController
    {
        private readonly IDebtRepository debtRepository;

        public DebtController() { }

        public DebtController(IDebtRepository debtRepository)
        {
            this.debtRepository = debtRepository;
        }

        // POST api/Debt
        /// <summary>
        /// Cadastra uma nova dívida. 
        /// </summary>
        /// <param name="debtAttribute">Objeto do débito.</param> 
        [AcceptVerbs("POST")]
        [Route("CreateDebt")]
        public HttpResponseMessage CreateDebt([FromBody]DebtViewModel debtAttribute)
        {
            Validates validates = new Validates();
            MessageViewModel messageAttribute = new MessageViewModel();
            var Message = validates.ValidateDebt(debtAttribute);
            if (Message == String.Empty)
            {

                Debt debt = new Debt();
                Boolean sucess;

                if (debtRepository == null)
                {
                    DebtRepository debtRepository = new DebtRepository();
                    debt.Value = Convert.ToDecimal(debtAttribute.Value);
                    debt.Date = Convert.ToDateTime(debtAttribute.Date);
                    debt.Description = debtAttribute.Description;
                    FriendRepository friendName = new FriendRepository();
                    debt.FriendIdIn = friendName.GetFriendIdByName(debtAttribute.FriendNameIn);
                    debt.FriendIdOut = friendName.GetFriendIdByName(debtAttribute.FriendNameOut);

                    sucess = debtRepository.Insert(debt);

                    if (sucess)
                    {
                        return Request.CreateResponse(HttpStatusCode.Created, messageAttribute.Create());
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not create debit.");
                }
                else
                {
                    sucess = debtRepository.Insert(debt);

                    if (sucess)
                    {
                        return Request.CreateResponse(HttpStatusCode.Created, messageAttribute.Create());
                    }
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not create debit.");
            }
            return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, Message);
        }

        // GET api/Debt/List/Name
        /// <summary>
        /// Recupera todos os valores que deve receber (Ordenado por amigo).
        /// </summary>
        /// <param name="name">Nome do credor.</param>                
        [AcceptVerbs("GET")]
        [Route("ListDebt")]
        public HttpResponseMessage GetListByFriendName(String name)
        {
            IList<Debt> debtList = new List<Debt>();

            if (debtRepository == null)
            {
                DebtRepository debtRepository = new DebtRepository();
                debtList = debtRepository.GetDebtListByFriendName(name);

                if (debtList != null)
                {
                    List<DebtViewModel> listDebt = new List<DebtViewModel>();
                    FriendRepository friendName = new FriendRepository();

                    foreach (dynamic item in debtList)
                    {
                        listDebt.Add(new DebtViewModel
                        {
                            Id = item.Id,
                            Date = Convert.ToString(item.Date),
                            Description = item.Description,
                            Value = item.Value.ToString(),
                            FriendNameIn = friendName.GetFriendNameById(item.FriendIdIn).Trim(),
                            FriendNameOut = friendName.GetFriendNameById(item.FriendIdOut).Trim()
                        });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listDebt.ToArray());
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find debt.");
            }
            else
            {
                debtList = debtRepository.GetDebtListByFriendName(name);

                if (debtList != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, debtList);
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find debt.");
        }

        // GET api/Debt/ListDebtFriend/NameOut
        /// <summary>
        /// Recupera todos os valores que deve pagar para os amigos.
        /// </summary>
        /// <param name="nameOut">Nome do devedor.</param>                
        [AcceptVerbs("GET")]
        [Route("ListDebtFriend")]
        public HttpResponseMessage GetListMyDebtsByFriendName(String nameOut)
        {
            DebtRepository debtRepository = new DebtRepository();
            IList<Debt> listDebt = debtRepository.GetListMyDebtsByFriendName(nameOut);

            if (listDebt != null)
            {
                List<DebtViewModel> listDebtView = new List<DebtViewModel>();
                FriendRepository friendName = new FriendRepository();

                foreach (dynamic item in listDebt)
                {
                    listDebtView.Add(new DebtViewModel
                    {
                        Id = item.Id,
                        Date = Convert.ToString(item.Date),
                        Description = item.Description,
                        Value = item.Value.ToString(),
                        FriendNameIn = friendName.GetFriendNameById(item.FriendIdIn).Trim(),
                        FriendNameOut = friendName.GetFriendNameById(item.FriendIdOut).Trim()
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, listDebtView.ToArray());
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find debt.");
        }

        // GET api/Debt/ById/Id
        /// <summary>
        /// Recupera o débito através do Id.
        /// </summary>
        /// <param name="debtId">Id do débito.</param>                
        [AcceptVerbs("GET")]
        [Route("DebtById")]
        public HttpResponseMessage GetDebtById(int debtId)
        {
            Debt getDebt = new Debt();

            if (debtRepository == null)
            {
                DebtRepository debtRepository = new DebtRepository();
                getDebt = debtRepository.GetDebtById(debtId);

                if (getDebt != null)
                {
                    DebtViewModel debt = new DebtViewModel();
                    FriendRepository friendName = new FriendRepository();

                    debt.Id = getDebt.Id;
                    debt.Value = getDebt.Value.ToString();
                    debt.Date = Convert.ToString(getDebt.Date);
                    debt.Description = getDebt.Description;
                    debt.FriendNameIn = friendName.GetFriendNameById(getDebt.FriendIdIn).Trim();
                    debt.FriendNameOut = friendName.GetFriendNameById(getDebt.FriendIdOut).Trim();

                    return Request.CreateResponse(HttpStatusCode.OK, debt);
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find debt.");
            }
            else
            {
                getDebt = debtRepository.GetDebtById(debtId);

                if (getDebt != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, getDebt);
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find debt.");
        }        

        // GET api/Debt/Total/Name
        /// <summary>
        /// Retorna o total em valor que deve receber.
        /// </summary>
        /// <param name="name">Nome do credor.</param>   
        /// <returns>Total do valor receber.</returns>        
        [AcceptVerbs("GET")]
        [Route("Total")]
        public HttpResponseMessage GetTotalDebts(String name)
        {
            DebtRepository debtRepository = new DebtRepository();
            String totalDebt = debtRepository.GetTotalDebtsByFriendName(name);

            if (totalDebt != "")
            {
                return Request.CreateResponse(HttpStatusCode.OK, totalDebt);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find debt.");
        }

        /// <summary>
        /// Atualiza a dívida do amigo. 
        /// </summary>
        /// <param name="debtAttribute">Objeto débito.</param>
        /// <param name="debtId">Id do débito.</param>        
        [AcceptVerbs("PUT")]
        [Route("UpdateDebt")]
        public HttpResponseMessage UpdateDebt([FromBody]DebtViewModel debtAttribute, [FromUri]int debtId)
        {
            Validates validates = new Validates();
            MessageViewModel messageAttribute = new MessageViewModel();
            String Message = validates.ValidateUpdateDebt(debtAttribute, debtId);
            if (Message == String.Empty)
            {
                Debt debt = new Debt();
                FriendRepository friendName = new FriendRepository();
                Boolean sucess;

                if (debtRepository == null)
                {
                    DebtRepository debtRepository = new DebtRepository();

                    debt = debtRepository.GetDebtById(debtId);

                    if (debt != null)
                    {
                        debtAttribute.Id = debtId;
                        debt.Value = Convert.ToDecimal(debtAttribute.Value);
                        debt.Date = Convert.ToDateTime(debtAttribute.Date);
                        debt.Description = debtAttribute.Description;
                        debt.FriendIdIn = friendName.GetFriendIdByName(debtAttribute.FriendNameIn);
                        debt.FriendIdOut = friendName.GetFriendIdByName(debtAttribute.FriendNameOut);

                        sucess = debtRepository.Update(debt, debtId);

                        if (sucess)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, debtAttribute);
                        }
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not update debt.");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find debt.");
                    }
                }
                else
                {
                    debt = debtRepository.GetDebtById(debtId);
                    sucess = debtRepository.Update(debt, debtId);

                    if (sucess)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, debt);
                    }
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find debt.");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, Message);
            }
        }

        // DELETE api/Debt
        /// <summary>
        /// Deleta a dívida da tabela (físicamente).
        /// </summary>
        /// <param name="debtId">Id do débito.</param>
        [AcceptVerbs("DELETE")]
        [Route("DeleteDebt/{debtId}")]
        public HttpResponseMessage DeleteDebtById([FromUri]int debtId)
        {
            Validates validates = new Validates();
            MessageViewModel messageAttribute = new MessageViewModel();
            String Message = validates.ValidateDelete(debtId);
            if (Message == String.Empty)
            {
                Debt debt = new Debt();
                Boolean sucess;

                if (debtRepository == null)
                {
                    DebtRepository debtRepository = new DebtRepository();
                    debt = debtRepository.GetDebtById(debtId);

                    if (debt != null)
                    {
                        sucess = debtRepository.DeleteObject(debt, true);

                        if (sucess)
                        {
                            return new HttpResponseMessage(HttpStatusCode.NoContent);
                        }
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not delete record.");
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Register not found.");
                }
                else
                {
                    debt = debtRepository.GetDebtById(debtId);
                    sucess = debtRepository.DeleteObject(debt, true);

                    if (sucess)
                    {
                        return new HttpResponseMessage(HttpStatusCode.NoContent);
                    }
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Register not found.");
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Message);
        }

    }
}

