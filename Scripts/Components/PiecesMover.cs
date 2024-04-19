using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    [RequireComponent(typeof(BoardComponent<>))]
    public abstract class PiecesMover<TStrategy, TBoard> : MonoBehaviour
        where TBoard : Board
        where TStrategy : BoardCollapseStrategy<TBoard>
    {
        private BoardComponent<TBoard> _boardComponent;
        public BoardComponent<TBoard> BoardComponent
        { 
            get
            {
                if (_boardComponent == null)
                    _boardComponent = GetComponent<BoardComponent<TBoard>>();    
                
                return _boardComponent;
            }
        }    

        public abstract void HandleCollapseMovemement(TStrategy strategy, ICollapseEventArgs collapseEventArgs);
    }
}
