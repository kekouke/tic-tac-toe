using System.Windows;
using System.Windows.Media;

namespace tic_tac_toe_ui.WPF.Shapes
{
    public static class FigureProvider
    {
        public static Circle CreateCircle(Point center)
        {
            return new Circle()
            {
                Center = center,
                Radius = 30,
                Stroke = Brushes.Blue,
                StrokeThickness = 3,
            };
        }

        public static Cross CreateCross(Point center)
        {
            return new Cross()
            {
                Center = center,
                Stroke = Brushes.Red,
                StrokeThickness = 3,
            };
        }
    }
}
