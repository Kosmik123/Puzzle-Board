using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IPiecesIndexable
    {
        PieceComponent this[Vector2Int coord] { get; set; }
    }

    public delegate void PieceCoordChangeEventHandler(PieceComponent piece, Vector2Int newCoord);

    [DisallowMultipleComponent, RequireComponent(typeof(IBoardComponent), typeof(BoardCollapsing<>))]
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
                if (_boardCollapsing == null && BoardComponent != null)
                    _boardCollapsing = BoardComponent.GetComponent<BoardCollapsing>();
                return _boardCollapsing;
            }
        }

        public bool ArePiecesMoving => piecesMovementManager.ArePiecesMoving;

        public bool IsCollapsing => BoardCollapsing.IsCollapsing;
        public void Collapse() => BoardCollapsing.Collapse();

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
            if (collapseOnStart)
                Collapse();
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
