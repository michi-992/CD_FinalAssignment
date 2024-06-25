namespace libs;

public class Response {
    public string text;
    public DialogNode? nextNode;

    public Response(string text, DialogNode? nextNode)
    {
        this.text = text;
        this.nextNode = nextNode;
    }
}