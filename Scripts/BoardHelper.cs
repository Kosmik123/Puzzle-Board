using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public static class BoardHelper
    {
        public static Vector2Int GetCorrectedDirection(Vector2Int fromCoord, Vector2Int direction, bool isHexagonal)
        {
            if (isHexagonal && direction.y != 0)
            {
                if (fromCoord.y % 2 == 0)
                {
                    if (direction.x > 0)
                        direction.x = 0;
                }
                else
                {
                    if (direction.x <= 0)
                        direction.x += 1;
                }
            }
            return direction;
        }

        public static readonly Vector2Int[] defaultBoardDirections =
{
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.down
        };

        public static readonly Vector2Int[] hexagonalBoardDirections =
        {
            Vector2Int.right,
            Vector2Int.up + Vector2Int.right,
            Vector2Int.up + Vector2Int.left,
            Vector2Int.left,
            Vector2Int.down + Vector2Int.left,
            Vector2Int.down + Vector2Int.right,
        };

        public static IReadOnlyList<Vector2Int> GetDirections(GridLayout.CellLayout layout) => GetDirections(layout == GridLayout.CellLayout.Hexagon);

        public static IReadOnlyList<Vector2Int> GetDirections(bool isHexagonal) => isHexagonal
            ? (IReadOnlyList<Vector2Int>)hexagonalBoardDirections
            : defaultBoardDirections;
    }
}
