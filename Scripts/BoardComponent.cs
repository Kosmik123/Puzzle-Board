using System.Collections;
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

        public abstract Piece this[Vector2Int coord] { get; set; }
        public GridLayout.CellLayout Layout => Grid.cellLayout;

        public abstract IBoard Board { get; }

        public abstract bool ContainsCoord(Vector2Int coord);

        public virtual PieceComponent GetPiece(int x, int y) => GetPiece(new Vector2Int(x, y));
        public abstract PieceComponent GetPiece(Vector2Int coord);

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

        protected virtual void Awake()
        { }

        public abstract IEnumerator<Vector2Int> GetEnumerator();

        public abstract Board GetBoardState();

        public void AddPiece(PieceComponent component)
        {
            pieceComponents.Add(component.Piece, component);
        }

        public void MovePiece(Piece piece, Vector2Int newCoord)
        {
            if (Board.ContainsCoord(newCoord) == false)
                return;

            if (Board[newCoord] != null)
            {
                Debug.LogError("Trying to move Piece to occupied coord");
                return;
            }

            Board[newCoord] = piece;
            piece.Coord = newCoord;
        }
    }

    public abstract class BoardComponent<TBoard> : BoardComponent // Proxy Pattern
        where TBoard : Board
    {
        protected TBoard board = null;
        public override IBoard Board => board;


        public sealed override Piece this[Vector2Int coord]
        {
            get => board[coord];
            set => board[coord] = value;
        }

        public override bool ContainsCoord(Vector2Int coord) => board.ContainsCoord(coord);

        public override Board GetBoardState() => board.Clone();

        public override PieceComponent GetPiece(Vector2Int coord)
        {
            if (ContainsCoord(coord) == false)
                return null;

            var piece = this[coord];
            if (piece == null || piece.IsCleared)
                return null;
            
            return pieceComponents[piece];
        }

        protected override void Awake()
        {
            base.Awake();
            CreateBoardData();
        }

        protected abstract void CreateBoardData();
    }
}
