using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [RequireComponent(typeof(SceneBoard<>))]
    public abstract class PiecesMover<TStrategy, TBoard> : MonoBehaviour
        where TBoard : Board
        where TStrategy : BoardCollapseStrategy<TBoard>
    {
        public bool IsMoving { get; protected set; }

        private SceneBoard<TBoard> _sceneBoard;
        public SceneBoard<TBoard> SceneBoard
        { 
            get
            {
                if (_sceneBoard == null)
                    _sceneBoard = GetComponent<SceneBoard<TBoard>>();    
                
                return _sceneBoard;
            }
        }    

        public abstract void HandleCollapseMovemement(TStrategy strategy, IReadOnlyList<ICollapseEventArgs> collapseEvents);
    }
}
