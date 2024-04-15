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

    [DisallowMultipleComponent, RequireComponent(typeof(BoardComponent<>))]
    public class BoardCollapseController<TStrategy, TBoard> : BoardCollapseController
        where TBoard : Board
        where TStrategy : BoardCollapseStrategy<TBoard>
    {
        public sealed override event System.Action OnPiecesColapsed;

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
        private TStrategy strategy;
        public TStrategy Strategy => strategy;

        private BoardCollapser<TBoard> CreateNewCollapser() => new BoardCollapser<TBoard>(
            BoardComponent.GetBoard(),
            Strategy, 
            pieceFactory: null);

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

        public sealed override System.Type BoardType => typeof(TBoard);

        private void Awake()
        {
            _collapser = CreateNewCollapser();
        }

        public sealed override void Collapse() => Collapser.Collapse();

        protected PieceComponent CreatePiece(Vector2Int coord)
        {
            var piece = PiecesSpawner.SpawnPiece(coord.x, coord.y);
            BoardComponent.AddPiece(piece);
            return piece;
        }

        private void CallCollapseEvent()
        {
            // piecesMovementManager.OnAllPiecesMovementStopped -= CallCollapseEvent;
            OnPiecesColapsed?.Invoke();
        }


        private void OnValidate()
        {
            _collapser ??= CreateNewCollapser();
        }
    }
}
