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
        
        private readonly Queue<ICollapseEventArgs> collapseEvents = new Queue<ICollapseEventArgs>();
        public IReadOnlyCollection<ICollapseEventArgs> CollapseEvents => collapseEvents;

        public TBoard Board => board;

        public BoardCollapser(TBoard board, BoardCollapseStrategy<TBoard> strategy, IPieceFactory pieceFactory)
        {
            this.board = board;    
            this.strategy = strategy;
            this.pieceFactory = pieceFactory;
            strategy.OnCollapsed += Strategy_OnCollapsed;
        }

        private void Strategy_OnCollapsed(BoardCollapseStrategy<TBoard> sender, ICollapseEventArgs eventArgs)
        {
            collapseEvents.Enqueue(eventArgs);
        }

        public void Collapse()
        {
            collapseEvents.Clear();
            strategy.Collapse(board, pieceFactory);
            OnCollapsed?.Invoke();
        }

        public void Dispose()
        {
            strategy.OnCollapsed -= Strategy_OnCollapsed;
        }
    }
}
