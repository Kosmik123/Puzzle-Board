using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    public delegate void PieceCoordChangeEventHandler(PieceComponent piece, Vector2Int newCoord);

    [DisallowMultipleComponent, RequireComponent(typeof(IBoardComponent), typeof(BoardCollapseController<,>))]
    public class BoardController : MonoBehaviour
    {
        public event System.Action OnPiecesColapsed
        {
            add => BoardCollapseController.OnPiecesColapsed += value;
            remove
            {
                if (BoardCollapseController)
                    BoardCollapseController.OnPiecesColapsed -= value;
            }
        }

        [SerializeField]
        protected DefaultPiecesMovementManager piecesMovementManager;
        public PiecesMovementManager PiecesMovementManager => piecesMovementManager;

        private BoardCollapseController _boardCollapseController;
        public BoardCollapseController BoardCollapseController
        {
            get
            {
                if (_boardCollapseController == null && BoardComponent != null)
                    _boardCollapseController = BoardComponent.GetComponent<BoardCollapseController>();
                return _boardCollapseController;
            }
        }

        public bool ArePiecesMoving => piecesMovementManager.ArePiecesMoving;

        public bool IsCollapsing => BoardCollapseController.IsCollapsing;

        [ContextMenu("Collapse")]
        public void Collapse() => BoardCollapseController.Collapse();

        protected BoardComponent _boardComponent;
        public BoardComponent BoardComponent
        {
            get
            {
                if (_boardComponent == null && this)
                    _boardComponent = GetComponent<BoardComponent>();
                return _boardComponent;
            }
        }

        [SerializeField]
        private bool collapseOnStart;
        public bool CollapseOnStart
        {
            get => collapseOnStart;
            set => collapseOnStart = value;
        }

        [SerializeField]
        private bool collapseConstantly;
        public bool CollapseConstantly
        {
            get => collapseConstantly;
            set => collapseConstantly = value;
        }

        protected virtual void Awake()
        {
            _boardComponent = GetComponent<BoardComponent>();
        }

        protected virtual void Start()
        {
            if (collapseOnStart)
                Collapse();
        }

        private readonly LinkedList<Vector2Int> shuffledCoords = new LinkedList<Vector2Int>();

        private void Update()
        {
            if (collapseConstantly)
                Collapse();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShufflePieces();
            }
        }

        public void ShufflePieces()
        {
            shuffledCoords.Clear();
            foreach (var coord in BoardComponent)
            {
                if (Random.value > 0.5f)
                {
                    shuffledCoords.AddFirst(coord);
                }
                else
                {
                    shuffledCoords.AddLast(coord);
                }
            }

            foreach (var coord in BoardComponent)
            {
                var randomCoord = shuffledCoords.First;
                shuffledCoords.RemoveFirst();
                // (Pieces[randomCoord.Value], Pieces[coord]) = (Pieces[coord], Pieces[randomCoord.Value]);
            }
        }
    }
}
