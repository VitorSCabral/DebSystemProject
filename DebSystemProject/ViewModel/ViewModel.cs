using System;


namespace DebSystemProject.Controllers
{    
    public class MessageViewModel
    {
        /// <summary>Código da operação.</summary>
        public String Code;
        /// <summary>Status da operação.</summary>
        public String Action;              

        public MessageViewModel Create()
        {
            if (true)
            {
                this.Code = "201";
                this.Action = "Sucess";                
            }                     
            return this; 
        }
    }

    public class FriendViewModel
    {
        /// <summary>Id do amigo.</summary>
        public int Id; 
        /// <summary>Nome do amigo.</summary>
        public String Name;
        /// <summary>Sexo com uma letra (F ou M).</summary>
        public String Sex; 
        /// <summary>Idade do amigo.</summary>
        public String Age; 
    }

    public class DebtViewModel
    {
        /// <summary>Id do débito.</summary>
        public int? Id;
        /// <summary>Valor da dívida.</summary>
        public String Value;
        /// <summary>Data da dívida.</summary>
        public String Date;
        /// <summary>Descrição da dívida.</summary>
        public String Description;
        /// <summary>Nome do credor.</summary>
        public String FriendNameIn;
        /// <summary>Nome do devedor.</summary>
        public String FriendNameOut;
    }

    public class PaymentViewModel
    {
        /// <summary>Id do pagamento.</summary>
        public int? Id;
        /// <summary>Valor do pagamento.</summary>
        public String Value;
        /// <summary>Data da realização do pagamento.</summary>
        public String Date;
        /// <summary>Nome do devedor.</summary>
        public String FriendNameOut;
        /// <summary>Nome do credor.</summary>
        public String FriendNameIn;
    }
}