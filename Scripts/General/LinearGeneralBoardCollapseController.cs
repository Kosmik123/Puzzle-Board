using Bipolar.PuzzleBoard.General;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Bipolar.PuzzleBoard.Components
{
    public abstract class MockGeneralCollapseStrategy : BoardCollapseStrategy<GeneralBoard>
    { }

    [RequireComponent(typeof(GeneralBoardComponent))]
    public class LinearGeneralBoardCollapseController : BoardCollapseController<LinearGeneralBoardCollapseStrategy, GeneralBoard>
    {
        [SerializeField]
        private Tilemap directionsMap;

        [SerializeField]
        private LinearGeneralBoardPiecesMovementManager piecesMovementManager;

        public override LinearGeneralBoardCollapseStrategy Strategy
        {
            get
            {
                CreateLines();
                return base.Strategy;
            }
        }

        private void Awake()
        {
            CreateLines();
        }

        [ContextMenu("Refresh")]
        private void CreateLines()
        {
            var directions = new Dictionary<Vector2Int, Vector2Int>();

            var startingCoords = new HashSet<Vector2Int>();
            var endingCoords = new HashSet<Vector2Int>();
            var notStartingCoords = new HashSet<Vector2Int>();
            var notEndingCoords = new HashSet<Vector2Int>();

            var tempTargetCoordsDict = new Dictionary<Vector2Int, Vector2Int>();
            var tempSourceCoordsDict = new Dictionary<Vector2Int, Vector2Int>();

            bool isBoardHexagonal = BoardComponent.Layout == GridLayout.CellLayout.Hexagon;
            foreach (var coord in BoardComponent.Board)
            {
                HandleCoordCreation(coord, isBoardHexagonal, startingCoords, endingCoords,
                    notStartingCoords, notEndingCoords, directions, tempSourceCoordsDict);
            }

            startingCoords.ExceptWith(notStartingCoords);
            endingCoords.ExceptWith(notEndingCoords);

            foreach (var kvp in tempSourceCoordsDict)
                tempTargetCoordsDict[kvp.Value] = kvp.Key;

            var lines = new CoordsLine[startingCoords.Count];
            int lineIndex = 0;
            foreach (var coord in startingCoords)
            {
                lines[lineIndex] = CreateCoordsLine(coord, tempTargetCoordsDict);
                lineIndex++;
            }
            strategy = new LinearGeneralBoardCollapseStrategy(directions, lines);
        }

        private void HandleCoordCreation(Vector2Int coord, bool isBoardHexagonal,
            HashSet<Vector2Int> startingCoords, HashSet<Vector2Int> endingCoords,
            HashSet<Vector2Int> notStartingCoords, HashSet<Vector2Int> notEndingCoords,
            Dictionary<Vector2Int, Vector2Int> directionsByCoord,
            Dictionary<Vector2Int, Vector2Int> targetToSourceDictionary)
        {
            if (DirectionTileHelper.TryGetTile(coord, directionsMap, out var tile) == false)
                return;

            var direction = DirectionTileHelper.GetTileDirection(coord, tile, isBoardHexagonal);
            directionsByCoord.Add(coord, direction);

            startingCoords.Add(coord);
            if (direction == Vector2Int.zero)
            {
                endingCoords.Add(coord);
                return;
            }

            var targetCoord = coord + direction;
            if (tile.Jump)
            {
                bool hasJumpTarget = gameObject;
                while (TilemapContainsCoord(((GeneralBoardComponent)BoardComponent).ShapeTilemap, targetCoord))
                {
                    if (BoardComponent.ContainsCoord(targetCoord))
                    {
                        hasJumpTarget = true;
                        break;
                    }
                    targetCoord += DirectionTileHelper.GetTileDirection(targetCoord, tile, isBoardHexagonal);
                }

                if (hasJumpTarget == false)
                {
                    endingCoords.Add(coord);
                    return;
                }
            }
            else if (BoardComponent.ContainsCoord(targetCoord) == false)
            {
                endingCoords.Add(coord);
                return;
            }

            notEndingCoords.Add(coord);
            endingCoords.Add(targetCoord);
            notStartingCoords.Add(targetCoord);

            if (targetToSourceDictionary.ContainsKey(targetCoord) == false)
                targetToSourceDictionary.Add(targetCoord, coord);
        }

        public static bool TilemapContainsCoord(Tilemap tilemap, Vector2Int coord)
        {
            var bounds = tilemap.cellBounds;
            return coord.y >= bounds.yMin && coord.y < bounds.yMax && coord.x >= bounds.xMin && coord.x < bounds.xMax;
        }

        private CoordsLine CreateCoordsLine(Vector2Int startingCoord, IReadOnlyDictionary<Vector2Int, Vector2Int> targetCoordsDict)
        {
            var coordsList = new List<Vector2Int>();

            var coord = startingCoord;
            while (IndexOfCoordInBoard(coord) >= 0)
            {
                coordsList.Add(coord);
                if (targetCoordsDict.TryGetValue(coord, out var target) == false)
                    break;

                coord = target;
            }

            return new CoordsLine(coordsList);
        }

        private int IndexOfCoordInBoard(Vector2Int checkedCoord)
        {
            int index = -1;
            foreach (var coord in BoardComponent.Board)
            {
                index++;
                if (coord == checkedCoord)
                    return index;
            }

            return index;
        }

        private int CollapseTokensInLine(CoordsLine line)
        {
            int nonExistingPiecesCount = 0;
            for (int index = line.Coords.Count - 1; index >= 0; index--)
            {
                var coord = line.Coords[index];
                var piece = BoardComponent.GetPiece(coord);
                if (piece == null || piece.IsCleared)
                {
                    nonExistingPiecesCount++;
                }
                else if (nonExistingPiecesCount > 0)
                {
                    var targetCoord = line.Coords[index + nonExistingPiecesCount];
                   // Board[coord] = null;
                   // Board[targetCoord] = piece.Piece;
                   // piecesMovementManager.StartPieceMovement(piece, line, index);
                }
            }
            return nonExistingPiecesCount;
        }

        private void RefillLine(CoordsLine line, int count)
        {
            var startCoord = line.Coords[0];
            var creatingDirection = -GetRealDirection(startCoord);
            var firstCellPosition = BoardComponent.CoordToWorld(startCoord);
            for (int i = 0; i < count; i++)
            {
                var coord = line.Coords[i];
                PieceComponent newPiece = null; // CreatePiece(coord);
                var spawningPosition = firstCellPosition + (Vector3)(creatingDirection * (count - i));
                newPiece.transform.position = spawningPosition;
                piecesMovementManager.StartPieceMovement(newPiece, line, -1);
            }
        }

        public Vector2Int GetDirection(Vector2Int coord) => throw new System.Exception();//directions[coord];

        public Vector2 GetRealDirection(Vector2Int coord)
        {
            var direction = GetDirection(coord);
            var nextCoord = coord + direction;

            var worldPos = BoardComponent.CoordToWorld(coord);
            var nextWorldPos = BoardComponent.CoordToWorld(nextCoord);

            return nextWorldPos - worldPos;
        }

        private void CallCollapseEvent()
        {
            piecesMovementManager.OnAllPiecesMovementStopped -= CallCollapseEvent;
            // OnPiecesColapsed?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            if (strategy != null && strategy.Lines != null && (BoardComponent as GeneralBoardComponent).Coords.Count > 0)
                foreach (var line in strategy.Lines)
                    GizmosDrawLine(line);

            void GizmosDrawLine(CoordsLine line)
            {
                for (int i = 0; i < line.Coords.Count; i++)
                {
                    var coord = line.Coords[i];
                    if (i > 0)
                    {
                        var sourceCoord = line.Coords[i - 1];
                        GizmosDrawLineSegment(sourceCoord, coord);
                    }

                    if (i == 0)
                        GizmosDrawLineStart(coord);

                    if (i == line.Coords.Count - 1)
                        GizmosDrawLineEnd(coord);
                }
            }

            void GizmosDrawLineSegment(Vector2Int start, Vector2Int end)
            {
                var startPos = BoardComponent.CoordToWorld(start);
                var target = BoardComponent.CoordToWorld(end);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(startPos, target);
            }

            void GizmosDrawLineStart(Vector2Int coord) => GizmosDrawLineTip(coord, Color.green, -0.1f);
            void GizmosDrawLineEnd(Vector2Int coord) => GizmosDrawLineTip(coord, Color.red, 0.1f);
            void GizmosDrawLineTip(Vector2Int coord, Color color, float offset)
            {
                if (DirectionTileHelper.TryGetTile(coord, directionsMap, out var tile))
                {
                    Gizmos.color = color;
                    Gizmos.DrawSphere(BoardComponent.CoordToWorld(coord) + (Vector3)(offset * (Vector2)tile.Direction), 0.1f);
                }
            }
        }
    }
}
