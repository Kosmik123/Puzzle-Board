using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.General
{
    [System.Serializable]
    public class GeneralBoardData : BoardData
    {
        private readonly Dictionary<Vector2Int, Piece> piecesByCoords = new Dictionary<Vector2Int, Piece>();

        public override Piece this[Vector2Int coord]
        {
            get => piecesByCoords[coord];
            set => piecesByCoords[coord] = value;
        }

        public GeneralBoardData(IEnumerable<Vector2Int> coords)
        {
            foreach (var coord in coords)
                piecesByCoords.Add(coord, null);
        }

        public override bool ContainsCoord(int x, int y) => piecesByCoords.ContainsKey(new Vector2Int(x, y));
    }
}
