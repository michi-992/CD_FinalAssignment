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
            tutorialDialogNodes = new List<TutorialDialogNode>
            {
                new TutorialDialogNode { Id = 1, Text = "1. In Sokoban, your goal is to push all the magical crates onto the glowing stars." },
                new TutorialDialogNode { Id = 2, Text = "2. Use your arcane powers to move your character with the arrow keys on your enchanted broomstick." },
                new TutorialDialogNode { Id = 3, Text = "3. Beware! Once a crate is pushed, it cannot be pulled back. Plan your moves carefully." },
                new TutorialDialogNode { Id = 4, Text = "4. Press 'Z' to undo your last move if you need to rethink your strategy." },
                new TutorialDialogNode { Id = 5, Text = "5. Approach NPCs, solve their riddles, and you can save your process." },
                new TutorialDialogNode { Id = 6, Text = "6. If you ever get stuck, press 'R' to restart the challenge." },
                new TutorialDialogNode { Id = 7, Text = "7. When you wish to return to the main menu, press 'M'." },
                new TutorialDialogNode { Id = 8, Text = "8. May the magical spirits guide you. Press '0' to return to the main menu and begin your journey!" }
            };
        }

        public void ShowTutorial()
        {
            int currentId = 0;
            while (true)
            {
                Console.Clear();
                var currentNode = tutorialDialogNodes.FirstOrDefault(node => node.Id == currentId);

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
