namespace ChatGptApi.Dto
{
    public class ChatGptResponse
    {
        public List<ChatGptChoice> choices { get; set; }
    }

    public class ChatGptChoice
    {
        public string text { get; set; }
    }
}
