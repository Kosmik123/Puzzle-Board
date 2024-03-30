using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [DisallowMultipleComponent, RequireComponent(typeof(Grid))]
    public abstract class Board : MonoBehaviour, IModifiableBoard
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

        public abstract Piece this[Vector2Int coord] { get; set; }
        public abstract BoardData Data { get; }
        
        public abstract bool ContainsCoord(Vector2Int coord);
        public abstract Piece GetPiece(int x, int y);
        public abstract Piece GetPiece(Vector2Int coord);

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
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public abstract class Board<TData> : Board
        where TData : BoardData
    {
        protected TData boardData = null;
        public override BoardData Data
        {
            get
            {
                if (boardData == null)
                    CreateBoardData();
                return boardData;
            }
        }

        public sealed override Piece this[Vector2Int coord]
        {
            get => boardData[coord];
            set => boardData[coord] = value;
        }

        public override bool ContainsCoord(Vector2Int coord) => boardData.ContainsCoord(coord);

        public override Piece GetPiece(int x, int y) => GetPiece(new Vector2Int(x, y));
        public override Piece GetPiece(Vector2Int coord)
        {
            if (ContainsCoord(coord) == false)
                return null;

            var piece = this[coord];
            if (piece == null || piece.IsCleared)
                return null;
            return piece;
        }

        protected override void Awake()
        {
            base.Awake();
            CreateBoardData();
        }

        protected abstract void CreateBoardData();
    }
}
