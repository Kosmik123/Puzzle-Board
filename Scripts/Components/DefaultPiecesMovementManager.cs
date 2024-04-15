using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public delegate void PieceMovementEndEventHandler(PieceComponent piece);

    [RequireComponent(typeof(BoardComponent))]
    public class DefaultPiecesMovementManager : PiecesMovementManager
    {
        public override event System.Action OnAllPiecesMovementStopped;
        public event PieceMovementEndEventHandler OnPieceMovementEnded;

        [SerializeField]
        private BoardComponent board;
        [SerializeField]
        private float defaultMovementDuration = 0.3f;

        private readonly Dictionary<PieceComponent, Coroutine> pieceMovementCoroutines = new Dictionary<PieceComponent, Coroutine>();
        public override bool ArePiecesMoving => pieceMovements.Count > 0;

        private readonly Dictionary<PieceComponent, (Vector3 Target, float Speed)> pieceMovements = new Dictionary<PieceComponent, (Vector3 Target, float Speed)>();

        public void StartPieceMovement(PieceComponent piece, Vector2Int targetCoord, float duration = -1)
        {
            if (duration < 0)
                duration = defaultMovementDuration;

            var target = board.CoordToWorld(targetCoord);
            float distance = Vector3.Distance(piece.transform.position, target);
            float speed = distance / duration;
            pieceMovements[piece] = (target, speed);
        }

        private List<PieceComponent> stoppedPieces = new List<PieceComponent>();
        private void LateUpdate()
        {
            float dt = Time.deltaTime;
            bool pieceMoved = false;
            foreach (var pieceMovement in pieceMovements)
            {
                pieceMoved = true;
                float distance = dt * pieceMovement.Value.Speed;
                MovePiece(pieceMovement.Key, pieceMovement.Value.Target, distance);
            }

            if (pieceMoved)
            {
                foreach (var piece in stoppedPieces)
                    pieceMovements.Remove(piece);

                stoppedPieces.Clear();
                if (ArePiecesMoving == false)
                    OnAllPiecesMovementStopped?.Invoke();
            }
        }

        private void MovePiece(PieceComponent piece, Vector3 target, float distace)
        {
            piece.transform.position = Vector3.MoveTowards(piece.transform.position, target, distace);
            if (piece.transform.position == target)
                stoppedPieces.Add(piece);
        }
    }
}
