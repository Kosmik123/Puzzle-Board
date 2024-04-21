using Bipolar.PuzzleBoard.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public struct ClearPiecesCommand : IBoardCommand
    {
        private readonly IReadOnlyList<Piece> piecesToClear;
        private readonly BoardComponent boardComponent;

        public ClearPiecesCommand(IReadOnlyList<Piece> piecesToClear, BoardComponent boardComponent)
        {
            this.piecesToClear = piecesToClear;
            this.boardComponent = boardComponent;
        }

        public IEnumerator Execute()
        {
            // tu będzie potrzebny manager clearingu żeby ogarnąć yieldowanie
            foreach (var piece in piecesToClear)
            {
                var pieceComponent = boardComponent.GetPieceComponent(piece);
                pieceComponent.Clear();
            }

            yield return new WaitForSeconds(0.5f); // temporary to test
        }

        public override string ToString()
        {
            return $"Command to clear {piecesToClear.Count} pieces";
        }
    }
   
}

namespace Bipolar.PuzzleBoard.Components
{

}
