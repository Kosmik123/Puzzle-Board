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

        [SerializeField]
        private PiecesSpawner piecesSpawner;
        public PiecesSpawner PiecesSpawner
        {
            get => piecesSpawner;
            set => piecesSpawner = value;
        }

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
        private TStrategy strategy;

        private BoardCollapser<TBoard> CreateNewCollapser() => new BoardCollapser<TBoard>(
            BoardComponent.GetBoard(),
            strategy,
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

        public sealed override void Collapse() => Collapser.Collapse();

        private void Collapser_OnCollapsed()
        {
            foreach (var collapseEvent in Collapser.CollapseEvents)
            {
                if (collapseEvent is IPieceCreatedCollapseEventArgs createEvent)
                {
                    var piece = createEvent.Piece;
                    var pieceComponent = PiecesSpawner.SpawnPiece(piece);
                    pieceComponent.transform.position = BoardComponent.CoordToWorld(piece.Coord);
                    BoardComponent.AddPiece(pieceComponent);
                }
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
