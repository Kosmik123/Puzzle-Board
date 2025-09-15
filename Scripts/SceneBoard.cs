using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{

    [DisallowMultipleComponent]
    public abstract class SceneBoard : MonoBehaviour, ISceneBoard
    {
        private Grid _grid;
        public Grid Grid
        {
            get
            {
                if (_grid == null)
                    _grid = GetComponentInParent<Grid>() ?? gameObject.AddComponent<Grid>();
                return _grid;
            }
        }

        protected readonly Dictionary<IPiece, ScenePiece> scenePieces = new Dictionary<IPiece, ScenePiece>();

        public GridLayout.CellLayout Layout => Grid.cellLayout;
        public IBoard Board => GetBoard();
        public abstract bool ContainsCoord(Vector2Int coord);

        public bool TryGetPiece(Vector2Int coord, out IPiece piece)
        {
            piece = default;
            if (ContainsCoord(coord) == false)
                return false;

            piece = Board[coord];
            if (piece == null || piece.IsCleared)
                return false;

            return true;
        }

        public Vector3 CoordToWorld(float x, float y) => CoordToWorld(new Vector2(x, y));
        public virtual Vector3 CoordToWorld(Vector2 coord)
        {
            var localPosition = Grid.CellToLocalInterpolated(coord);
            return transform.TransformPoint(localPosition);
        }

        public virtual Vector2Int WorldToCoord(Vector3 worldPosition)
        {
            switch (Grid.cellLayout)
            {
                case GridLayout.CellLayout.Hexagon:
                    break;

                case GridLayout.CellLayout.Rectangle:
                    worldPosition += 0.5f * (Grid.cellSize + Grid.cellGap);
                    break;

                default:
                    worldPosition.y += 0.5f * (Grid.cellSize.y + Grid.cellGap.y);
                    break;
            }

            var coord = Grid.WorldToCell(worldPosition);
            return (Vector2Int)coord;
        }

       // public abstract IEnumerator<Vector2Int> GetEnumerator();

        public abstract IBoard GetBoardState();

        public void AddScenePiece(ScenePiece scenePiece)
        {
            var piece = scenePiece.Piece;
            if (scenePieces.ContainsKey(piece))
                Debug.LogError("EJ!");
            scenePieces.Add(piece, scenePiece);
        }

        public void RemoveScenePiece(ScenePiece scenePiece)
        {
            scenePieces.Remove(scenePiece.Piece);
        }

        public ScenePiece GetScenePiece(Vector2Int coord) => GetScenePiece(Board[coord]);
        public ScenePiece GetScenePiece(IPiece piece)
        {
            if (scenePieces.TryGetValue(piece, out var component))
                return component;

            return null;
        }

        public void MovePiece(Piece piece, Vector2Int newCoord)
        {
            var board = Board;
            if (board.ContainsCoord(newCoord) == false)
                return;

            if (board[newCoord] != null)
            {
                Debug.LogError("Trying to move Piece to occupied coord");
                return;
            }

            board[newCoord] = piece;
        }

        protected abstract IBoard GetBoard();
    }

    public abstract class SceneBoard<TBoard> : SceneBoard
        where TBoard : Board
    {
        protected TBoard board;
        public new TBoard Board
        {
            get
            {
                if (board == null)
					board = CreateBoard();
                return board;
            }
        }

        protected override IBoard GetBoard() => Board;

        public override bool ContainsCoord(Vector2Int coord) => board.ContainsCoord(coord);

        public override IBoard GetBoardState() => board.Clone();

        protected virtual void Reset()
        {
            board = null;
        }

        protected virtual void Awake()
        {
            board ??= CreateBoard();
        }

        protected abstract TBoard CreateBoard();
    }
}
