namespace libs
{
    public class Response
    {
        public string ResponseText { get; set; }
        public DialogNode NextNode { get; set; }
        public Action Action { get; set; }

        public Response(string responseText, DialogNode nextNode, Action action = null)
        {
            ResponseText = responseText;
            NextNode = nextNode;
            Action = action;
        }
    }
}
