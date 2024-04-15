namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public class BoardCollapser<TBoard>
        where TBoard : IBoard
    {
        public event System.Action OnCollapsed;

        private readonly TBoard board;
        private readonly BoardCollapseStrategy<TBoard> strategy;
        private readonly IPieceFactory pieceFactory;

        public TBoard Board => board;

        public BoardCollapser(TBoard board, BoardCollapseStrategy<TBoard> strategy, IPieceFactory pieceFactory)
        {
            this.board = board;    
            this.strategy = strategy;
            this.pieceFactory = pieceFactory;
        }

        public void Collapse()
        {
            strategy.Collapse(board, pieceFactory);



            OnCollapsed?.Invoke();
        }
    }
}
