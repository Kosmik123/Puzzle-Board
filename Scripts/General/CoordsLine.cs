using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bipolar.PuzzleBoard.General
{
    public class CoordsLine
    {
        private readonly Vector2Int[] coords;
        public IReadOnlyList<Vector2Int> Coords => coords;

        public CoordsLine(IEnumerable<Vector2Int> coords)
        {
            this.coords = coords.ToArray();
        }
    }
}
