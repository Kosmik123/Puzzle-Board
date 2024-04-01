using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Bipolar.PuzzleBoard
{
    public abstract class PieceColorsList : ScriptableObject, IReadOnlyList<IPieceColor>
    {
        public abstract IPieceColor this[int index] { get; }
        public abstract int Count { get; }
        public abstract IEnumerator<IPieceColor> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
