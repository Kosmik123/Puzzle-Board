using UnityEngine;
using UnityEngine.Profiling;

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
            using (new PredictableRandom(seedInstance))
            {
                int randomIndex = Random.Range(0, pieceColorsList.Count);
                return pieceColorsList[randomIndex];
            }
        }

        public readonly struct PredictableRandom : System.IDisposable
        {
            private readonly Random.State previousState;

            public PredictableRandom(int seed)
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
