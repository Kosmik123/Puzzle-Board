namespace Bipolar.PuzzleBoard
{
    public interface IPieceProperty
    { }

    [System.Serializable]
    public struct FrozenPieceProperty : IPieceProperty
    {
        public int hitPoints;
    }

    [System.Serializable]
    public readonly struct ImmovablePieceProperty : IPieceProperty
    { }

    [System.Serializable]
    public readonly struct CrossBomb : IPieceProperty
    { }   
    
    [System.Serializable]
    public readonly struct XCrossBomb : IPieceProperty
    { }   
    
    [System.Serializable]
    public readonly struct HorizontalBomb : IPieceProperty
    { }  
    
    [System.Serializable]
    public readonly struct VerticalBomb : IPieceProperty
    { }
    
    [System.Serializable]
    public readonly struct ColorBomb : IPieceProperty
    { }
}
