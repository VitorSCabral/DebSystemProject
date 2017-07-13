using System.Web.Http;
using DebSystemProject.Models.Repository;
using System;
using DebSystemProject.Models;
using System.Collections.Generic;
using static DebSystemProject.Models.Repository.PaymentRepository;
using System.Net;
using System.Net.Http;

namespace DebSystemProject.Controllers
{
    [RoutePrefix("api/Payment")]
    public class PaymentController : ApiController
    {
        private readonly IPaymentRepository paymentRepository;

        public PaymentController() { }

        public PaymentController(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        // POST api/Payment
        /// <summary>
        /// Insere um pagamento, e apaga o débito com o amigo.    
        /// </summary>
        /// <param name="debtId">Id do débito.</param>
        [AcceptVerbs("POST")]
        [Route("CreatePayment")]
        public HttpResponseMessage CreatePayment([FromUri]int debtId)
        {
            MessageViewModel messageAttribute = new MessageViewModel();
            Debt debt = new Debt();

            if (paymentRepository == null)
            {
                DebtRepository debtRepository = new DebtRepository();
                debt = debtRepository.GetDebtById(debtId);

                if (debt != null)
                {
                    PaymentRepository paymentRepository = new PaymentRepository();
                    Boolean sucess = paymentRepository.Insert(debt);

                    if (sucess)
                    {
                        return Request.CreateResponse(HttpStatusCode.Created, messageAttribute.Create());
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not create payment.");
                }
            }
            else
            {
                paymentRepository.Insert(debt);
            }
            return Request.CreateResponse(HttpStatusCode.Created, messageAttribute.Create());
        }       

        // GET api/Payment/Name/NameOut
        /// <summary>
        /// Lista todos os valores quitados com meus amigos.
        /// </summary>
        /// <param name="nameOut">Nome do devedor</param>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        [Route("ListPaymentFriend")]
        public HttpResponseMessage GetListMyPaymentsByFriendName(String nameOut)
        {
            PaymentRepository paymentRepository = new PaymentRepository();
            IList<Payment> paymentList = paymentRepository.GetPaymentListByFriendName(nameOut);

            if (paymentList != null)
            {
                List<PaymentViewModel> listPaymentView = new List<PaymentViewModel>();
                FriendRepository friendName = new FriendRepository();

                foreach (dynamic item in paymentList)
                {
                    listPaymentView.Add(new PaymentViewModel
                    {
                        Id = item.Id,
                        Value = item.Value.ToString(),
                        Date = Convert.ToString(item.Date),
                        FriendNameIn = friendName.GetFriendNameById(item.FriendIdIn).Trim(),
                        FriendNameOut = friendName.GetFriendNameById(item.FriendIdOut).Trim()
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, listPaymentView.ToArray());
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find payment.");
        }

        // GET api/Payment/ById/Id
        /// <summary>
        /// Recupera o pagamento através do Id.
        /// </summary>
        /// <param name="paymentId">Id do pagamento.</param>        
        [AcceptVerbs("GET")]
        [Route("PaymentById")]
        public HttpResponseMessage GetPaymentById(int paymentId)
        {
            Payment getPayment = new Payment();

            if (paymentRepository == null)
            {
                PaymentRepository paymentRepository = new PaymentRepository();
                getPayment = paymentRepository.GetPaymentById(paymentId);

                if (getPayment != null)
                {
                    PaymentViewModel payment = new PaymentViewModel();
                    payment.Id = getPayment.Id;
                    payment.Date = Convert.ToString(getPayment.Date);
                    payment.Value = Convert.ToString(getPayment.Value);
                    FriendRepository friendName = new FriendRepository();
                    payment.FriendNameIn = friendName.GetFriendNameById(getPayment.FriendIdIn).Trim();
                    payment.FriendNameOut = friendName.GetFriendNameById(getPayment.FriendIdOut).Trim();

                    return Request.CreateResponse(HttpStatusCode.OK, payment);
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find payment.");
            }
            else
            {
                getPayment = paymentRepository.GetPaymentById(paymentId);

                if (getPayment != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, getPayment);
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find payment.");
        }

        /// <summary>
        /// Atualiza o pagamento do amigo. 
        /// </summary>
        /// <param name="paymentAttribute">Objeto pagamento.</param>
        /// <param name="paymentId">Id do pagamento.</param>        
        [AcceptVerbs("PUT")]
        [Route("UpdatePayment")]
        public HttpResponseMessage UpdatePayment([FromBody]PaymentViewModel paymentAttribute, [FromUri]int paymentId)
        {
            Validates validates = new Validates();
            MessageViewModel messageAttribute = new MessageViewModel();
            String Message = validates.ValidateUpdatePayment(paymentAttribute, paymentId);
            if (Message == String.Empty)
            {
                Payment payment = new Payment();

                if (paymentRepository == null)
                {
                    PaymentRepository paymentRepository = new PaymentRepository();
                    payment = paymentRepository.GetPaymentById(paymentId);

                    if (payment != null)
                    {
                        paymentAttribute.Id = paymentId;
                        payment.Value = Convert.ToDecimal(paymentAttribute.Value);
                        FriendRepository friendName = new FriendRepository();
                        payment.FriendIdIn = friendName.GetFriendIdByName(paymentAttribute.FriendNameIn);
                        payment.FriendIdOut = friendName.GetFriendIdByName(paymentAttribute.FriendNameOut);

                        Boolean sucess = paymentRepository.Update(payment, paymentId);

                        if (sucess)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, paymentAttribute);
                        }
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not update payment.");
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find payment.");
                }
                else
                {
                    payment = paymentRepository.GetPaymentById(paymentId);
                    paymentRepository.Update(payment, paymentId);
                }
                return Request.CreateResponse(HttpStatusCode.OK, payment);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, Message);
            }
        }

        // DELETE api/Payment
        /// <summary>
        /// Deleta o pagamento da tabela (físicamente).
        /// </summary>
        /// <param name="paymentId">Id do pagamento.</param>
        [AcceptVerbs("DELETE")]
        [Route("DeletePayment/{paymentId}")]
        public HttpResponseMessage DeletePaymentById([FromUri]int paymentId)
        {
            Validates validates = new Validates();
            MessageViewModel messageAttribute = new MessageViewModel();
            String Message = validates.ValidateDelete(paymentId);
            if (Message == String.Empty)
            {
                Payment payment = new Payment();
                Boolean sucess;

                if (paymentRepository == null)
                {
                    PaymentRepository paymentRepository = new PaymentRepository();
                    payment = paymentRepository.GetPaymentById(paymentId);

                    if (payment != null)
                    {
                        sucess = paymentRepository.DeleteObject(payment, true);

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
                    payment = paymentRepository.GetPaymentById(paymentId);
                    sucess = paymentRepository.DeleteObject(payment, true);

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

