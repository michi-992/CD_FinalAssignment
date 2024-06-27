using System;
using System.Collections.Generic;
using System.Linq;

namespace libs
{
    public class TutorialDialog
    {
        private List<TutorialDialogNode> tutorialDialogNodes;

        public TutorialDialog()
        {
            LoadTutorialDialogNodes();
        }

        private void LoadTutorialDialogNodes()
        {
            dynamic jsonData = FileHandler.ReadTutorialJson();
            tutorialDialogNodes = jsonData.ToObject<List<TutorialDialogNode>>();
        }

        public void ShowTutorial()
        {
            int currentId = 0;
            while (true)
            {
                Console.Clear();
                var currentNode = tutorialDialogNodes.FirstOrDefault(node => node.Id == currentId);

                Console.WriteLine("Hexoban");
                Console.WriteLine("");
                Console.WriteLine("Welcome, young witch apprentice!");
                Console.WriteLine("Enter the number of the tutorial step you want to go to (1-8), press Enter, or press '0' to exit.");
                Console.WriteLine("1: Goal. 2: Movement. 3: Warning. 4: Undo. 5: NPCs. 6: Restart. 7: Main Menu. 8: Farewell.");
                Console.WriteLine("");

                if (currentNode != null)
                {
                    Console.WriteLine(currentNode.Text);
                }

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
            Console.Clear();
        }
    }

    public class TutorialDialogNode
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
