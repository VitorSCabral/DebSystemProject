using System.Data.Entity.Validation;
using System;
using System.Linq;
using System.Data.Entity;
using static DebSystemProject.Models.Repository.FriendRepository;

namespace DebSystemProject.Models.Repository
{
    public class FriendRepository: IFriendRepository
    {
        // Cadastra um amigo na tabela
        public Boolean Insert(Friend friend)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();                
                
                connection.dateBase.Friends.Add(friend);
                connection.dateBase.SaveChanges();

                return true;
            }
            catch (DbEntityValidationException e)
            {
                return false;
                throw e;
            }
        }

        // Busca um amigo por id

        public Friend GetFriendById(int friendId)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();

                var friendObj = from Friend in connection.dateBase.Friends
                                 where Friend.Id == friendId
                                select Friend;

                if (friendObj.Count() > 0)
                {
                    return friendObj.First();
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

        // Atualiza o amigo na tabela.
        public Boolean Update(Friend friend, int friendId)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();

                FriendRepository friendRepository = new FriendRepository();
                Friend updateFriend = friendRepository.GetFriendById(friendId);

                if (friend.Name != "")
                {
                    updateFriend.Name = friend.Name.Trim();
                }
                if (friend.Sex != "")
                {
                    updateFriend.Sex = friend.Sex;
                }
                if (friend.Age > 0)
                {
                    updateFriend.Age = Convert.ToInt32(friend.Age);
                }

                connection.dateBase.Friends.Attach(updateFriend);
                connection.dateBase.Entry(updateFriend).State = EntityState.Modified;
                connection.dateBase.SaveChanges();

                return true;
            }
            catch (DbEntityValidationException e)
            {
                return false;
                throw e;
            }
        }

        //Deleta o amigo da tabela (físicamente).
        public Boolean DeleteObject(Friend friend, bool commit = false)
        {
            try
            {
                Connection connection = Connection.Instance;

                if (friend != null)
                {
                    connection.dateBase.Debts.RemoveRange(friend.Debts);
                    connection.dateBase.Debts.RemoveRange(friend.Debts1);
                    connection.dateBase.Payments.RemoveRange(friend.Payments);
                    connection.dateBase.Payments.RemoveRange(friend.Payments1);
                    connection.dateBase.Friends.Attach(friend);
                    connection.dateBase.Friends.Remove(friend);

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

        // Busca um amigo por nome
        public Friend GetFriendByName(String name)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();

                var friendObj = from friend in connection.dateBase.Friends
                                where friend.Name == name
                                select friend;

                if (friendObj.Count() > 0)
                {
                    return friendObj.First();
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

        // Busca o ID de um amigo pelo o nome
        public int GetFriendIdByName(String name)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();

                var friendObj = from friend in connection.dateBase.Friends
                                where friend.Name == name
                                select friend;

                if (friendObj.Count() > 0)
                {
                    return friendObj.ToList()[0].Id;
                }
                else
                {
                    return 0;
                }

            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
        }

        // Busca o nome de um amigo pelo o ID
        public String GetFriendNameById(int? friendId)
        {
            try
            {
                Connection connection = Connection.Instance;
                connection.dateBase = new DebSystemDatabaseEntities();

                var friendObj = from friend in connection.dateBase.Friends
                                where friend.Id == friendId
                                select friend;

                if (friendObj.Count() > 0)
                {
                    return friendObj.ToList()[0].Name;
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

        public interface IFriendRepository
        {            
            Boolean Insert(Friend friend);
            Friend GetFriendById(int friendId);
            Boolean Update(Friend friend, int friendId);
            Boolean DeleteObject(Friend friend, bool commit = false);
            Friend GetFriendByName(String name);
            int GetFriendIdByName(String name);
            String GetFriendNameById(int? friendId);            
        }       
    }
}