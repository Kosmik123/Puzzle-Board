using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public static class BoardHelper
    {
        public static Vector2Int GetFixedDirection(Vector2Int coord, Vector2Int direction, bool isHexagonal)
        {
            if (isHexagonal && direction.y != 0)
            {
                if (coord.y % 2 == 0)
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
    }
}
