using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    public delegate void PieceCoordChangeEventHandler(PieceComponent piece, Vector2Int newCoord);


    [DisallowMultipleComponent, RequireComponent(typeof(IBoardComponent), typeof(BoardCollapseController<,>))]
    public class BoardController : MonoBehaviour
    {
        private BoardCollapseController _boardCollapseController;
        public BoardCollapseController BoardCollapseController
        {
            get
            {
                if (_boardCollapseController == null && BoardComponent != null)
                    _boardCollapseController = BoardComponent.GetComponent<BoardCollapseController>();
                return _boardCollapseController;
            }
        }

        public bool IsBusy { get; private set; }

        [ContextMenu("Collapse")]
        public void Collapse() => BoardCollapseController.Collapse();

        protected BoardComponent _boardComponent;
        public BoardComponent BoardComponent
        {
            get
            {
                if (_boardComponent == null && this)
                    _boardComponent = GetComponent<BoardComponent>();
                return _boardComponent;
            }
        }

        [SerializeField]
        private bool collapseOnStart;
        public bool CollapseOnStart
        {
            get => collapseOnStart;
            set => collapseOnStart = value;
        }

        [SerializeField]
        private bool collapseConstantly;
        public bool CollapseConstantly
        {
            get => collapseConstantly;
            set => collapseConstantly = value;
        }

        private readonly Queue<IBoardCommand> commandsQueue = new Queue<IBoardCommand>();
        private IBoardCommand currentlyExecutedCommand = null;

        protected virtual void Awake()
        {
            _boardComponent = GetComponent<BoardComponent>();
        }

        protected virtual void Start()
        {
            if (collapseOnStart)
                Collapse();
        }

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
            
            if (collapseConstantly)
                Collapse();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShufflePieces();
            }
        }

        private IEnumerator ExecuteCommand(IBoardCommand command)
        {
            currentlyExecutedCommand = command;
            yield return command.Execute();
            currentlyExecutedCommand = null;
        }

        private readonly LinkedList<Vector2Int> shuffledCoords = new LinkedList<Vector2Int>();
        public void ShufflePieces()
        {
            shuffledCoords.Clear();
            foreach (var coord in BoardComponent.Board)
            {
                if (Random.value > 0.5f)
                {
                    shuffledCoords.AddFirst(coord);
                }
                else
                {
                    shuffledCoords.AddLast(coord);
                }
            }

            foreach (var coord in BoardComponent.Board)
            {
                var randomCoord = shuffledCoords.First;
                shuffledCoords.RemoveFirst();
                // (Pieces[randomCoord.Value], Pieces[coord]) = (Pieces[coord], Pieces[randomCoord.Value]);
            }
        }
    }
}
