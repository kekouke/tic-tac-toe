using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace tic_tac_toe_ui.WPF.Shapes
{
    public class Circle : Shape
    {
        private ArcSegment firstArcSegment;

        private ArcSegment secondArcSegment;

        private PathGeometry geometry;

        private Point endPointFirstSegment;

        protected override Geometry DefiningGeometry => geometry;

        public static DependencyProperty CenterProperty { private set; get; }

        public static DependencyProperty RadiusProperty { private set; get; }

        public static DependencyProperty AngleProperty { private set; get; }

        public static DependencyProperty PointProperty { private set; get; }

        static Circle()
        {
            CenterProperty = DependencyProperty.Register("Center",
                typeof(Point), typeof(Circle),
                new PropertyMetadata(new Point(), OnPropertyChanged));

            RadiusProperty = DependencyProperty.Register("Radius",
                typeof(double), typeof(Circle),
                new PropertyMetadata(0.0, OnPropertyChanged));

            AngleProperty = DependencyProperty.Register("Angle",
                typeof(double), typeof(Circle),
                new PropertyMetadata(0.0, OnPropertyChanged));

            PointProperty = DependencyProperty.Register("Point",
                typeof(Point), typeof(Circle));
        }

        static void OnPropertyChanged(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs args
        ) {
            var circle = obj as Circle;
            circle.Point = circle.CalculatePoint();
            if (args.Property.Name == nameof(Angle))
            {
                circle.UpdateArcSegmentViaAngle();
                return;
            }
            circle.RebuildGeometry();
        }

        private Point CalculatePoint()
        {
            double x = Center.X + Radius * Math.Sin(Math.PI * Angle / 180);
            double y = Center.Y - Radius * Math.Cos(Math.PI * Angle / 180);
            return new Point(x, y);
        }

        private void UpdateArcSegmentViaAngle()
        {
            firstArcSegment.Point = Angle <= 180 ? Point : endPointFirstSegment;
            secondArcSegment.Point = Angle > 180 ? Point : secondArcSegment.Point;
        }

        private void RebuildGeometry()
        {
            Point = CalculatePoint();
            firstArcSegment = new ArcSegment()
            {
                SweepDirection = SweepDirection.Clockwise,
                Size = new Size(Radius, Radius),
                Point = Point,
            };
            secondArcSegment = new ArcSegment()
            {
                SweepDirection = SweepDirection.Clockwise,
                Size = new Size(Radius, Radius),
                Point = new Point(Point.X, Point.Y + 2 * Radius),
            };
            endPointFirstSegment = secondArcSegment.Point;

            geometry = new PathGeometry()
            {
                Figures =
                    {
                        new PathFigure()
                        {
                            Segments = { firstArcSegment },
                            StartPoint = firstArcSegment.Point,
                        },
                        new PathFigure()
                        {
                            Segments = { secondArcSegment },
                            StartPoint = secondArcSegment.Point,
                        },
                    }
            };
        }

        public Point Center
        {
            set { SetValue(CenterProperty, value); }
            get { return (Point)GetValue(CenterProperty); }
        }

        public double Radius
        {
            set { SetValue(RadiusProperty, value); }
            get { return (double)GetValue(RadiusProperty); }
        }

        public double Angle
        {
            set { SetValue(AngleProperty, value); }
            get { return (double)GetValue(AngleProperty); }
        }

        public Point Point
        {
            protected set { SetValue(PointProperty, value); }
            get { return (Point)GetValue(PointProperty); }
        }
    }
}
