using Bipolar.PuzzleBoard.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [RequireComponent(typeof(BoardComponent<>))]
    public abstract class PiecesMover<TStrategy, TBoard> : MonoBehaviour
        where TBoard : Board
        where TStrategy : BoardCollapseStrategy<TBoard>
    {
        public bool IsMoving { get; protected set; }

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

        public abstract void HandleCollapseMovemement(TStrategy strategy, IReadOnlyList<ICollapseEventArgs> collapseEvents);
    }
}
