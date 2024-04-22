using Bipolar.PuzzleBoard.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Grid))]
    public abstract class BoardComponent : MonoBehaviour, IBoardComponent
    {
        private Grid _grid;
        public Grid Grid
        {
            get
            {
                if (_grid == null)
                    _grid = GetComponent<Grid>();
                return _grid;
            }
        }

        protected readonly Dictionary<Piece, PieceComponent> pieceComponents = new Dictionary<Piece, PieceComponent>();

        public GridLayout.CellLayout Layout => Grid.cellLayout;

        public abstract IReadOnlyBoard Board { get; }

        public abstract bool ContainsCoord(Vector2Int coord);

        public Piece GetPiece(Vector2Int coord)
        {
            if (ContainsCoord(coord) == false)
                return null;

            var piece = Board[coord];
            if (piece == null || piece.IsCleared)
                return null;

            return piece;
        }

        public Vector3 CoordToWorld(float x, float y) => CoordToWorld(new Vector2(x, y));
        public virtual Vector3 CoordToWorld(Vector2 coord)
        {
            var localPosition = Grid.CellToLocalInterpolated(coord);
            return transform.TransformPoint(localPosition);
        }

        public virtual Vector2Int WorldToCoord(Vector3 worldPosition)
        {
            switch (Grid.cellLayout)
            {
                case GridLayout.CellLayout.Hexagon:
                    break;

                case GridLayout.CellLayout.Rectangle:
                    worldPosition += 0.5f * (Grid.cellSize + Grid.cellGap);
                    break;

                default:
                    worldPosition.y += 0.5f * (Grid.cellSize.y + Grid.cellGap.y);
                    break;
            }

            var coord = Grid.WorldToCell(worldPosition);
            return (Vector2Int)coord;
        }

       // public abstract IEnumerator<Vector2Int> GetEnumerator();

        public abstract IBoard GetBoardState();

        public void AddPieceComponent(PieceComponent component)
        {
            var piece = component.Piece;
            if (pieceComponents.ContainsKey(piece))
                Debug.LogError("EJ!");
            pieceComponents.Add(piece, component);
        }

        public void RemovePieceComponent(PieceComponent pieceComponent)
        {
            pieceComponents.Remove(pieceComponent.Piece);
        }

        public PieceComponent GetPieceComponent(Vector2Int coord) => GetPieceComponent(Board[coord]);
        public PieceComponent GetPieceComponent(Piece piece)
        {
            if (pieceComponents.TryGetValue(piece, out var component))
                return component;

            return null;
        }

        public void SwapPieces(Vector2Int pieceCoord1, Vector2Int pieceCoord2)
        {
            var board = GetBoardInternal();
            (board[pieceCoord1], board[pieceCoord2]) = (board[pieceCoord2], board[pieceCoord1]);
            board[pieceCoord1].Coord = pieceCoord1;
            board[pieceCoord2].Coord = pieceCoord2;
        }

        public void MovePiece(Piece piece, Vector2Int newCoord)
        {
            var board = GetBoardInternal();
            if (board.ContainsCoord(newCoord) == false)
                return;

            if (board[newCoord] != null)
            {
                Debug.LogError("Trying to move Piece to occupied coord");
                return;
            }

            board[newCoord] = piece;
            piece.Coord = newCoord;
        }

        internal abstract IBoard GetBoardInternal();
    }

    public abstract class BoardComponent<TBoard> : BoardComponent
        where TBoard : Board
    {
        protected TBoard board;
        public override IReadOnlyBoard Board => GetBoard();

        public TBoard GetBoard()
        {
            if (board == null)
                CreateBoard();
            return board;
        }

        public override bool ContainsCoord(Vector2Int coord) => board.ContainsCoord(coord);

        public override IBoard GetBoardState() => board.Clone();

        protected virtual void Reset()
        {
            board = null;
        }

        protected virtual void Awake()
        {
            board ??= CreateBoard();
        }

        protected abstract TBoard CreateBoard();
        internal override IBoard GetBoardInternal() => GetBoard();
    }
}
