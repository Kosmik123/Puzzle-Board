using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IPieceColor
    { }

    public interface IVisualPieceColor : IPieceColor
    {
        Sprite Sprite { get; }
        Color Color { get; }
    }
}
