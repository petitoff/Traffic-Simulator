﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace Traffic_Simulator.Model
{
    public class Car
    {
        // Properties
        public Point Position { get; set; }
        public double Speed { get; set; }
        public double Direction { get; set; }
        public Brush Color { get; set; }
        public Ellipse Shape { get; set; }

        // Constructor
        public Car(Point position, double speed, double direction, Brush color)
        {
            Position = position;
            Speed = speed;
            Direction = direction;
            Color = color;
            Shape = new Ellipse
            {
                Width = 40,
                Height = 30,
                Fill = color
            };
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
