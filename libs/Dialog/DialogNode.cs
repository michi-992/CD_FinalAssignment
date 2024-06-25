using System.Data.Common;
using System.Net;

namespace libs;

public class DialogNode {

    public string text;
    public List<Response> responses = new List<Response>();

    public DialogNode (string text) {
        this.text = text;
    }

    public void AddResponse(string responseText, DialogNode? nextNode)
    {
        this.responses.Add(new Response(responseText, nextNode));
    }
}