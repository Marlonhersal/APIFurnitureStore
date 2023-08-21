using Org.BouncyCastle.Asn1.Crmf;

namespace APIFurnitureStoreAPI.Configuration
{
    public class SmtpSettings
    {
        public string Sever { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
