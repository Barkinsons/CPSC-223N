using System;

namespace Vector {
    public class Vector2 {


        public double X {get; set;}
        public double Y {get; set;}

        public Vector2(double X, double Y) {
            this.X = X;
            this.Y = Y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b) {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator *(Vector2 v, double factor) {
            return new Vector2(v.X * factor, v.Y * factor);
        }

        public static Vector2 operator /(Vector2 v, double factor) {
            return new Vector2(v.X / factor, v.Y / factor);
        }

        public Vector2 Normalize() {
            double magnitude = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            X /= magnitude;
            Y /= magnitude;
            return this;
        }

        public static double DistanceSquared(Vector2 a, Vector2 b) {
            return Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2);
        }

        public static double Distance(Vector2 a, Vector2 b) {
            return Math.Sqrt(DistanceSquared(a, b));
        }
    }
}