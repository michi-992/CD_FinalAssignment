namespace libs;

public class Dialog
{  
    public DialogNode currentNode;
    public DialogNode startNode;

    public Dialog (DialogNode startNode)
    {
        this.startNode = startNode;
        this.currentNode = startNode;
    }

    public void Start()
    {
        //(int x, int y) = Console.GetCursorPosition();
        //TODO Clear Buffer of Console to overwrite the nodes
        while (this.currentNode != null) {
            Console.WriteLine(this.currentNode.text);

            if (this.currentNode.responses == null || this.currentNode.responses.Count == 0) {
                break;
            }

            for (int i = 0; i < this.currentNode.responses.Count; i++) {
                Console.WriteLine($"{i + 1}.{this.currentNode.responses[i].text}");
            }

            int choice;
            while (true)
            {
                Console.Write("Choose an option: ");
                if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= this.currentNode.responses.Count)
                {
                    break;
                }
                Console.WriteLine("Invalid choice, please try again.");
            }
            this.currentNode = this.currentNode.responses[choice - 1].nextNode;
            GameEngine.Instance.Render();
        }
        Thread.Sleep(2000);
    }
}