/* ------------------------------------------------------------------------------------------------
| Red Ball, Blue Ball - Demonstrate real-time speed and direction updates.                        |
| Copyright (C) 2023  Jared Sevilla                                                               |
|                                                                                                 |
| This program is free software: you can redistribute it and/or modify                            |
| it under the terms of the GNU General Public License as published by                            |
| the Free Software Foundation, either version 3 of the License, or                               |
| (at your option) any later version.                                                             |
|                                                                                                 |
| This program is distributed in the hope that it will be useful,                                 |
| but WITHOUT ANY WARRANTY; without even the implied warranty of                                  |
| MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                                   |
| GNU General Public License for more details.                                                    |
|                                                                                                 |
| You should have received a copy of the GNU General Public License                               |
| along with this program.  If not, see <https://www.gnu.org/licenses/>.                          |
------------------------------------------------------------------------------------------------ */

/** 
 * file: Vector.cs
 * purpose: Defines a vector class for the Red Ball, Blue Ball program.
 * author: Jared Sevilla
 * email: jgsevilla@csu.fullerton.edu
 * course: CPSC223N
 * program: Red Ball, Blue Ball
 * due: 11 December, 2023
**/

// ***** PROGRAM STARTS HERE **********************************************************************
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
// ***** END PROGRAM ******************************************************************************