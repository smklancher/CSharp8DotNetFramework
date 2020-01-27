using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp8DotNetFramework
{
    internal static class CSharp8
    {
        // Asynchronous streams
        // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#asynchronous-streams
        // requires https://www.nuget.org/packages/Microsoft.Bcl.AsyncInterfaces
        public static async Task AsyncStreamsAsync()
        {
            static async IAsyncEnumerable<int> GenerateSequence()
            {
                for (int i = 0; i < 20; i++)
                {
                    await Task.Delay(100).ConfigureAwait(false);
                    yield return i;
                }
            }

            await foreach (var number in GenerateSequence())
            {
                Console.WriteLine(number);
            }
        }

        // Default interface members
        // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#default-interface-methods
        public static void DefaultInterfaceMembers()
        {
            // Not possible in .NET Framework : https://stu.dev/csharp8-doing-unsupported-things/#c-8-0-feature-compatibility
        }

        // Indices and ranges
        // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#indices-and-ranges
        // Requires polyfill: https://www.nuget.org/packages/IndexRange/
        //  Additional steps required to define GetSubArray, which is needed to create subranges from arrays:
        //  https://github.com/bgrainger/IndexRange#define-getsubarrayt
        //  (added here via Directory.Build.props)
        public static void IndicesAndRanges()
        {
            var words = new string[]
            {
                            // index from start    index from end
                "The",      // 0                   ^9
                "quick",    // 1                   ^8
                "brown",    // 2                   ^7
                "fox",      // 3                   ^6
                "jumped",   // 4                   ^5
                "over",     // 5                   ^4
                "the",      // 6                   ^3
                "lazy",     // 7                   ^2
                "dog"       // 8                   ^1
            };              // 9 (or words.Length) ^0

            // writes "dog"
            Console.WriteLine($"The last word is {words[^1]}");

            // The following code creates a subrange with the words "quick", "brown", and "fox". It includes words[1] through words[3]. The element words[4] isn't in the range.
            var quickBrownFox = words[1..4];

            // The following code creates a subrange with "lazy" and "dog". It includes words[^2] and words[^1]. The end index words[^0] isn't included:
            var lazyDog = words[^2..^0];

            // The following examples create ranges that are open ended for the start, end, or both:
            var allWords = words[..]; // contains "The" through "dog".
            var firstPhrase = words[..4]; // contains "The" through "fox"
            var lastPhrase = words[6..]; // contains "the", "lazy" and "dog"

            // You can also declare ranges as variables:
            Range phrase = 1..4;

            // The range can then be used inside the [ and ] characters:
            var text = words[phrase];
        }

        // Null-coalescing assignment
        // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#null-coalescing-assignment
        public static void NullCoalesingAssignment()
        {
            List<int>? numbers = null;
            int? i = null;

            // You can use the ??= operator to assign the value of its right-hand operand to its
            // left-hand operand only if the left-hand operand evaluates to null.
            numbers ??= new List<int>();
            numbers.Add(i ??= 17);
            numbers.Add(i ??= 20);

            Console.WriteLine(string.Join(" ", numbers));  // output: 17 17
            Console.WriteLine(i);  // output: 17
        }

        // Stackalloc in nested expressions
        // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#stackalloc-in-nested-expressions
        public static void StackallocInNestedExpressions()
        {
            // Reference System.Memory to use Span on .NET Framework:
            // https://www.nuget.org/packages/System.Memory/
            //Span<int> numbers = stackalloc[] { 1, 2, 3, 4, 5, 6 };
            //var ind = numbers.IndexOfAny(stackalloc[] { 2, 4, 6, 8 });
            //Console.WriteLine(ind);  // output: 1
        }

        // Static local functions
        // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#static-local-functions
        public static int StaticLocalFunctions()
        {
            int y = 5;
            int x = 7;
            return Add(x, y);

            static int Add(int left, int right) => left + right;
        }

        // Using declarations
        // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#using-declarations
        public static int UsingDelcarations(IEnumerable<string> lines)
        {
            if (lines == null) { throw new ArgumentException("Cannot be null", nameof(lines)); }

            using var file = new System.IO.StreamWriter("WriteLines2.txt");

            // Notice how we declare skippedLines after the using statement.
            int skippedLines = 0;
            foreach (string line in lines)
            {
                if (!line.Contains("Second"))
                {
                    file.WriteLine(line);
                }
                else
                {
                    skippedLines++;
                }
            }

            // Notice how skippedLines is in scope here.
            return skippedLines;

            // file is disposed here
        }

        // Readonly members
        // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#readonly-members
        private struct ReadonlyMembers
        {
            public readonly double Distance => Math.Sqrt(X * X + Y * Y);

            public double X { get; set; }

            public double Y { get; set; }

            public readonly override string ToString() =>
                $"({X}, {Y}) is {Distance} from the origin";
        }

        // Unmanaged constructed types
        // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#unmanaged-constructed-types
        private static class UnmanagedConstructedTypes
        {
            public static void Example()
            {
                // Reference System.Memory to use Span on .NET Framework:
                // https://www.nuget.org/packages/System.Memory/
                //Span<Coords<int>> coordinates = stackalloc[]
                //{
                //    new Coords<int> { X = 0, Y = 0 },
                //    new Coords<int> { X = 0, Y = 3 },
                //    new Coords<int> { X = 4, Y = 0 }
                //};

                var x = new Coords<int> { X = 4, Y = 0 };
            }

            public struct Coords<T>
            {
                public T X;

                public T Y;

                public Coords(T x, T y) : this()
                {
                    X = x;
                    Y = y;
                }
            }
        }
    }
}