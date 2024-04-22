using Bipolar.PuzzleBoard.General;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Bipolar.PuzzleBoard
{
    [RequireComponent(typeof(GeneralBoardComponent))]
    public class PrioritizedGeneralBoardCollapsing : BoardCollapseController<MockGeneralCollapseStrategy, GeneralBoard>
    {
        [SerializeField]
        private Tilemap[] directionsTilemaps;

        private Dictionary<Vector2Int, List<Vector2Int>> directionsByCoord;
        public IReadOnlyList<Vector2Int> GetDirections(Vector2Int coord) => directionsByCoord.TryGetValue(coord, out var directions) ? directions : null;

        private Dictionary<Vector2Int, List<Vector2Int>> sourceTilesCoordsByCoord;

        [SerializeField]
        private DefaultPiecesMovementManager piecesMovementManager;

        private void Awake()
        {
            CreateDirections();
            CreateSourcesDictionary();
        }

        [ContextMenu("Create Directions")]
        private void CreateDirections()
        {
            directionsByCoord = new Dictionary<Vector2Int, List<Vector2Int>>();
            foreach (var tilemap in directionsTilemaps)
                ExtractDirectionsFromTilemap(tilemap);
        }

        private void ExtractDirectionsFromTilemap(Tilemap tilemap)
        {
            bool isBoardHexagonal = BoardComponent.Layout == GridLayout.CellLayout.Hexagon;
            var bounds = tilemap.cellBounds;
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++) 
                { 
                    var coord = new Vector2Int(x, y);
                    if (DirectionTileHelper.TryGetTile(coord, tilemap, out var tile) == false)
                        continue;

                    var direction = DirectionTileHelper.GetTileDirection(coord, tile, isBoardHexagonal);
                    var directionsList = GetOrCreateList(directionsByCoord, coord);
                    directionsList.Add(direction);
                }
            }
        }

        [ContextMenu("Create Sources")]
        private void CreateSourcesDictionary()
        {
            sourceTilesCoordsByCoord = new Dictionary<Vector2Int, List<Vector2Int>>();
            foreach (var coordAndDirections in directionsByCoord)
            {
                var coord = coordAndDirections.Key;
                var directions = coordAndDirections.Value;
                foreach (var direction in directions)
                {
                    var targetCoord = coord + direction;
                    var sourcesList = GetOrCreateList(sourceTilesCoordsByCoord, targetCoord);
                    sourcesList.Add(coord);
                }
            }
        }

        public static List<TValue> GetOrCreateList<TKey, TValue>(Dictionary<TKey, List<TValue>> dictionary, TKey key)
        {
            if (dictionary.TryGetValue(key, out var directionsList) == false)
                dictionary[key] = directionsList = new List<TValue>();

            return directionsList;  
        }

        private readonly Queue<Vector2Int> emptyCoords = new Queue<Vector2Int>();
        //public override void Collapse()
        //{
        //    piecesMovementManager.OnPieceMovementEnded += PiecesMovementManager_OnPieceMovementEnded;
        //    foreach (var coord in BoardComponent.Board)
        //    {
        //        if (BoardComponent.GetPiece(coord) == null)
        //            continue;

        //        TryCollapsePieceFromCoord(coord);
        //    }
        //}

        private bool TryCollapsePieceFromCoord(Vector2Int coord)
        {
            var piece = BoardComponent.GetPiece(coord);
            var directions = GetDirections(coord);
            if (directions == null)
                return false;

            foreach (var direction in directions)
            {
                var targetCoord = coord + direction;
                if (BoardComponent.ContainsCoord(targetCoord) == false)
                    continue;

                if (BoardComponent.GetPiece(targetCoord) == null)
                {
                    //Board[coord] = null;
                    //Board[targetCoord] = piece;
                   // collapsingPiecesCoords.Add(piece, targetCoord);
                    //piecesMovementManager.StartPieceMovement(piece, targetCoord);
                    return true;
                }
            }

            return false;
        }


        private readonly Dictionary<PieceComponent, Vector2Int> collapsingPiecesCoords = new Dictionary<PieceComponent, Vector2Int>();
        private void PiecesMovementManager_OnPieceMovementEnded(PieceComponent piece)
        {
            if (collapsingPiecesCoords.TryGetValue(piece, out var coord))
            {
                collapsingPiecesCoords.Remove(piece);
                TryCollapsePieceFromCoord(coord);
            }

            if (collapsingPiecesCoords.Count <= 0)
            {
                piecesMovementManager.OnPieceMovementEnded -= PiecesMovementManager_OnPieceMovementEnded;
                // OnPiecesColapsed?.Invoke();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            if (directionsByCoord == null)
                return;
            
            foreach (var kvp in directionsByCoord)
            {
                var coord = kvp.Key;
                var directions = kvp.Value;
                for (int i = 0; i < directions.Count; i++)
                {
                    var direction = directions[i];
                    var targetCoord = coord + direction;
                    if (BoardComponent.ContainsCoord(targetCoord))
                    {
                        var startPosition = BoardComponent.CoordToWorld(coord);
                        var endPosition = BoardComponent.CoordToWorld(targetCoord);
                        Gizmos.DrawLine(startPosition, endPosition);
                    }
                }
            }
        }
    }
}
