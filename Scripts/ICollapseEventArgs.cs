using Bipolar.PuzzleBoard;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface ICollapseEventArgs
    {
        IPiece Piece { get; }
    }
}

public interface IPieceCreatedCollapseEventArgs : ICollapseEventArgs
{
    int CreateIndex { get; }
    Vector2Int CreationCoord { get; }
}

public readonly struct PieceCreatedEventArgs : IPieceCreatedCollapseEventArgs
{
    public readonly IPiece Piece { get; }
    public int CreateIndex { get; }
    public Vector2Int CreationCoord { get; }

    public PieceCreatedEventArgs(IPiece piece, int createIndex, Vector2Int creationCoord)
    {
        Piece = piece;
        CreateIndex = createIndex;
        CreationCoord = creationCoord;
    }

    public override string ToString()
    {
        return $"New piece was created at {CreationCoord} event";
    }
}
