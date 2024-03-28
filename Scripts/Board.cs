﻿using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class Board<TData> : Board 
        where TData : BoardData
    {
        protected TData boardData;

        public override Piece this[Vector2Int coord]
        {
            get => boardData[coord];
            set => boardData[coord] = value;
        }
    }

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

        protected virtual void Awake()
        { }

        public bool ContainsCoord(Vector2Int coord) => ContainsCoord(coord.x, coord.y);
        public abstract bool ContainsCoord(int x, int y);

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

        public Piece GetPiece(int x, int y) => GetPiece(new Vector2Int(x, y));
        public Piece GetPiece(Vector2Int coord)
        {
            if (ContainsCoord(coord) == false)
                return null;

            var piece = this[coord];
            if (piece == null || piece.IsCleared)
                return null;
            return piece;
        }
    }
}
