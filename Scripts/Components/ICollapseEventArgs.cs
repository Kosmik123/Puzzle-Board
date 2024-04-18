using Bipolar.PuzzleBoard;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface ICollapseEventArgs
    { 
        Piece Piece { get; }
    }
}

public interface IPieceCreatedCollapseEventArgs : ICollapseEventArgs
{
    int CreateIndex { get; }
}

public readonly struct PieceCreatedEventArgs : IPieceCreatedCollapseEventArgs
{
    public readonly Piece Piece { get; }
    public int CreateIndex { get; }

    public PieceCreatedEventArgs(Piece piece, int indexInLine)
    {
        Piece = piece;
        CreateIndex = indexInLine;
    }

    public override string ToString()
    {
        return $"New piece was created at {Piece.Coord} event";
    }
}
