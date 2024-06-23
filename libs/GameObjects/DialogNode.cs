using System.Collections.Generic;

namespace libs
{
    public class DialogNode
{
    public string Text { get; set; }
    public List<Response> Responses { get; set; }

    public DialogNode(string text, List<Response> responses)
    {
        Text = text;
        Responses = responses;
    }

    public DialogNode(string text)
    {
        Text = text;
        Responses = new List<Response>();
    }

    public void AddResponse(string responseText, DialogNode nextNode)
    {
        Responses.Add(new Response(responseText, nextNode));
    }
}

}
