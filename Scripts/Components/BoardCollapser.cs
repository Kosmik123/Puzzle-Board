using System.Collections.Generic;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public class BoardCollapser<TBoard> : System.IDisposable
        where TBoard : IBoard
    {
        public event System.Action OnCollapsed;

        private readonly TBoard board;
        private readonly BoardCollapseStrategy<TBoard> strategy;
        private readonly IPieceFactory pieceFactory;
        
        private readonly List<ICollapseEventArgs> collapseEvents = new List<ICollapseEventArgs>();
        public IReadOnlyList<ICollapseEventArgs> CollapseEvents => collapseEvents;

        public TBoard Board => board;

        public BoardCollapser(TBoard board, BoardCollapseStrategy<TBoard> strategy, IPieceFactory pieceFactory)
        {
            this.board = board;    
            this.strategy = strategy;
            this.pieceFactory = pieceFactory;
            strategy.OnPieceCollapsed += Strategy_OnCollapsed;
        }

        private void Strategy_OnCollapsed(BoardCollapseStrategy<TBoard> sender, ICollapseEventArgs eventArgs)
        {
            collapseEvents.Add(eventArgs);
        }

        public void Collapse()
        {
            collapseEvents.Clear();
            strategy.Collapse(board, pieceFactory);
            OnCollapsed?.Invoke();
        }

        public void Dispose()
        {
            strategy.OnPieceCollapsed -= Strategy_OnCollapsed;
        }
    }
}
