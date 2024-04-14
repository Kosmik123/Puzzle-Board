using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.General
{
    [System.Serializable]
    public class GeneralBoard : Board
    {
        private readonly Dictionary<Vector2Int, Piece> piecesByCoords = new Dictionary<Vector2Int, Piece>();

        public override Piece this[Vector2Int coord]
        {
            get => piecesByCoords[coord];
            set => piecesByCoords[coord] = value;
        }

        public GeneralBoard(IEnumerable<Vector2Int> coords, GridLayout.CellLayout layout) : base(layout)
        {
            foreach (var coord in coords)
                piecesByCoords.Add(coord, null);
        }

        private GeneralBoard(GeneralBoard source) : base(source.Layout)
        {
            piecesByCoords = source.piecesByCoords;
        }

        public override bool ContainsCoord(int x, int y) => piecesByCoords.ContainsKey(new Vector2Int(x, y));

        public override Board Clone() => new GeneralBoard(this);

        public override IEnumerator<Vector2Int> GetEnumerator()
        {
            foreach (var coord in piecesByCoords.Keys)
                yield return coord;
        }
    }
}
