using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
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

        public abstract IBoard Board { get; }

        public abstract bool ContainsCoord(Vector2Int coord);

        public PieceComponent GetPiece(int x, int y) => GetPiece(new Vector2Int(x, y));
        public PieceComponent GetPiece(Vector2Int coord)
        {
            if (ContainsCoord(coord) == false)
                return null;

            var piece = Board[coord];
            if (piece == null || piece.IsCleared)
                return null;

            return pieceComponents[piece];
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

        public abstract IEnumerator<Vector2Int> GetEnumerator();

        public abstract IBoard GetBoardState();

        public void AddPiece(PieceComponent component)
        {
            var piece = component.Piece;
            pieceComponents.Add(piece, component);
        }

        public void RemovePiece(PieceComponent pieceComponent)
        {
            pieceComponents.Remove(pieceComponent.Piece);
        }

        public PieceComponent GetPieceComponent(Piece piece)
        {
            if (pieceComponents.TryGetValue(piece, out var component))
                return component;

            return null;
        }

        public void MovePiece(Piece piece, Vector2Int newCoord)
        {
            var board = Board;
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
    }

    public abstract class BoardComponent<TBoard> : BoardComponent
        where TBoard : Board
    {
        protected TBoard board;
        public override IBoard Board => GetBoard();

        public TBoard GetBoard()
        {
            if (board == null)
                CreateBoardData();
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
            CreateBoardData();
        }

        protected abstract void CreateBoardData();
    }
}
