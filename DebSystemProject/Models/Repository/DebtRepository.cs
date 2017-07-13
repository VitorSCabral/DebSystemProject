using System.Data.Entity.Validation;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using static DebSystemProject.Models.Repository.DebtRepository;

namespace DebSystemProject.Models.Repository
{
    public class DebtRepository : IDebtRepository
    {
        // Cadastra uma dívida da tabela
        public Boolean Insert(Debt debt)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();

                debt.Value = Convert.ToDecimal(debt.Value);
                debt.Date = Convert.ToDateTime(debt.Date);
                debt.Description = debt.Description;
                debt.FriendIdIn = debt.FriendIdIn;
                debt.FriendIdOut = debt.FriendIdOut;

                connection.dateBase.Debts.Attach(debt);
                connection.dateBase.Debts.Add(debt);
                connection.dateBase.SaveChanges();

                return true;
            }
            catch (DbEntityValidationException e)
            {
                return false;
                throw e;
            }
        }

        // Busca uma dívida por id
        public Debt GetDebtById(int debtId)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();

                var debtObj = from debt in connection.dateBase.Debts
                              where debt.Id == debtId
                              select debt;

                if (debtObj.Count() > 0)
                {
                    return debtObj.First();
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

        // Atualiza a dívida na tabela.
        public Boolean Update(Debt debt, int debtId)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();

                DebtRepository debtRepository = new DebtRepository();
                Debt updateDebt = debtRepository.GetDebtById(debtId);

                if (debt.Value > 0)
                {
                    updateDebt.Value = debt.Value;
                }
                if (debt.Description != "")
                {
                    updateDebt.Description = debt.Description;
                }
                if (debt.FriendIdIn > 0)
                {
                    updateDebt.FriendIdIn = debt.FriendIdIn;
                }
                if (debt.FriendIdOut > 0)
                {
                    updateDebt.FriendIdOut = debt.FriendIdOut;
                }

                connection.dateBase.Debts.Attach(updateDebt);
                connection.dateBase.Entry(updateDebt).State = EntityState.Modified;
                connection.dateBase.SaveChanges();

                return true;
            }
            catch (DbEntityValidationException e)
            {
                return false;
                throw e;
            }
        }

        // Deleta a dívida da tabela (físicamente).
        public Boolean DeleteObject(Debt debt, bool commit = false)
        {
            try
            {
                Connection connection = Connection.Instance;

                if (debt != null)
                {
                    connection.dateBase.Debts.Attach(debt);
                    connection.dateBase.Debts.Remove(debt);

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

        // Busca o total de dívidas que os amigos tem para um determinado amigo
        public String GetTotalDebtsByFriendName(String friendNameIn)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();

                FriendRepository friendRepository = new FriendRepository();
                int friendIdIn = friendRepository.GetFriendIdByName(friendNameIn);

                var debObj = from debt in connection.dateBase.Debts
                             where debt.FriendIdIn == friendIdIn
                             select debt.Value;

                return Convert.ToString(debObj.Sum());
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
        }

        // Busca uma lista de dívidas ordenadas pro amigo
        public IList<Debt> GetDebtListByFriendName(String nameIn)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();

                var debtListObj = from debt in connection.dateBase.Debts
                                  join friendIn in connection.dateBase.Friends on debt.FriendIdIn equals friendIn.Id
                                  join friendOut in connection.dateBase.Friends on debt.FriendIdOut equals friendOut.Id
                                  where friendIn.Name == nameIn
                                  orderby friendIn.Name, friendOut.Name
                                  select debt;

                if (debtListObj.Count() > 0)
                {
                    return debtListObj.ToList();
                }
                return null;
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
        }

        // Busca a lista de dívidas que o amigo tem com os outros
        public IList<Debt> GetListMyDebtsByFriendName(String nameOut)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();

                var debtListObj = from debt in connection.dateBase.Debts
                                  join friendIn in connection.dateBase.Friends on debt.FriendIdIn equals friendIn.Id
                                  join friendOut in connection.dateBase.Friends on debt.FriendIdOut equals friendOut.Id
                                  where friendOut.Name == nameOut
                                  orderby friendIn.Name, friendOut.Name
                                  select debt;

                if (debtListObj.Count() > 0)
                {
                    return debtListObj.ToList();
                }
                return null;
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
        }

        public interface IDebtRepository
        {
            Boolean Insert(Debt debt);
            Debt GetDebtById(int debtId);
            Boolean Update(Debt debt, int debtId);
            Boolean DeleteObject(Debt debt, bool commit = false);
            String GetTotalDebtsByFriendName(String friendNameIn);
            IList<Debt> GetDebtListByFriendName(String nameIn);
            IList<Debt> GetListMyDebtsByFriendName(String nameOut);

        }
    }
}