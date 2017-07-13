using System.Data.Entity.Validation;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using static DebSystemProject.Models.Repository.PaymentRepository;

namespace DebSystemProject.Models.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        // Cadastra um pagamento na tabela       
        public Boolean Insert(Debt debt)
        {
            try
            {
                Connection connection = Connection.Instance;

                Payment payment = new Payment();
                payment.Date = DateTime.Now;
                payment.Value = debt.Value;
                payment.FriendIdIn = debt.FriendIdIn;
                payment.FriendIdOut = debt.FriendIdOut;

                connection.dateBase.Payments.Add(payment);

                // Ao pagar a dívida excluir ela do banco
                DebtRepository debtRepository = new DebtRepository();
                debtRepository.DeleteObject(debt);

                connection.dateBase.SaveChanges();

                return true;
            }
            catch (DbEntityValidationException e)
            {
                return false;
                throw e;
            }
        }

        // Busca um pagamento por id
        public Payment GetPaymentById(int paymentId)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();

                var paymentObj = from Payment in connection.dateBase.Payments
                                 where Payment.Id == paymentId
                                 select Payment;

                if (paymentObj.Count() > 0)
                {
                    return paymentObj.First();
                }
                else
                {
                    return null;
                }

            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
        }

        // Atualiza o pagamento
        public Boolean Update(Payment payment, int paymentId)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();

                PaymentRepository paymentRepository = new PaymentRepository();
                Payment updatePayment = paymentRepository.GetPaymentById(paymentId);

                updatePayment.Date = DateTime.Now;

                if (payment.Value > 0)
                {
                    updatePayment.Value = payment.Value;
                }
                if (payment.FriendIdIn > 0)
                {
                    updatePayment.FriendIdIn = payment.FriendIdIn;
                }
                if (payment.FriendIdOut > 0)
                {
                    updatePayment.FriendIdOut = payment.FriendIdOut;
                }

                connection.dateBase.Payments.Attach(updatePayment);
                connection.dateBase.Entry(updatePayment).State = EntityState.Modified;
                connection.dateBase.SaveChanges();

                return true;
            }
            catch (DbEntityValidationException e)
            {
                return false;
                throw e;
            }
        }

        //Deleta o pagamento da tabela (físicamente).
        public Boolean DeleteObject(Payment payment, bool commit = false)
        {
            try
            {
                Connection connection = Connection.Instance;

                if (payment != null)
                {

                    connection.dateBase.Payments.Attach(payment);
                    connection.dateBase.Payments.Remove(payment);

                    if (commit)
                    {
                        connection.dateBase.SaveChanges();

                        return true;
                    }
                }
                return false;
            }
            catch (DbEntityValidationException e)
            {
                return false;
                throw e;
            }
        }

        // Busca uma lista de pagamentos ordenadas pro amigo
        public IList<Payment> GetPaymentListByFriendName(String nameOut)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();

                var paymentListObj = from payment in connection.dateBase.Payments
                                     join friendIn in connection.dateBase.Friends on payment.FriendIdIn equals friendIn.Id
                                     join friendOut in connection.dateBase.Friends on payment.FriendIdOut equals friendOut.Id
                                     where friendOut.Name == nameOut
                                     orderby friendIn.Name, friendOut.Name
                                     select payment;

                if (paymentListObj.Count() > 0)
                {
                    return paymentListObj.ToList();
                }
                return null;
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
        }

        public interface IPaymentRepository
        {
            Boolean Insert(Debt debt);
            Payment GetPaymentById(int paymentId);
            Boolean Update(Payment payment, int paymentId);
            Boolean DeleteObject(Payment payment, bool commit = false);
            IList<Payment> GetPaymentListByFriendName(String nameOut);
        }
    }
}