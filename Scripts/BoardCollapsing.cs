using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class BoardCollapsing : MonoBehaviour
    {
        public abstract event System.Action OnPiecesColapsed;

        public abstract bool IsCollapsing { get; }
        public abstract void Collapse();
    }

    [DisallowMultipleComponent, RequireComponent(typeof(Board<>))]
    public abstract class BoardCollapsing<TBoard> : BoardCollapsing
        where TBoard : IModifiableBoard
    {
        [SerializeField]
        private PiecesSpawner piecesSpawner;
        public PiecesSpawner PiecesSpawner
        {
            get => piecesSpawner;
            set => piecesSpawner = value;
        }

        [SerializeField]
        private PieceTypeProvider pieceTypeProvider;
        public PieceTypeProvider PieceTypeProvider
        {
            get => pieceTypeProvider;
            set => pieceTypeProvider = value;
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

        protected Piece CreatePiece(Vector2Int coord)
        {
            var piece = PiecesSpawner.SpawnPiece();
            piece.Type = PieceTypeProvider.GetPieceType(coord.x, coord.y);
            Board[coord] = piece;
            return piece;
        }
    }
}
