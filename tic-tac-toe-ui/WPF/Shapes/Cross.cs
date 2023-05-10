using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace tic_tac_toe_ui.WPF.Shapes
{
    public class Cross : Shape
    {

        private Point leftToRightEndPoint;
        private Point rightToLeftEndPoint;

        private LineSegment leftToRight;
        private LineSegment rightToLeft;
        private PathGeometry geometry;

        protected override Geometry DefiningGeometry => geometry;

        public static DependencyProperty CenterProperty { private set; get; }
        public static DependencyProperty StrokeProgressProperty { private set; get; }

        static Cross()
        {
            CenterProperty = DependencyProperty.Register("Center",
                typeof(Point), typeof(Cross),
                new PropertyMetadata(new Point(), OnPropertyChanged));
            StrokeProgressProperty = DependencyProperty.Register("StrokeProgress",
                typeof(double), typeof(Cross),
                new PropertyMetadata(.0, OnPropertyChanged));
        }

        private static void OnPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs args
        ) {
            var cross = d as Cross;
            if (args.Property.Name == nameof(Center))
            {
                cross.BuildGeometry();
                return;
            }

            if (cross.StrokeProgress > 0.5)
            {
                double t = 2 * (cross.StrokeProgress - 0.5);
                double x = t * cross.rightToLeftEndPoint.X + (1 - t) * cross.rightToLeft.Point.X;
                double y = t * cross.rightToLeftEndPoint.Y + (1 - t) * cross.rightToLeft.Point.Y;
                cross.rightToLeft.Point = new Point(x, y);
            }
            else
            {
                double t = cross.StrokeProgress * 2;
                double x = t * cross.leftToRightEndPoint.X + (1 - t) * cross.leftToRight.Point.X;
                double y = t * cross.leftToRightEndPoint.Y + (1 - t) * cross.leftToRight.Point.Y;
                cross.leftToRight.Point = new Point(x, y); 
            }
        }

        public void BuildGeometry()
        {
            leftToRight = new LineSegment()
            {
                Point = new Point(Center.X - 30, Center.Y - 30),
            };
            rightToLeft = new LineSegment()
            {
                Point = new Point(Center.X + 30, Center.Y - 30),
            };
            leftToRightEndPoint = Center;
            leftToRightEndPoint.X += 30;
            leftToRightEndPoint.Y += 30;
            rightToLeftEndPoint = Center;
            rightToLeftEndPoint.X -= 30;
            rightToLeftEndPoint.Y += 30;

            geometry = new PathGeometry()
            {
                Figures =
                    {
                        new PathFigure()
                        {
                            Segments = { leftToRight },
                            StartPoint = leftToRight.Point,
                        },
                        new PathFigure()
                        {
                            Segments = { rightToLeft },
                            StartPoint = rightToLeft.Point,
                        },
                    }
            };
        }

        public Point Center
        {
            set { SetValue(CenterProperty, value); }
            get { return (Point)GetValue(CenterProperty); }
        }

        public double StrokeProgress
        {
            set { SetValue(StrokeProgressProperty, value); }
            get { return (double)GetValue(StrokeProgressProperty); }
        }
    }
}
