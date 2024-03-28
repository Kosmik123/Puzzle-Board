using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Bipolar.PuzzleBoard.General
{
    public interface IGeneralBoard : IBoard
    {
        IReadOnlyList<Vector2Int> Coords { get; }
        void CreateBoardShape();
    }

    public class GeneralBoard : Board<GeneralBoardData>, IGeneralBoard
    {
        [SerializeField, Tooltip("Provides board shape")]
        [FormerlySerializedAs("tilemap")]
        private Tilemap shapeTilemap;
        public Tilemap ShapeTilemap => shapeTilemap;

        private List<Vector2Int> includedCoords;
        public IReadOnlyList<Vector2Int> Coords
        {
            get
            {
                if (includedCoords == null) 
                    CreateBoardShape();
                return includedCoords;
            }
        }

        private void Reset()
        {
            shapeTilemap = GetComponentInChildren<Tilemap>();
        }

        protected override void CreateBoardData()
        {
            CreateBoardShape();
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
                    if (tile != null)
                    {
                        includedCoords.Add(coord);
                    }
                }
            }
            boardData = new GeneralBoardData(includedCoords, Grid.cellLayout);
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
