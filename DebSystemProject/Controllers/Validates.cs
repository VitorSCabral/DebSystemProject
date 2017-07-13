﻿using System;


namespace DebSystemProject.Controllers
{
    public class Validates
    {
        public String ValidateFriend(FriendViewModel friendAttribute)
        {
            String Message = String.Empty;

            if (friendAttribute == null)
            {
                Message = "Não pode ser enviado amigo em branco. /n ";
            }
            if (friendAttribute.Name == "")
            {
                Message = "Campo nome não pode estar em branco. /n ";
            }
            if (friendAttribute.Name.Length > 100)
            {
                Message = "Campo Name não deve possuir mais de 100 caracteres. /n ";
            }
            if (friendAttribute.Sex == "")
            {
                Message = String.Concat(Message, "Campo sexo não pode estar em branco. /n ");
            }

            if (friendAttribute.Sex.ToUpper() != "M" && friendAttribute.Sex.ToUpper() != "F")
            {
                Message = String.Concat(Message, "Campo sexo valores aceitos M ou F. /n ");
            }
            if (friendAttribute.Age == "")
            {
                Message = String.Concat(Message, "Campo idade não pode estar em branco. /n ");
            }

            int number;
            bool Test = Int32.TryParse(friendAttribute.Age, out number);

            if (!Test)
            {
                Message = String.Concat(Message, "Campo idade deve ser inteiro /n ");
            }

            return Message;

        }
        
        public String ValidateUpdateFriend(FriendViewModel friendAttribute, int friendId)
        {
            String Message = String.Empty;
            String testId = friendId.ToString();

            if (testId == "")
            {
                Message = "Id deve estar preenchido. /n ";
            }
            if (testId.Length <= 0)
            {
                Message = String.Concat(Message, "Id deve ser maior que 0.");
            }

            int number;
            bool TestId = Int32.TryParse(testId, out number);

            if (!TestId)
            {
                Message = String.Concat(Message, "Campo Id deve ser inteiro /n ");
            }
            else if (Message == "")
            {
                if (friendAttribute == null)
                {
                    Message = String.Concat(Message, "Não pode ser enviado amigo em branco. /n ");
                }
                if (friendAttribute.Name.Length > 100)
                {
                    Message = String.Concat(Message, "Campo Name não deve possuir mais de 100 caracteres. /n ");
                }
                if (friendAttribute.Sex.ToUpper() != "M" && friendAttribute.Sex.ToUpper() != "F" && friendAttribute.Sex.Trim() != "")
                {
                    Message = String.Concat(Message, "Campo sexo valores aceitos M ou F. /n ");
                }
                number = 0;
                bool Test = Int32.TryParse(friendAttribute.Age, out number);

                if (!Test)
                {
                    Message = String.Concat(Message, "Campo idade deve ser inteiro /n ");
                }                
            }
            return Message;
        }
        
        public String ValidateDelete(int Id)
        {
            String Message = String.Empty;
            String testId = Id.ToString();

            if (testId == "")
            {
                Message = "Id deve estar preenchido. /n ";
            }
            if (testId.Length <= 0)
            {
                Message = String.Concat(Message, "Id deve ser maior que 0.");
            }

            int number;
            bool Test = Int32.TryParse(testId, out number);

            if (!Test)
            {
                Message = String.Concat(Message, "Campo Id deve ser inteiro /n ");
            }
            return Message;
        }
    }
}