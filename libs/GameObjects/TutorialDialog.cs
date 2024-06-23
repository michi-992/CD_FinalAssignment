using System;
using System.Collections.Generic;
using System.Threading;

namespace libs
{
    public class TutorialDialog
    {
        private DialogNode _currentNode;
        private DialogNode _startingNode;
        private DialogNode _endNode;

        private DialogNode tutorialStartNode;

        public TutorialDialog()
        {
            // Initialize tutorial nodes
            InitializeNodes();

            _startingNode = tutorialStartNode;
            _currentNode = _startingNode;

            // End node to handle end of tutorial
            _endNode = new DialogNode("End of tutorial. What would you like to do now?")
            {
                Responses = new List<Response>
                {
                    new Response("Continue to the game", null)
                }
            };
        }

        private void InitializeNodes()
        {
            // First part of tutorial
            tutorialStartNode = new DialogNode("Welcome to the Sokoban tutorial!\n\n" +
                                               "In Sokoban, your goal is to push all the boxes onto the targets.");

            DialogNode secondNode = new DialogNode("You control the player character using arrow keys." +
                                                "Use Z to undo a move, R to reset the game, S to save and L to load.");

            DialogNode thirdNode = new DialogNode("You can only push one box at a time. " +
                                                  "Boxes can be pushed horizontally or vertically, but not diagonally.");

            DialogNode fourthNode = new DialogNode("Boxes cannot be pulled, so plan your moves carefully!");

            tutorialStartNode.AddResponse("Next", secondNode);
            secondNode.AddResponse("Next", thirdNode);
            thirdNode.AddResponse("Next", fourthNode);
            fourthNode.AddResponse("Continue to game", _endNode);
            fourthNode.AddResponse("Restart Tutorial", tutorialStartNode);
        }

        public void StartTutorial()
        {
            Console.Clear();
            while (_currentNode != null)
            {
                Console.WriteLine(_currentNode.Text);
                for (int i = 0; i < _currentNode.Responses.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_currentNode.Responses[i].ResponseText}");
                }

                if (_currentNode.Responses.Count == 0)
                    break;

                int choice;
                while (true)
                {
                    Console.Write("Choose an option: ");
                    if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= _currentNode.Responses.Count)
                    {
                        break;
                    }
                    Console.WriteLine("Invalid choice, please try again.");
                }

                _currentNode = _currentNode.Responses[choice - 1].NextNode;
            }

            _currentNode = _endNode; // Set current node to end node

            Console.WriteLine("End of tutorial dialog.");
            Thread.Sleep(2000);
            Console.Clear();
        }

        public bool ContinueToGame
        {
            get { return _currentNode == null || _currentNode == _endNode; }
        }
    }
}
