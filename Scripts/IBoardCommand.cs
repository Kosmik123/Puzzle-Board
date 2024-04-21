using System.Collections;

namespace Bipolar.PuzzleBoard
{
    public interface IBoardCommand
    {
        public IEnumerator Execute();
    }
}
