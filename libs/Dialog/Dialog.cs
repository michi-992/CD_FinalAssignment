namespace libs;

// class representing a dialog consisting of multiple dialog nodes (dialog tree)
public class Dialog
{  
    
    public DialogNode currentNode;
    public DialogNode startNode;

    // Constructor initializing the dialog with a starting node
    public Dialog (DialogNode startNode)
    {
        this.startNode = startNode;
        this.currentNode = startNode;
    }

    // Method to start the dialog
    public void Start()
    {
        // loop until the dialog reaches an end node (no responses left)
        while (this.currentNode != null) {

            // clear console and display game title
            Console.Clear();
            Console.WriteLine("HEXOBAN");
            Console.WriteLine();
            
            //display the current node's text
            Console.WriteLine(this.currentNode.text);

            // no responses left -> break the loop
            if (this.currentNode.responses == null || this.currentNode.responses.Count == 0) {
                break;
            }

            // display available responses
            for (int i = 0; i < this.currentNode.responses.Count; i++) {
                Console.WriteLine($"{i + 1}.{this.currentNode.responses[i].text}");
            }

            int choice;
            while (true)
            {
                // ask user to choose an option, if invalid, ask again
                Console.Write("Choose an option: ");
                if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= this.currentNode.responses.Count)
                {
                    break;
                }
                Console.WriteLine("Invalid choice, please try again.");
            }

            // jump to next node based on the user choice
            this.currentNode = this.currentNode.responses[choice - 1].nextNode;
        }

        // pasue for 3 seconds before closing the ending dialog
        Thread.Sleep(3000);
    }
}