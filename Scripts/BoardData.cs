using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public abstract class BoardData
    {
        public abstract Piece this[Vector2Int coord] { get; set; }
        public GridLayout.CellLayout Layout { get; private set; }
        public bool ContainsCoord(Vector2Int coord) => ContainsCoord(coord.x, coord.y);
        public abstract bool ContainsCoord(int x, int y);

        public BoardData(GridLayout.CellLayout layout)
        {
            Layout = layout;
        }
    }
}
