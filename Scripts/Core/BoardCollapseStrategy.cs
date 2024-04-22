namespace Bipolar.PuzzleBoard.Core
{ 
    [System.Serializable]
    public abstract class BoardCollapseStrategy<TBoard>
        where TBoard : IBoard
    {
        public delegate void CollapseEventHandler(BoardCollapseStrategy<TBoard> sender, ICollapseEventArgs eventArgs);
        
        public abstract event CollapseEventHandler OnPieceCollapsed;

        public abstract bool Collapse(TBoard board, IPieceFactory pieceFactory);
    }
}
