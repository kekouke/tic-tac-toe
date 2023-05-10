using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace tic_tac_toe_ui.WPF.Animation
{
    public class Animator
    {
        private Shape target;
        private AnimationTimeline animation;
        private DependencyProperty property;

        public Animator(Shape target, DependencyProperty property, AnimationTimeline animation) 
        {
            this.target = target;
            this.property = property;
            this.animation = animation;
        }

        public void Start(EventHandler? onCompletedHandler = null)
        {
            if (onCompletedHandler != null)
            {
                animation.Completed += onCompletedHandler;
            }
            target.BeginAnimation(property, animation);
        }
    }
}
