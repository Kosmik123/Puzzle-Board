using Bipolar.PuzzleBoard.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public struct CollapseBoardCommand<TStrategy, TBoard> : IBoardCommand 
        where TBoard : Board
        where TStrategy : BoardCollapseStrategy<TBoard>
    {
        private readonly IReadOnlyList<ICollapseEventArgs> collapseEvents;
        private readonly PiecesMover<TStrategy, TBoard> piecesMover;
        private readonly TStrategy strategy;

        public CollapseBoardCommand(
            PiecesMover<TStrategy, TBoard> piecesMover,
            IReadOnlyList<ICollapseEventArgs> collapseEvents,
            TStrategy strategy)
        {
            this.collapseEvents = collapseEvents;
            this.piecesMover = piecesMover;
            this.strategy = strategy;
        }

        public IEnumerator Execute()
        {
            piecesMover.HandleCollapseMovemement(strategy, collapseEvents);
            var mover = piecesMover;
            yield return new WaitWhile(() => mover.IsMoving);
        }

        public override string ToString()
        {
            return $"Command to collapse {collapseEvents.Count} pieces";
        }
    }


}
