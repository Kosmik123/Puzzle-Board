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
}
