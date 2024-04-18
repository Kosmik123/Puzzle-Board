using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class DefaultPredictablePieceColorProvider : PredictablePieceColorProvider
    {
        [SerializeField]
        private PieceColorsList pieceColors;

        [field: SerializeField]
        public override int Time { get; set; }

        [field: SerializeField]
        public override int Seed { get; set; }

        public override IPieceColor GetPieceColor(int x, int y)
        {
            var previousState = Random.state;
            Random.InitState(Seed + Time);
            int randomIndex = Random.Range(0, pieceColors.Count);
            Random.state = previousState;
            return pieceColors[randomIndex];
        }
    }
}
