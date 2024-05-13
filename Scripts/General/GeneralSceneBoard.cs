using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Bipolar.PuzzleBoard.General
{
    public interface IGeneralSceneBoard : ISceneBoard
    {
        void CreateBoardShape();
    }

    [AddComponentMenu("Board Puzzles/General Board")]
    public class GeneralSceneBoard : SceneBoard<GeneralBoard>, IGeneralSceneBoard
    {
        [SerializeField, Tooltip("Provides board shape")]
        private Tilemap shapeTilemap;
        public Tilemap ShapeTilemap => shapeTilemap;

        private List<Vector2Int> includedCoords;
        public IReadOnlyCollection<Vector2Int> Coords
        {
            get
            {
                if (includedCoords == null) 
                    CreateBoardShape();
                return includedCoords;
            }
        }

        //Piece IReadOnlyBoard.this[Vector2Int coord] => this[coord];
        public Piece this[Vector2Int coord]
        {
            get => board[coord]; 
            set => board[coord] = value;
        }

        protected override void Reset()
        {
            base.Reset();
            shapeTilemap = GetComponentInChildren<Tilemap>();
        }

        protected override GeneralBoard CreateBoard()
        {
            CreateBoardShape();
            return board;
        }

        [ContextMenu("Refresh")]
        public void CreateBoardShape()
        {
            includedCoords = new List<Vector2Int>();
            var coordBounds = shapeTilemap.cellBounds;
            for (int y = coordBounds.yMin; y <= coordBounds.yMax; y++)
            {
                for (int x = coordBounds.xMin; x <= coordBounds.xMax; x++)
                {
                    var coord = new Vector2Int(x, y);
                    var tile = shapeTilemap.GetTile((Vector3Int)coord);
                    if (tile)
                        includedCoords.Add(coord);
                }
            }
            board = new GeneralBoard(includedCoords, Grid.cellLayout);
        }

        public override bool ContainsCoord(Vector2Int coord)
        {
            return shapeTilemap.cellBounds.Contains(new Vector3Int(coord.x, coord.y, shapeTilemap.cellBounds.z))
                && base.ContainsCoord(coord);
        }

        public override Vector3 CoordToWorld(Vector2 coord)
        {
            Vector3 cellPosition = base.CoordToWorld(coord);
            return cellPosition + Grid.Swizzle(Grid.cellSwizzle, shapeTilemap.tileAnchor);
        }

        public override Vector2Int WorldToCoord(Vector3 worldPosition)
        {
            worldPosition -= shapeTilemap.tileAnchor;
            return base.WorldToCoord(worldPosition);
        }

        public IEnumerator<Vector2Int> GetEnumerator()
        {
            foreach (var coord in Coords)
                yield return coord;
        }

        private void OnDrawGizmosSelected()
        {
            if (includedCoords == null)
                return;

            Gizmos.color = 0.7f * Color.white;
            foreach (var coord in Coords)
                Gizmos.DrawSphere(CoordToWorld(coord), 0.3f);
        }
    }
}
