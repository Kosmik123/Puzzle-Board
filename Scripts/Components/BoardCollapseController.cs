using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    public abstract class BoardCollapseController : MonoBehaviour
    {
        public abstract event System.Action OnPiecesColapsed;

        public abstract System.Type BoardType { get; }
        public abstract void Collapse();
    }

    [DisallowMultipleComponent, RequireComponent(typeof(BoardComponent<>))]
    public class BoardCollapseController<TStrategy, TBoard> : BoardCollapseController
        where TBoard : Board
        where TStrategy : BoardCollapseStrategy<TBoard>
    {
        public sealed override event System.Action OnPiecesColapsed;

        [SerializeField]
        private PieceFactoryWrapper pieceFactory;

        [SerializeField]
        private BoardComponent<TBoard> _boardComponent;
        public BoardComponent<TBoard> BoardComponent
        {
            get
            {
                if (_boardComponent == null)
                    _boardComponent = GetComponent<BoardComponent<TBoard>>();
                return _boardComponent;
            }
        }

        private BoardCollapser<TBoard> _collapser;
        public BoardCollapser<TBoard> Collapser
        {
            get
            {
                _collapser ??= CreateNewCollapser();
                return _collapser;
            }
        }

        [SerializeField]
        protected TStrategy strategy;
        public virtual TStrategy Strategy => strategy;

        [SerializeField]
        protected PiecesMover<TStrategy, TBoard> mover;
        
        private BoardCollapser<TBoard> CreateNewCollapser() => new BoardCollapser<TBoard>(
            BoardComponent.GetBoard(),
            Strategy,
            pieceFactory ? pieceFactory.PieceFactory : null);

        public sealed override System.Type BoardType => typeof(TBoard);

        private void Awake()
        {
            _collapser = CreateNewCollapser();
        }

        private void OnEnable()
        {
            Collapser.OnCollapsed += Collapser_OnCollapsed;
        }

        [ContextMenu("Collapse")]
        public sealed override void Collapse() => Collapser.Collapse();

        private void Collapser_OnCollapsed()
        {
            var collapseCommand = new CollapseBoardCommand<TStrategy, TBoard>(mover, Collapser.CollapseEvents, Strategy);
            collapseCommand.Execute();
        }

        private void OnDisable()
        {
            Collapser.OnCollapsed -= Collapser_OnCollapsed;
        }

        private void OnDestroy()
        {
            _collapser?.Dispose();
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
                _collapser = null;
            else
                _collapser ??= CreateNewCollapser();
        }
    }

    public struct CollapseBoardCommand<TStrategy, TBoard> : IBoardCommand 
        where TBoard : Board
        where TStrategy : BoardCollapseStrategy<TBoard>
    {
        private readonly IReadOnlyList<ICollapseEventArgs> collapseEvents;
        private readonly PiecesMover<TStrategy, TBoard> piecesMover;
        private readonly TStrategy strategy;

        public CollapseBoardCommand(
            PiecesMover<TStrategy, TBoard> piecesMover,
            IReadOnlyList<ICollapseEventArgs> collapseEvents,
            TStrategy strategy)
        {
            this.collapseEvents = collapseEvents;
            this.piecesMover = piecesMover;
            this.strategy = strategy;
        }

        public void Execute()
        {
            foreach (var collapseEvent in collapseEvents)
            {
                piecesMover.HandleCollapseMovemement(strategy, collapseEvent);
            }

        }
    }
}
