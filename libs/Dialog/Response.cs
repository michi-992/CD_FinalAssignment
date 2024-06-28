namespace libs;

// class representing a response in the dialog
public class Response {
    // the textual content of the response
    public string text;

    // the dialog node it goes to after choosing the response
    public DialogNode? nextNode;

    // constructor initializing the response with text and next node 
    public Response(string text, DialogNode? nextNode)
    {
        this.text = text;
        this.nextNode = nextNode;
    }
}