using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public abstract class BoardData
    {
        public abstract Piece this[Vector2Int coord] { get; set; }
    }
}
