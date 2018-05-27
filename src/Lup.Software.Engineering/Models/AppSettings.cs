namespace Lup.Software.Engineering.Models
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
    }

    public class ConnectionStrings
    {
        public string TableStorage { get; set; }
    }

}
