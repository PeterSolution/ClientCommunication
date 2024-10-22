

namespace ServerApi.Models
{
    public class NotificationDbModel
    {
        public int id { get; set; }
        public int iddata { get; set; }
        public int idduser { get; set; }
        public string date { get; set; }
        public bool isseen { get; set; }
    }
}
