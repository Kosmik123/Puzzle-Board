namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public abstract class BoardCollapseStrategy<TBoard>
        where TBoard : IBoard
    {
        public abstract bool Collapse(TBoard board, IPieceFactory pieceFactory);
    }
}
