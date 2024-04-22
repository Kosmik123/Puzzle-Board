using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class PieceFactoryWrapper : MonoBehaviour
    {
        public abstract IPieceFactory PieceFactory { get; }
    }

    public class DefaultPieceFactoryWrapper : PieceFactoryWrapper 
    {
        [SerializeField]
        private PieceColorProvider pieceColorProvider;

        private IPieceFactory _pieceFactory;
        public override IPieceFactory PieceFactory
        {
            get
            {
                _pieceFactory ??= new DefaultPieceFactory(pieceColorProvider);
                return _pieceFactory;
            }
        }
    }
}
