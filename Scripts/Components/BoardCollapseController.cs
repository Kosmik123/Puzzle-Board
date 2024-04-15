using UnityEditor;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class BoardCollapseStrategyWrapper<TBoard> : MonoBehaviour
        where TBoard : IBoard
    {
        [SerializeReference]
        private BoardCollapseStrategy strategy;
        public BoardCollapseStrategy<TBoard> Strategy => (BoardCollapseStrategy<TBoard>)strategy;
        
        public abstract System.Type StrategyType { get; }
        protected abstract BoardCollapseStrategy<TBoard> CreateStrategy();

        private void Reset()
        {
            strategy = CreateStrategy();
        }

        private void OnValidate()
        {
            strategy ??= CreateStrategy();
        }
    }

    public class BoardCollapseStrategyWrapper<TStrategy, TBoard> : BoardCollapseStrategyWrapper<TBoard>
        where TStrategy : BoardCollapseStrategy<TBoard>, new()
        where TBoard : IBoard
    {
        public override System.Type StrategyType => typeof(TStrategy);
        protected override BoardCollapseStrategy<TBoard> CreateStrategy() => new TStrategy();
    }

    [System.Serializable]
    public abstract class BoardCollapseStrategy
    {
        public abstract void Collapse(IBoard board);
    }

    [System.Serializable]
    public abstract class BoardCollapseStrategy<TBoard> : BoardCollapseStrategy
        where TBoard : IBoard
    {
        public sealed override void Collapse(IBoard board) => Collapse((TBoard)board);   
        protected abstract void Collapse(TBoard board);   
    }

    [System.Serializable]
    public class BoardCollapser<TBoard>
        where TBoard : IBoard
    {
        private readonly TBoard board;
        private readonly BoardCollapseStrategy<TBoard> strategy;

        public BoardCollapser(TBoard board, BoardCollapseStrategy<TBoard> strategy)
        {
            this.board = board;    
            this.strategy = strategy;
        }

        public void Collapse()
        {

        }
    }

    public abstract class BoardCollapseController : MonoBehaviour
    {
        public abstract event System.Action OnPiecesColapsed;

        public abstract System.Type BoardType { get; }
        public abstract bool IsCollapsing { get; }
        public abstract void Collapse();
    }

    [DisallowMultipleComponent, RequireComponent(typeof(BoardComponent<>))]
    public abstract class BoardCollapseController<TStrategy, TBoard> : BoardCollapseController
        where TBoard : Board
        where TStrategy : BoardCollapseStrategy<TBoard>
    {
        private BoardCollapser<TBoard> _collapser;
        public BoardCollapser<TBoard> Collapser
        {
            get
            {
                _collapser ??= CreateNewCollapser();
                return _collapser;
            }
        }

        [SerializeReference, HideInInspector]
        private BoardCollapseStrategy strategy;
        public BoardCollapseStrategy<TBoard> Strategy => (BoardCollapseStrategy<TBoard>)strategy;

        private BoardCollapser<TBoard> CreateNewCollapser() => new BoardCollapser<TBoard>((TBoard)BoardComponent.Board, Strategy);

        [SerializeField]
        private PiecesSpawner piecesSpawner;
        public PiecesSpawner PiecesSpawner
        {
            get => piecesSpawner;
            set => piecesSpawner = value;
        }

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

        public override System.Type BoardType => typeof(TBoard);

        public override void Collapse() => Collapser.Collapse();

        protected PieceComponent CreatePiece(Vector2Int coord)
        {
            var piece = PiecesSpawner.SpawnPiece(coord.x, coord.y);
            BoardComponent.AddPiece(piece);
            return piece;
        }

        private void Awake()
        {
            _collapser = CreateNewCollapser();
        }

        private void OnValidate()
        {
            if (Strategy != null)
            {
                _collapser ??= CreateNewCollapser();
            }
            else
            {
                _collapser = null;
            }

        }
    }
}
