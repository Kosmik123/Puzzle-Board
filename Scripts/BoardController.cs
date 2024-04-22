using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class BoardController : MonoBehaviour
    {
        public bool IsBusy { get; private set; }

        private readonly Queue<IBoardCommand> commandsQueue = new Queue<IBoardCommand>();
        private IBoardCommand currentlyExecutedCommand = null;

        public void RequestCommand(IBoardCommand command)
        {
            commandsQueue.Enqueue(command);
        }

        private void DisplayCommandsQueue()
        {
            string message = "Queue:\n";
            foreach (var c in commandsQueue)
                message += $"\t{c}\n";

            Debug.Log(message);
        }

        private void Update()
        {
            if (currentlyExecutedCommand == null && commandsQueue.Count > 0)
            {
                var command = commandsQueue.Dequeue();
                StartCoroutine(ExecuteCommand(command));
            }
        }

        private IEnumerator ExecuteCommand(IBoardCommand command)
        {
            currentlyExecutedCommand = command;
            yield return command.Execute();
            currentlyExecutedCommand = null;
        }
    }
}
