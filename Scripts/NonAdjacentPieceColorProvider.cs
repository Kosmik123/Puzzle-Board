using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class NonAdjacentPieceColorProvider : PiecesColorProvider
    {
        [SerializeField]
        private PieceColorSettings settings;
        [SerializeField]
        private Board board;

        private readonly HashSet<IPieceColor> forbiddenPieceColors = new HashSet<IPieceColor>();
        
        protected virtual void Reset()
        {
            board = FindObjectOfType<Board>();
        }

        public override IPieceColor GetPieceColor(int x, int y)
        {
            forbiddenPieceColors.Clear();
            var piece = board.GetPiece(x - 1, y);
            if (piece)
                forbiddenPieceColors.Add(piece.Color);

            piece = board.GetPiece(x + 1, y);
            if (piece)
                forbiddenPieceColors.Add(piece.Color);

            piece = board.GetPiece(x, y - 1);
            if (piece)
                forbiddenPieceColors.Add(piece.Color);

            piece = board.GetPiece(x, y + 1);
            if (piece)
                forbiddenPieceColors.Add(piece.Color);

            return settings.GetPieceColorExcept(forbiddenPieceColors);
        }
    }
}
