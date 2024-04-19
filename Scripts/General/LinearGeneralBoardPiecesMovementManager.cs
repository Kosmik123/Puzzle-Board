using Bipolar.PuzzleBoard.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    [RequireComponent(typeof(GeneralBoardComponent))]
    public class LinearGeneralBoardPiecesMovementManager : PiecesMovementManager
    {
        public override event System.Action OnAllPiecesMovementStopped;

        [SerializeField]
        private float piecesMovementSpeed = 8f;

        private GeneralBoardComponent _board;
        public GeneralBoardComponent Board
        {
            get
            {
                if (_board == null && this)
                    _board = GetComponent<GeneralBoardComponent>();
                return _board;
            }
        }

        private readonly Dictionary<PieceComponent, Coroutine> pieceMovementCoroutines = new Dictionary<PieceComponent, Coroutine>();
        public override bool ArePiecesMoving => pieceMovementCoroutines.Count > 0;

        public void StartPieceMovement(PieceComponent piece, CoordsLine line, int fromIndex)
        {
            var movementCoroutine = StartCoroutine(MovementCo(piece, line, fromIndex));
            pieceMovementCoroutines.Add(piece, movementCoroutine);
        }

        private IEnumerator MovementCo(PieceComponent piece, CoordsLine line, int fromIndex)
        {
            for (int startIndex = fromIndex; startIndex < line.Coords.Count - 1; startIndex++)
            {
                var targetIndex = startIndex + 1;
                var targetCoord = line.Coords[targetIndex];

                var startPosition = startIndex < 0 ? piece.transform.position : Board.CoordToWorld(line.Coords[startIndex]);
                var targetPosition = Board.CoordToWorld(targetCoord);
                float realDistance = Vector3.Distance(startPosition, targetPosition);

                float progressSpeed = piecesMovementSpeed / realDistance;

                piece.transform.position = startPosition;
                for (float progress = 0; progress < 1; progress += Time.deltaTime * progressSpeed)
                {
                    yield return null;
                    piece.transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
                }
                piece.transform.position = targetPosition;
                if (targetCoord == piece.Piece.Coord)
                    break;
            }
            
            pieceMovementCoroutines.Remove(piece);
            if (ArePiecesMoving == false)
                OnAllPiecesMovementStopped?.Invoke();
        }
    }
}
