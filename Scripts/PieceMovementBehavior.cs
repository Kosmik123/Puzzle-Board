using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PieceComponent))]
    public abstract class PieceMovementBehavior : MonoBehaviour
    {
        public abstract event System.Action<PieceMovementBehavior> OnMovementEnded;

        public abstract void MoveTo(Vector3 targetPosition, float speed = -1);
    }

    public static class PieceMovementExtension
    {
        public static void MoveTo(this PieceComponent piece, Vector3 targetPosition, System.Action moveFinishedCallback = null, float speed = -1)
        {
            if (piece.TryGetComponent<PieceMovementBehavior>(out var pieceMovement) && pieceMovement.isActiveAndEnabled)
            {
                pieceMovement.MoveTo(targetPosition, speed);
            }
            else
            {
                piece.transform.position = targetPosition;
                moveFinishedCallback?.Invoke();
            }
        }
    }
}
