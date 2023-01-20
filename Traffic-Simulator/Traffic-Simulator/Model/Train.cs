using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Traffic_Simulator.Const;

namespace Traffic_Simulator.Model
{
    public class Train
    {
        // Properties
        public int Id { get; set; }
        public Point Position { get; set; }
        public double Speed { get; set; }
        public double Direction { get; set; }
        public Brush Color { get; set; }
        public Rectangle Shape { get; set; }
        public TraversalDirection TraversalDirection { get; set; }

        // Constructor
        public Train(int id, Point position, double speed, double direction, Brush color, TraversalDirection traversalDirection)
        {
            Id = id;
            Position = position;
            Speed = speed;
            Direction = direction;
            Color = color;
            Shape = new Rectangle
            {
                Width = 50,
                Height = 80,
                Fill = color
            };
            TraversalDirection = traversalDirection;
        }

        // Method to update the car's position based on its speed and direction
        public void UpdatePosition()
        {
            double x = Position.X + (Math.Cos(Direction) * Speed);
            double y = Position.Y - (Math.Sin(Direction) * Speed);
            Position = new Point(x, y);
        }

        // Method to update the car's shape on the canvas
        public void UpdateShape(Canvas canvas)
        {
            Canvas.SetLeft(Shape, Position.X - (Shape.Width / 2));
            Canvas.SetTop(Shape, Position.Y - (Shape.Height / 2));
            if (!canvas.Children.Contains(Shape))
            {
                canvas.Children.Add(Shape);
            }
        }

        /// <summary>
        /// Method to changed the direction of the car (rotate it)
        /// </summary>
        public void UpdateDirection()
        {
            this.Shape.RenderTransform = new RotateTransform(this.Direction);
        }
    }
}
