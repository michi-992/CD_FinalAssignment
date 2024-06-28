using System.Data.Common;
using System.Net;

namespace libs;

// class representing a node in the dialog tree
public class DialogNode {

    // textual content of the dialog node
    public string text;

    // list of possible responses from the node
    public List<Response> responses = new List<Response>();

    public DialogNode (string text) {
        this.text = text; // contructor initializing the node with text
    }

    // method to add a response to the list of responses of the node
    public void AddResponse(string responseText, DialogNode? nextNode)
    {
        this.responses.Add(new Response(responseText, nextNode));
    }
}