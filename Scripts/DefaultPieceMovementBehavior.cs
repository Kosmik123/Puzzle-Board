using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class DefaultPieceMovementBehavior : PieceMovementBehavior
    {
        public override event System.Action<PieceMovementBehavior> OnMovementEnded;

        public readonly struct MovementData
        {
            public Vector3 TargetPosition { get; }
            public float Speed { get; }
            
            public MovementData(Vector3 targetPosition, float speed)
            {
                TargetPosition = targetPosition;
                Speed = speed;
            }
        }

        [SerializeField, Min(0.0001f)]
        private float defaultSpeed;

        private readonly Queue<MovementData> movementQueue = new Queue<MovementData>();
        private MovementData currentMovement;
        private bool hasMovement = false;


        public override void MoveTo(Vector3 targetPosition, float speed = -1)
        {
            if (speed < 0)
                speed = defaultSpeed;

            var movement = new MovementData(targetPosition, speed);
            // to jest przykład szybkiego fixa o którym ktoś może zapomnieć
            movementQueue.Enqueue(movement);
            //currentMovement = movement;
        }

        private void Update()
        {
            if (hasMovement == false && movementQueue.Count > 0)
            {
                currentMovement = movementQueue.Dequeue();
                hasMovement = true;
            }
        }

        private void FixedUpdate()
        {
            if (hasMovement)
            {
                var position = transform.position;
                float distance = currentMovement.Speed * Time.deltaTime;
                Vector3 target = currentMovement.TargetPosition;
                position = Vector3.MoveTowards(position, target, distance);
                transform.position = position;
                if (position == target)
                {
                    hasMovement = false;
                    if (movementQueue.Count <= 0)
                        OnMovementEnded?.Invoke(this);  
                }
            }
        }
    }
}
