using System;

namespace Stencil.Samples
{
    public class Baz : IBaz
    {
        // Should be null
        public IBaz RecursiveBaz { get; set; }

        public string SayBaz()
        {
            return "baz";
        }
    }
}
