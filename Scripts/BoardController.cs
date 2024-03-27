using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IPiecesIndexable
    {
        Piece this[Vector2Int coord] { get; set; }
    }

    public delegate void PieceCoordChangeEventHandler(Piece piece, Vector2Int newCoord);

    [DisallowMultipleComponent, RequireComponent(typeof(IModifiableBoard), typeof(BoardCollapsing<>))]
    public class BoardController : MonoBehaviour
    {
        public event System.Action OnPiecesColapsed
        {
            add => BoardCollapsing.OnPiecesColapsed += value;
            remove
            {
                if (BoardCollapsing)
                    BoardCollapsing.OnPiecesColapsed -= value;
            }
        }

        [SerializeField]
        protected DefaultPiecesMovementManager piecesMovementManager;
        public PiecesMovementManager PiecesMovementManager => piecesMovementManager;

        private BoardCollapsing _boardCollapsing;
        public BoardCollapsing BoardCollapsing
        {
            get
            {
                if (_boardCollapsing == null && Board != null)
                    _boardCollapsing = Board.GetComponent<BoardCollapsing>();
                return _boardCollapsing;
            }
        }

        public bool ArePiecesMoving => piecesMovementManager.ArePiecesMoving;

        public bool IsCollapsing => BoardCollapsing.IsCollapsing;
        public void Collapse() => BoardCollapsing.Collapse();

        protected Board _board;
        public Board Board
        {
            get
            {
                if (_board == null)
                    _board = GetComponent<Board>();
                return _board;
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
        public IPiecesIndexable Pieces
        {
            get
            {
                piecesIndexable ??= new BoardControllerPiecesIndexable(
                    getFunction: (coord) => Board[coord],
                    setFunction: (coord, piece) =>
                    {
                        if (piece)
                            piecesMovementManager.StartPieceMovement(piece, coord);
                        Board[coord] = piece;
                    });
                return piecesIndexable;
            }
        }

        protected virtual void Awake()
        {
            _board = GetComponent<Board>();
        }

        protected virtual void Start()
        {
            if (collapseOnStart)
                Collapse();
        }

        public class BoardControllerPiecesIndexable : IPiecesIndexable
        {
            private readonly System.Func<Vector2Int, Piece> getFunction;
            private readonly System.Action<Vector2Int, Piece> setFunction;

            public BoardControllerPiecesIndexable(System.Func<Vector2Int, Piece> getFunction,
                System.Action<Vector2Int, Piece> setFunction)
            {
                this.getFunction = getFunction;
                this.setFunction = setFunction;
            }

            public Piece this[Vector2Int coord]
            {
                get => getFunction(coord);
                set => setFunction(coord, value);
            }
        }
    }
}
