using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class NonAdjacentPieceColorProvider : RandomPieceColorProvider
    {
        [SerializeField]
        private BoardComponent boardComponent;

        [SerializeField]
        private bool avoidAdjacentPieceColors = true;
        public bool AvoidAdjacentPieceColors
        {
            get => avoidAdjacentPieceColors;
            set => avoidAdjacentPieceColors = value;
        }

        private readonly HashSet<IPieceColor> forbiddenPieceColors = new HashSet<IPieceColor>();
        private readonly List<IPieceColor> tempAvailableColors = new List<IPieceColor>();

        protected virtual void Reset()
        {
            boardComponent = FindObjectOfType<BoardComponent>();
        }

        public override IPieceColor GetPieceColor(int x, int y)
        {
            if (avoidAdjacentPieceColors == false)
                base.GetPieceColor(x, y);

            forbiddenPieceColors.Clear();
            var coord = new Vector2Int(x, y);
            bool isHexagonal = boardComponent.Layout == GridLayout.CellLayout.Hexagon;

            var directions = BoardHelper.GetDirections(boardComponent.Layout);
            for (int i = 0; i < directions.Count; i++)
            {
                var otherCoord = coord + BoardHelper.GetCorrectedDirection(coord, directions[i], isHexagonal);
                var piece = boardComponent.GetPiece(otherCoord);
                if (piece != null)
                    forbiddenPieceColors.Add(piece.Color);
            }

            tempAvailableColors.Clear();
            foreach (var color in pieceColorsList)
                if (forbiddenPieceColors.Contains(color) == false)
                    tempAvailableColors.Add(color);

            if (tempAvailableColors.Count <= 0)
            {
                Debug.LogWarning("Couldn't find not adjacent piece color");
                return base.GetPieceColor(x, y);
            }

            return tempAvailableColors[Random.Range(0, tempAvailableColors.Count)];
        }
    }
}
