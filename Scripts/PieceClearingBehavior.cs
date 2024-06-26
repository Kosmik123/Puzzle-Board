﻿using System.Collections;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScenePiece))]
    public abstract class PieceClearingBehavior : MonoBehaviour
    {
        public event System.Action<PieceClearingBehavior> OnClearing;

        private ScenePiece _pieceComponent;
        public ScenePiece PieceComponent
        {
            get
            {
                if (_pieceComponent == null)
                    _pieceComponent = GetComponent<ScenePiece>();
                return _pieceComponent;
            }
        }

        protected abstract void ClearPiece();

        public void Clear()
        {
            OnClearing?.Invoke(this);
            ClearPiece();
        }

        protected void FinishClearing()
        {
            PieceComponent.IsCleared = true;
        }
    }

    public abstract class CoroutinePieceClearingBehavior : PieceClearingBehavior
    {
        protected abstract IEnumerator ClearingProcessCo();

        protected sealed override void ClearPiece()
        {
            StartCoroutine(ClearingCo());
        }

        private IEnumerator ClearingCo()
        {
            yield return ClearingProcessCo();
            PieceComponent.IsCleared = true;
        }
    }

    public static class PieceClearingExtension
    {
        public static void Clear(this ScenePiece piece)
        {
            if (piece.TryGetComponent<PieceClearingBehavior>(out var pieceClearing))
            {
                pieceClearing.Clear();
            }
            else
            {
                piece.IsCleared = true;
            }
        }
    }
}
