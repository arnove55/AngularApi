namespace AngularApi.Models
{
    public class Emailmodel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Emailmodel(string to,string subject,string content) 
        {
            To = to;
            Subject = subject;
            Content = content;
        
        }
    }
}
