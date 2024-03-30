using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class NonAdjacentTokenTypeProvider : PiecesColorProvider
    {
        [SerializeField]
        private Settings settings;
        [SerializeField]
        private Board board;

        private readonly HashSet<IPieceColor> forbiddenPieceTypes = new HashSet<IPieceColor>();
        
        public override IPieceColor GetPieceColor(int x, int y)
        {
            forbiddenPieceTypes.Clear();
            var piece = board.GetPiece(x - 1, y);
            if (piece)
                forbiddenPieceTypes.Add(piece.Color);

            piece = board.GetPiece(x + 1, y);
            if (piece)
                forbiddenPieceTypes.Add(piece.Color);

            piece = board.GetPiece(x, y - 1);
            if (piece)
                forbiddenPieceTypes.Add(piece.Color);

            piece = board.GetPiece(x, y + 1);
            if (piece)
                forbiddenPieceTypes.Add(piece.Color);

            return settings.GetPieceTypeExcept(forbiddenPieceTypes);
        }
    }
}
