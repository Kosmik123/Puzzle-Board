using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IBoardCommandsInvoker
    {
        void RequestCommand(IBoardCommand command);
    }

    public class BoardController : MonoBehaviour, IBoardCommandsInvoker
    {
        public bool IsBusy => currentlyExecutedCommand != null && commandsQueue.Count > 0;

        private readonly Queue<IBoardCommand> commandsQueue = new Queue<IBoardCommand>();
        private IBoardCommand currentlyExecutedCommand = null;

        [field: SerializeField]
#if NAUGHTY_ATTRIBUTES
        [field:NaughtyAttributes.ReadOnly]
#endif
        public int CommandsCount { get; private set; }

        public void RequestCommand(IBoardCommand command)
        {
            commandsQueue.Enqueue(command);
            DisplayCommandsQueue();
        }

        private void DisplayCommandsQueue()
        {
            string message = $"Queue [{commandsQueue.Count}]:\n";
            foreach (var c in commandsQueue)
                message += $"\t{c}\n";

            Debug.Log(message);
        }

        private void Update()
        {
            CommandsCount = commandsQueue.Count;
            if (currentlyExecutedCommand == null && commandsQueue.Count > 0)
            {
                var command = commandsQueue.Dequeue();
                StartCoroutine(ExecuteCommand(command));
                DisplayCommandsQueue();
            }
        }

        private IEnumerator ExecuteCommand(IBoardCommand command)
        {
            currentlyExecutedCommand = command;
            yield return command.Execute();
            if (command is IDisposable disposableCommand)
                disposableCommand.Dispose();

            currentlyExecutedCommand = null;
        }
    }

    public readonly struct ShuffleBoardCommand : IBoardCommand
    {
        private readonly IBoard board;
        private readonly BoardShuffler boardShuffler;

        public ShuffleBoardCommand(IBoard board, BoardShuffler boardShuffler)
        {
            this.board = board;
            this.boardShuffler = boardShuffler;
        }

        public IEnumerator Execute()
        {
            boardShuffler.Shuffle(board);
            yield return null;
        }
    }
}
