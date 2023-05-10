using System;
using System.Collections.Generic;

namespace tic_tac_toe_ui.WPF.Animation
{
    public class AnimatorList
    {
        private List<Animator> animators = new List<Animator>();

        public event EventHandler AnimationsCompleted;

        public bool IsEmpty => animators.Count == 0;

        public void Enqueue(Animator animator)
        {
            animators.Add(animator);
            if (animators.Count == 1)
            {
                animators[0].Start(OnAnimationCompleted);
            }       
        }

        private void OnAnimationCompleted(object? sender, EventArgs e)
        {
            animators.RemoveAt(0);
            if (!IsEmpty)
            {
                animators[0].Start(OnAnimationCompleted);
                return;
            }
            AnimationsCompleted?.Invoke(sender, e);
        }
    }
}
