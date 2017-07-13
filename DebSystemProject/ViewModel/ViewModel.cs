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
}