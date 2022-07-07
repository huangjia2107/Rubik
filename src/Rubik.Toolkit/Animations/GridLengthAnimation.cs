using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Rubik.Toolkit.Animations
{
    public class GridLengthAnimation : AnimationTimeline
    {
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(GridLength), typeof(GridLengthAnimation));
        public GridLength From
        {
            get { return (GridLength)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(GridLength), typeof(GridLengthAnimation));
        public GridLength To
        {
            get { return (GridLength)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(GridLengthAnimation), new PropertyMetadata(null));
        public IEasingFunction EasingFunction
        {
            get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }

        #region Override

        public override Type TargetPropertyType => typeof(GridLength);

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            var fromVal = From.Value;
            var toVal = To.Value;
            var progress = animationClock.CurrentProgress.Value;

            var easingFunction = EasingFunction;
            if (easingFunction != null)
                progress = easingFunction.Ease(progress);

            if (fromVal > toVal)
                return new GridLength((1 - progress) * (fromVal - toVal) + toVal, From.GridUnitType);
            else
                return new GridLength(progress * (toVal - fromVal) + fromVal, To.GridUnitType);
        }

        #endregion
    }
}
