using Bipolar.PuzzleBoard;

namespace Bipolar.PuzzleBoard
{
    public interface ICollapseEventArgs
    { 
        BoardPiece Piece { get; }
    }
}

public interface IPieceCreatedCollapseEventArgs : ICollapseEventArgs
{
    int CreateIndex { get; }
}

public readonly struct PieceCreatedEventArgs : IPieceCreatedCollapseEventArgs
{
    public readonly BoardPiece Piece { get; }
    public int CreateIndex { get; }

    public PieceCreatedEventArgs(BoardPiece piece, int indexInLine)
    {
        Piece = piece;
        CreateIndex = indexInLine;
    }

    public override string ToString()
    {
        return $"New piece was created at {Piece.Coord} event";
    }
}
