using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CSharp8DotNetFramework
{
    internal class CSharp8Patterns
    {
        // Positional Patterns: https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#positional-patterns
        private class PositionalPatterns
        {
            public PositionalPatterns()
            {
                var x = new Point(0, 0);
            }

            public enum Quadrant
            {
                Unknown,
                Origin,
                One,
                Two,
                Three,
                Four,
                OnBorder
            }

            private static Quadrant GetQuadrant(Point point) => point switch
            {
                (0, 0) => Quadrant.Origin,
                var (x, y) when x > 0 && y > 0 => Quadrant.One,
                var (x, y) when x < 0 && y > 0 => Quadrant.Two,
                var (x, y) when x < 0 && y < 0 => Quadrant.Three,
                var (x, y) when x > 0 && y < 0 => Quadrant.Four,
                var (_, _) => Quadrant.OnBorder,
                _ => Quadrant.Unknown
            };

            public class Point
            {
                public Point(int x, int y) => (X, Y) = (x, y);

                public int X { get; }

                public int Y { get; }

                public void Deconstruct(out int x, out int y) =>
                    (x, y) = (X, Y);
            }
        }

        // Property Patterns: https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#property-patterns
        private class PropertyPatterns
        {
            public PropertyPatterns()
            {
                var x = new Address();
            }

            public static decimal ComputeSalesTax(Address location, decimal salePrice) =>
                location switch
                {
                    { State: "WA" } => salePrice * 0.06M,
                    { State: "MN" } => salePrice * 0.75M,
                    { State: "MI" } => salePrice * 0.05M,
                    // other cases removed for brevity...
                    _ => 0M
                };

            public class Address
            {
                public string State { get; set; } = "WA";
            }
        }

        // Switch expressions: https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#switch-expressions
        private class SwitchExpressions
        {
            public enum Rainbow
            {
                Red,
                Orange,
                Yellow,
                Green,
                Blue,
                Indigo,
                Violet
            }

            // reference to PresentationCore.dll
            public static Color FromRainbow(Rainbow colorBand) =>
                colorBand switch
                {
                    Rainbow.Red => Color.FromRgb(0xFF, 0x00, 0x00),
                    Rainbow.Orange => Color.FromRgb(0xFF, 0x7F, 0x00),
                    Rainbow.Yellow => Color.FromRgb(0xFF, 0xFF, 0x00),
                    Rainbow.Green => Color.FromRgb(0x00, 0xFF, 0x00),
                    Rainbow.Blue => Color.FromRgb(0x00, 0x00, 0xFF),
                    Rainbow.Indigo => Color.FromRgb(0x4B, 0x00, 0x82),
                    Rainbow.Violet => Color.FromRgb(0x94, 0x00, 0xD3),
                    _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(colorBand)),
                };

            public static void Test()
            {
                var x = Color.FromRgb(0xFF, 0x00, 0x00);
            }
        };

        // Tuple patterns: https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#tuple-patterns
        private class TuplePatterns
        {
            public static string RockPaperScissors(string first, string second)
                => (first, second) switch
                {
                    ("rock", "paper") => "rock is covered by paper. Paper wins.",
                    ("rock", "scissors") => "rock breaks scissors. Rock wins.",
                    ("paper", "rock") => "paper covers rock. Paper wins.",
                    ("paper", "scissors") => "paper is cut by scissors. Scissors wins.",
                    ("scissors", "rock") => "scissors is broken by rock. Rock wins.",
                    ("scissors", "paper") => "scissors cuts paper. Scissors wins.",
                    (_, _) => "tie"
                };
        }
    }
}