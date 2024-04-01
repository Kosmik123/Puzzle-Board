using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class RandomPieceTypeProvider : PiecesColorProvider
    {
        [SerializeField]
        protected PieceColorsList pieceColorsList;

        public override IPieceColor GetPieceColor(int x, int y)
        {
            var randomIndex = Random.Range(0, pieceColorsList.Count);
            return pieceColorsList[randomIndex];
        }
    }
}
