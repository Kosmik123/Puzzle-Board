using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class DefaultPredictablePieceColorProvider : PredictablePieceColorProvider
    {
        [SerializeField]
        private PieceColorsList pieceColorsList;

        [field: SerializeField]
        public override int Time { get; set; }

        [field: SerializeField]
        public override int Seed { get; set; }

        public override IPieceColor GetPieceColor(int x, int y)
        {
            int seedInstance = 10000 * x + 100 * y + Time + Seed;
            using (new RandomScope(seedInstance))
            {
                int randomIndex = Random.Range(0, pieceColorsList.Count);
                return pieceColorsList[randomIndex];
            }
        }

        public readonly struct RandomScope : System.IDisposable
        {
            private readonly Random.State previousState;

            public RandomScope(int seed)
            {
                previousState = Random.state;
                Random.InitState(seed);
            }

            public void Dispose()
            {
                Random.state = previousState;
            }
        }
    }
}
