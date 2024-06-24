using System.Collections.Generic;

namespace libs
{
    public class DialogNode
{
    public string Text { get; set; }
    public List<Response> Responses { get; set; }
    public Action Action { get; set; }

    public DialogNode(string text, List<Response> responses, Action action = null)
    {
        Text = text;
        Responses = responses;
        Action = action;
    }

    public DialogNode(string text)
    {
        Text = text;
        Responses = new List<Response>();
    }

    public void AddResponse(string responseText, DialogNode nextNode, Action action = null)
    {
        Responses.Add(new Response(responseText, nextNode));
        if (action != null)
        {
            nextNode.Action = action;
        }
    }
}

}
