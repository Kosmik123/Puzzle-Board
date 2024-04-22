using Bipolar.PuzzleBoard.Core;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class PieceFactoryWrapper : MonoBehaviour
    {
        [SerializeField]
        private PieceColorProvider pieceColorProvider;

        private IPieceFactory _pieceFactory;
        public IPieceFactory PieceFactory
        {
            get
            {
                _pieceFactory ??= new DefaultPieceFactory(pieceColorProvider);
                return _pieceFactory;
            }
        }
    }
}
