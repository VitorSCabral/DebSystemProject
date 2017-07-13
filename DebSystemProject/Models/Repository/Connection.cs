namespace DebSystemProject.Models
{
    public sealed class Connection
    {
        private static readonly Connection instance = new Connection();

        public DebSystemDatabaseEntities dateBase { get; set; }
        private Connection() { }

        public static Connection Instance
        {
            get
            {
                return instance;
            }
        }
    }
}