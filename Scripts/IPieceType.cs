using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IPieceType
    { }

    public interface ISpritePieceType : IPieceType
    {
        Sprite Sprite { get; }
        Color Color { get; }
    }
}
