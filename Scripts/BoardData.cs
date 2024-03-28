using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public abstract class BoardData
    {
        public abstract Piece this[Vector2Int coord] { get; set; }

        public bool ContainsCoord(Vector2Int coord) => ContainsCoord(coord.x, coord.y);
        public abstract bool ContainsCoord(int x, int y);
    }
}
