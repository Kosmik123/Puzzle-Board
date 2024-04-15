using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Bipolar.PuzzleBoard
{
    public interface IPiecesIndexable // i want to remove that
    {
        PieceComponent this[Vector2Int coord] { get; set; }
    }

    public delegate void PieceCoordChangeEventHandler(PieceComponent piece, Vector2Int newCoord);

    [DisallowMultipleComponent, RequireComponent(typeof(IBoardComponent), typeof(BoardCollapseController<,>))]
    public class BoardController : MonoBehaviour
    {
        public event System.Action OnPiecesColapsed
        {
            add => BoardCollapseController.OnPiecesColapsed += value;
            remove
            {
                if (BoardCollapseController)
                    BoardCollapseController.OnPiecesColapsed -= value;
            }
        }

        [SerializeField]
        protected DefaultPiecesMovementManager piecesMovementManager;
        public PiecesMovementManager PiecesMovementManager => piecesMovementManager;

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

        public bool ArePiecesMoving => piecesMovementManager.ArePiecesMoving;

        public bool IsCollapsing => BoardCollapseController.IsCollapsing;

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

        private BoardControllerPiecesIndexable piecesIndexable;
        private IPiecesIndexable Pieces
        {
            get
            {
                piecesIndexable ??= new BoardControllerPiecesIndexable(
                    getFunction: (coord) => BoardComponent.GetPiece(coord),
                    setFunction: (coord, pieceComponent) =>
                    {
                        if (pieceComponent)
                            piecesMovementManager.StartPieceMovement(pieceComponent, coord);
                        BoardComponent.Board[coord] = pieceComponent.Piece;
                    });
                return piecesIndexable;
            }
        }

        protected virtual void Awake()
        {
            _boardComponent = GetComponent<BoardComponent>();
        }

        protected virtual void Start()
        {
            Profiler.BeginSample("Start Example");
            if (collapseOnStart)
                Collapse();
            Profiler.EndSample();
        }

        private readonly LinkedList<Vector2Int> shuffledCoords = new LinkedList<Vector2Int>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShufflePieces();
            }
        }

        public void ShufflePieces()
        {
            shuffledCoords.Clear();
            foreach (var coord in BoardComponent)
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

            foreach (var coord in BoardComponent)
            {
                var randomCoord = shuffledCoords.First;
                shuffledCoords.RemoveFirst();
                (Pieces[randomCoord.Value], Pieces[coord]) = (Pieces[coord], Pieces[randomCoord.Value]);
            }
        }

        public class BoardControllerPiecesIndexable : IPiecesIndexable
        {
            private readonly System.Func<Vector2Int, PieceComponent> getFunction;
            private readonly System.Action<Vector2Int, PieceComponent> setFunction;

            public BoardControllerPiecesIndexable(System.Func<Vector2Int, PieceComponent> getFunction,
                System.Action<Vector2Int, PieceComponent> setFunction)
            {
                this.getFunction = getFunction;
                this.setFunction = setFunction;
            }

            public PieceComponent this[Vector2Int coord]
            {
                get => getFunction(coord);
                set => setFunction(coord, value);
            }
        }
    }
}
