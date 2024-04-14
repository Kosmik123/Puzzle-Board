using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.General
{
    [System.Serializable]
    public class GeneralBoardState : Board
    {
        private readonly Dictionary<Vector2Int, PieceComponent> piecesByCoords = new Dictionary<Vector2Int, PieceComponent>();

        public override PieceComponent this[Vector2Int coord]
        {
            get => piecesByCoords[coord];
            set => piecesByCoords[coord] = value;
        }

        public GeneralBoardState(IEnumerable<Vector2Int> coords, GridLayout.CellLayout layout) : base(layout)
        {
            foreach (var coord in coords)
                piecesByCoords.Add(coord, null);
        }

        private GeneralBoardState(GeneralBoardState source) : base(source.Layout)
        {
            piecesByCoords = source.piecesByCoords;
        }

        public override bool ContainsCoord(int x, int y) => piecesByCoords.ContainsKey(new Vector2Int(x, y));

        public override Board Clone() => new GeneralBoardState(this);

        public override IEnumerator<Vector2Int> GetEnumerator()
        {
            foreach (var coord in piecesByCoords.Keys)
                yield return coord;
        }
    }
}
