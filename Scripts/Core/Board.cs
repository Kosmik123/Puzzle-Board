using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public abstract class Board : IBoard
    {
        public abstract Piece this[Vector2Int coord] { get; set; }
        public GridLayout.CellLayout Layout { get; private set; }
        public bool ContainsCoord(Vector2Int coord) => ContainsCoord(coord.x, coord.y);
        public abstract bool ContainsCoord(int x, int y);

        private readonly bool isInited = false;
        protected virtual bool IsInited => isInited;

        public readonly System.Guid ID;

        public Board(GridLayout.CellLayout layout)
        {
            ID = System.Guid.NewGuid();
            isInited = true;
            Layout = layout;
        }

        public abstract Board Clone();
        public static void Copy(Board source, IBoard target) => source.CopyState(target);
        protected abstract void CopyState(IBoard target);

        public abstract IEnumerator<Vector2Int> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

#if UNITY_EDITOR
        public static bool operator !=(Board lhs, Board rhs) => !(lhs == rhs);
        public static bool operator ==(Board lhs, Board rhs)
        {
            bool rightIsNull = rhs is null;
            bool leftIsNull = lhs is null;
            if (rightIsNull && leftIsNull)
                return true;

            if (rightIsNull)
                return !lhs.IsInited;

            if (leftIsNull)
                return !rhs.IsInited;

            return lhs.Equals(rhs);
        }
#endif
    }
}

