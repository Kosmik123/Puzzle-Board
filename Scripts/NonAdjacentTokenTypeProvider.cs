using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class NonAdjacentTokenTypeProvider : PieceTypeProvider
    {
        [SerializeField]
        private Settings settings;
        [SerializeField]
        private Board board;

        private readonly HashSet<IPieceType> forbiddenPieceTypes = new HashSet<IPieceType>();
        
        public override IPieceType GetPieceType(int x, int y)
        {
            forbiddenPieceTypes.Clear();
            var piece = board.GetPiece(x - 1, y);
            if (piece)
                forbiddenPieceTypes.Add(piece.Type);

            piece = board.GetPiece(x + 1, y);
            if (piece)
                forbiddenPieceTypes.Add(piece.Type);

            piece = board.GetPiece(x, y - 1);
            if (piece)
                forbiddenPieceTypes.Add(piece.Type);

            piece = board.GetPiece(x, y + 1);
            if (piece)
                forbiddenPieceTypes.Add(piece.Type);

            return settings.GetPieceTypeExcept(forbiddenPieceTypes);
        }
    }
}
