using System.Windows;
using System.Windows.Media.Animation;

namespace StartPro.External;

public class BindableDoubleAnimation : DoubleAnimationBase
{
    public DoubleAnimation InternalAnimation { get; }

    public double To
    {
        get => (double) GetValue(ToProperty);
        set => SetValue(ToProperty, value);
    }

    public static readonly DependencyProperty ToProperty =
        DependencyProperty.Register("To", typeof(double), typeof(BindableDoubleAnimation), new UIPropertyMetadata(0d, new PropertyChangedCallback((s, e) =>
        {
            BindableDoubleAnimation sender = (BindableDoubleAnimation) s;
            sender.InternalAnimation.To = (double) e.NewValue;
        })));

    public double From
    {
        get => (double) GetValue(FromProperty);
        set => SetValue(FromProperty, value);
    }

    public static readonly DependencyProperty FromProperty =
        DependencyProperty.Register("From", typeof(double), typeof(BindableDoubleAnimation), new UIPropertyMetadata(0d, new PropertyChangedCallback((s, e) =>
        {
            BindableDoubleAnimation sender = (BindableDoubleAnimation) s;
            sender.InternalAnimation.From = (double) e.NewValue;
        })));

    public BindableDoubleAnimation( )
    {
        InternalAnimation = new DoubleAnimation( );
    }

    protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock animationClock)
        => InternalAnimation.GetCurrentValue(defaultOriginValue, defaultDestinationValue, animationClock);

    protected override Freezable CreateInstanceCore( )
        => InternalAnimation.Clone( );
}
