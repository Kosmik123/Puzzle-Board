using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Grid))]
    public abstract class SceneBoard : MonoBehaviour, ISceneBoard
    {
        private Grid _grid;
        public Grid Grid
        {
            get
            {
                if (_grid == null)
                    _grid = GetComponent<Grid>();
                return _grid;
            }
        }

        protected readonly Dictionary<Piece, ScenePiece> scenePieces = new Dictionary<Piece, ScenePiece>();

        public GridLayout.CellLayout Layout => Grid.cellLayout;

        public abstract IReadOnlyBoard Board { get; }

        public abstract bool ContainsCoord(Vector2Int coord);

        public Piece GetPiece(Vector2Int coord)
        {
            if (ContainsCoord(coord) == false)
                return null;

            var piece = Board[coord];
            if (piece == null || piece.IsCleared)
                return null;

            return piece;
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
        public ScenePiece GetScenePiece(Piece piece)
        {
            if (scenePieces.TryGetValue(piece, out var component))
                return component;

            return null;
        }

        public void MovePiece(Piece piece, Vector2Int newCoord)
        {
            var board = GetBoardInternal();
            if (board.ContainsCoord(newCoord) == false)
                return;

            if (board[newCoord] != null)
            {
                Debug.LogError("Trying to move Piece to occupied coord");
                return;
            }

            board[newCoord] = piece;
        }

        internal abstract IBoard GetBoardInternal();
    }

    public abstract class SceneBoard<TBoard> : SceneBoard
        where TBoard : Board
    {
        protected TBoard board;
        public override IReadOnlyBoard Board => GetBoard();

        public TBoard GetBoard()
        {
            if (board == null)
                CreateBoard();
            return board;
        }

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
        internal override IBoard GetBoardInternal() => GetBoard();
    }
}
