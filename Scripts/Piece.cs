using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class Piece : MonoBehaviour
    {
        public event System.Action<IPieceType> OnTypeChanged;
        public event System.Action<Piece> OnCleared;

        public virtual IPieceType Type 
        {
            get => default;
            set 
            {
                OnTypeChanged?.Invoke(value);
            } 
        }

        private bool isCleared = false;
        public bool IsCleared
        {
            get => isCleared;
            set
            {
                isCleared = value;
                if (isCleared)
                    Invoke(nameof(CallClearedEvent), 0);
            }
        }

        private void CallClearedEvent()
        {
            OnCleared?.Invoke(this);
        }

        protected virtual void OnValidate()
        {
            Type = Type;
        }
    }

    public abstract class Piece<T> : Piece
        where T : Object, IPieceType
    {
        [SerializeField]
        private T type;
        public override IPieceType Type
        {
            get => type;
            set
            {
                type = value as T;
                if (type)
                    gameObject.name = $"Piece ({type.name})";
                base.Type = type;
            }
        }
    }
}
