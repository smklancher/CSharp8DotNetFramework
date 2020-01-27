using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp8DotNetFramework
{
    public static class CSharp8NullableAttributes
    {
        static CSharp8NullableAttributes()
        {
            var x = new ConditionalPostConditions();
        }

        // Specify post-conditions: MaybeNull and NotNull
        // https://docs.microsoft.com/en-us/dotnet/csharp/nullable-attributes#specify-post-conditions-maybenull-and-notnull
        private static class PostConditions<T>
        {
            // I'm not clear about how this is supposed to work: https://github.com/dotnet/roslyn/issues/30953#issuecomment-509334985
            //[return: MaybeNull]
            //public T Find<T>(IEnumerable<T> sequence, Func<T, bool> match)
            //{
            //    return default(T);
            //}

            // null can be passed in, but is always non-null on return
            public static void EnsureCapacity([NotNull]ref T[]? storage)
            {
                storage = Array.Empty<T>();
            }
        }

        // Specify preconditions: AllowNull and DisallowNull
        // https://docs.microsoft.com/en-us/dotnet/csharp/nullable-attributes#specify-preconditions-allownull-and-disallownull
        private static class PreConditions
        {
            private static string? _comment;
            private static string screenName = GenerateRandomScreenName();

            // default is null, but client an only set non-null
            [DisallowNull]
            public static string? ReviewComment
            {
                get => _comment;
                set => _comment = value ?? throw new ArgumentNullException(nameof(value), "Cannot set to null");
            }

            // Allow null because the setter turns null into a default value
            [AllowNull]
            public static string ScreenName
            {
                get => screenName;
                set => screenName = value ?? GenerateRandomScreenName();
            }

            private static string GenerateRandomScreenName() => "hi";
        }

        // Specify conditional post-conditions: NotNullWhen, MaybeNullWhen, and NotNullIfNotNull
        // https://docs.microsoft.com/en-us/dotnet/csharp/nullable-attributes#specify-post-conditions-maybenull-and-notnull
        private class ConditionalPostConditions
        {
            // NotNullIfNotNull: returns null if and only if parameter is null
            [return: NotNullIfNotNull("url")]
            private static string? GetTopLevelDomainFromFullUrl(string? url)
            {
                if (url == null) { return null; }
                return url;
            }

            // NotNullWhen: Use for patterns like TryGet or IsNullOrEmpty
            private static bool TryGetMessage(string key, [NotNullWhen(true)] out string? message)
            {
                bool keyIsFound = string.IsNullOrEmpty(key);
                if (keyIsFound)
                {
                    message = "hi";
                }
                else
                {
                    message = null;
                }

                return keyIsFound;
            }

            // MaybeNullWhen: not clear on how this is used
        }
    }
}