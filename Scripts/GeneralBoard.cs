using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.General
{
    public interface IGeneralBoard : IBoard
    {
        IReadOnlyCollection<Vector2Int> Coords { get; }
    }

    [System.Serializable]
    public class GeneralBoard : Board, IGeneralBoard
    {
        private readonly Dictionary<Vector2Int, Piece> piecesByCoords = new Dictionary<Vector2Int, Piece>();
        public IReadOnlyCollection<Vector2Int> Coords => piecesByCoords.Keys;

        protected override bool IsInited => base.IsInited && piecesByCoords != null;

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
            foreach (var coordAndPiece in source.piecesByCoords)
                piecesByCoords.Add(coordAndPiece.Key, coordAndPiece.Value);
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
