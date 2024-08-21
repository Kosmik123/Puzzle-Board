using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class CompoundPieceFactory<TPiece> : GenericPieceFactory<TPiece> 
        where TPiece : ICompoundPiece, new()
    {
        private IPiecePropertiesConfiguration piecePropertiesConfiguration;

        public CompoundPieceFactory(
            IPieceColorProvider pieceColorProvider,
            IPiecePropertiesConfiguration piecePropertiesConfiguration = null) 
            : base(pieceColorProvider)
        {
            this.piecePropertiesConfiguration = piecePropertiesConfiguration;
        }

        public override IPiece CreatePiece(Vector2Int coord)
        {
            var piece = base.CreatePiece(coord);
            if (piece is ICompoundPiece compoundPiece)
            {
                piecePropertiesConfiguration?.ConfigureProperties(compoundPiece, coord);
                return piece;
            }
            return null;
        }

    }
}