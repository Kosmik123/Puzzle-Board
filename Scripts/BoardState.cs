using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public abstract class BoardState : IBoardState
    {
        public abstract Piece this[Vector2Int coord] { get; set; }
        public GridLayout.CellLayout Layout { get; private set; }
        public bool ContainsCoord(Vector2Int coord) => ContainsCoord(coord.x, coord.y);
        public abstract bool ContainsCoord(int x, int y);

        public BoardState(GridLayout.CellLayout layout)
        {
            Layout = layout;
        }

        public abstract BoardState Clone();

        public abstract IEnumerator<Vector2Int> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
