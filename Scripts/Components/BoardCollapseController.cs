using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class BoardCollapseStrategyCreator<TBoard> : ScriptableObject
        where TBoard : IBoard
    {
        public abstract System.Type StrategyType { get; }
        public System.Type BoardType => typeof(TBoard); 
        public abstract BoardCollapseStrategy<TBoard> CreateStrategy();
    }

    public class BoardCollapseStrategyCreator<TStrategy, TBoard> : BoardCollapseStrategyCreator<TBoard>
        where TStrategy : BoardCollapseStrategy<TBoard>, new()
        where TBoard : IBoard
    {
        public override System.Type StrategyType => typeof(TStrategy);
        public override BoardCollapseStrategy<TBoard> CreateStrategy() => new TStrategy();
    }

    [System.Serializable]
    public abstract class BoardCollapseStrategy<TBoard>
        where TBoard : IBoard
    {
        public abstract void Collapse(TBoard board);
    }

    [System.Serializable]
    public class BoardCollapser<TBoard>
        where TBoard : IBoard
    {
        private TBoard board;

        public BoardCollapser(TBoard board)
        {
            this.board = board;    
        }

        private BoardCollapseStrategy<TBoard> strategy;
    }

    public abstract class BoardCollapseController : MonoBehaviour
    {
        public abstract event System.Action OnPiecesColapsed;

        public abstract bool IsCollapsing { get; }
        public abstract void Collapse();
    }

    [DisallowMultipleComponent, RequireComponent(typeof(BoardComponent<>))]
    public abstract class BoardCollapseController<TBoardComponent, TBoard> : BoardCollapseController
        where TBoardComponent : BoardComponent<TBoard>
        where TBoard : Board
    {
        [SerializeField]
        private BoardCollapseStrategyCreator<TBoard> strategyCreator;

        [SerializeField]
        private BoardCollapseStrategy<TBoard> _strategy;
        public BoardCollapseStrategy<TBoard> Strategy
        {
            get
            {
                _strategy ??= strategyCreator.CreateStrategy();
                return _strategy;
            }
        }

        private BoardCollapser<TBoard> _collapser;
        public BoardCollapser<TBoard> Collapser
        {
            get
            {
                _collapser ??= new BoardCollapser<TBoard>((TBoard)BoardComponent.Board);
                return _collapser;
            }
        }

        [SerializeField]
        private PiecesSpawner piecesSpawner;
        public PiecesSpawner PiecesSpawner
        {
            get => piecesSpawner;
            set => piecesSpawner = value;
        }

        private TBoardComponent _boardComponent;
        public TBoardComponent BoardComponent
        {
            get
            {
                if (_boardComponent == null)
                    _boardComponent = GetComponent<TBoardComponent>();
                return _boardComponent;
            }
        }

        public override void Collapse()
        {
           // Collapser.Collapse((TBoard)BoardComponent.Board);
        }

        protected PieceComponent CreatePiece(Vector2Int coord)
        {
            var piece = PiecesSpawner.SpawnPiece(coord.x, coord.y);
            BoardComponent.AddPiece(piece);
            return piece;
        }

        private void OnValidate()
        {
            if (strategyCreator)
            {
                if (_strategy == null || _strategy.GetType() != strategyCreator.StrategyType)
                {
                    _strategy = strategyCreator.CreateStrategy();
                }
            }
        }
    }
}
