using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class BoardCollapsing : MonoBehaviour
    {
        public abstract event System.Action OnPiecesColapsed;

        public abstract bool IsCollapsing { get; }
        public abstract void Collapse();
    }

    [DisallowMultipleComponent, RequireComponent(typeof(BoardComponent<>))]
    public abstract class BoardCollapsing<TBoard> : BoardCollapsing
        where TBoard : IBoardComponent
    {
        [SerializeField]
        private PiecesSpawner piecesSpawner;
        public PiecesSpawner PiecesSpawner
        {
            get => piecesSpawner;
            set => piecesSpawner = value;
        }

        private TBoard _board;
        public TBoard Board
        {
            get
            {
                if (_board == null)
                    _board = GetComponent<TBoard>();
                return _board;
            }
        }

        protected PieceComponent CreatePiece(Vector2Int coord)
        {
            var piece = PiecesSpawner.SpawnPiece(coord.x, coord.y);
            Board.AddPiece(piece);
            return piece;
        }
    }
}
