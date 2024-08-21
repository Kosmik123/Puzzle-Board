using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class PieceFactoryWrapper : MonoBehaviour
    {
        [SerializeField]
        protected PieceColorProvider pieceColorProvider;

        public abstract IPieceFactory PieceFactory { get; }
    }

    public class DefaultPieceFactoryWrapper : PieceFactoryWrapper 
    {
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
