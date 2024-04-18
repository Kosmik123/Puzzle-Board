using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class BoardCollapseController : MonoBehaviour
    {
        public abstract event System.Action OnPiecesColapsed;

        public abstract System.Type BoardType { get; }
        public bool IsCollapsing { get; protected set; }
        public abstract void Collapse();
    }

    [RequireComponent(typeof(BoardComponent<>))]
    public abstract class PiecesMover<TStrategy, TBoard> : MonoBehaviour
        where TBoard : Board
        where TStrategy : BoardCollapseStrategy<TBoard>
    {
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

        public abstract void HandleCollapseMovemement(TStrategy strategy, ICollapseEventArgs collapseEventArgs);
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
            foreach (var collapseEvent in Collapser.CollapseEvents)
            {
                mover.HandleCollapseMovemement(Strategy, collapseEvent);
            }
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
            _collapser ??= CreateNewCollapser();
        }
    }
}
