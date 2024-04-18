using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface ICollapseEventArgs
    { 
        Piece Piece { get; }
    }

    public interface IPieceCreatedCollapseEventArgs : ICollapseEventArgs
    {
        int IndexInLine { get; }
    }

    public interface IExistingPieceCollapseEventArgs : ICollapseEventArgs
    {
        Vector2Int FromCoord { get; }
    }

    [System.Serializable]
    public abstract class BoardCollapseStrategy<TBoard>
        where TBoard : IBoard
    {
        public delegate void CollapseEventHandler(BoardCollapseStrategy<TBoard> sender, ICollapseEventArgs eventArgs);
        
        public abstract event CollapseEventHandler OnPieceCollapsed;

        public abstract bool Collapse(TBoard board, IPieceFactory pieceFactory);
    }
}
