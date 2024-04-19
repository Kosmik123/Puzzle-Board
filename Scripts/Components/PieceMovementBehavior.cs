using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
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
        public static void MoveTo(this PieceComponent piece, Vector3 targetPosition, float speed = -1)
        {
            if (piece.TryGetComponent<PieceMovementBehavior>(out var pieceMovement))
            {
                pieceMovement.MoveTo(targetPosition, speed);
            }
            else
            {
                piece.transform.position = targetPosition;
            }
        }
    }
}
