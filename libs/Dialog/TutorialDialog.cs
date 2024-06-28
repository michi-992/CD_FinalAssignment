using System;
using System.Collections.Generic;
using System.Linq;

namespace libs
{
    // class responsible for the tutorial dialog
    public class TutorialDialog
    {
        // list of nodes in the tutorial dialog
        private List<TutorialDialogNode> tutorialDialogNodes;

        // constructor initializing the tutorial dialog
        public TutorialDialog()
        {
            LoadTutorialDialogNodes();
        }

        // method to load the tutorial dialog nodes from JSON
        private void LoadTutorialDialogNodes()
        {
            dynamic jsonData = FileHandler.ReadTutorialJson();
            tutorialDialogNodes = jsonData.ToObject<List<TutorialDialogNode>>();
        }

        // method to display the tutorial dialog
        public void ShowTutorial()
        {
            int currentId = 0;

            // loop until the user presses 0 + Enter to exit the tutorial
            while (true)
            {
                // clear the console and display game title and instructions
                Console.Clear();
                Console.WriteLine("HEXOBAN");
                Console.WriteLine("");
                Console.WriteLine("Welcome, young witch apprentice!");
                Console.WriteLine("Enter the number of the tutorial step you want to go to (1-8), press Enter, or press '0' to exit.");
                Console.WriteLine("1: Goal. 2: Movement. 3: Warning. 4: Undo. 5: NPCs. 6: Restart. 7: Main Menu. 8: Farewell.");
                Console.WriteLine("");

                // using LINQ to query 
                var currentNode = tutorialDialogNodes.FirstOrDefault(node => node.Id == currentId);

                // diaplay the current tutorial node's text if it exists
                if (currentNode != null)
                {
                    Console.WriteLine(currentNode.Text);
                }

                // get user input for the tutorial step to navigate to (using LINQ) or exit
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == 0)
                    {
                        break;
                    }
                    else if (tutorialDialogNodes.Any(node => node.Id == choice))
                    {
                        currentId = choice;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice, please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input, please enter a number.");
                }
            }
            // clear console after exiting the tutorial
            Console.Clear();
        }
    }
}

// class representing a node in the tutorial dialog
public class TutorialDialogNode
{
    public int Id { get; set; } // id of tutorial node for querying
    public string Text { get; set; } // textual content of tutorial node
}