namespace Stencil.Samples
{
    public class Bar : IBar
    {
        public IFoo Foo { get; set; }

        public string SayBar()
        {
            return Foo.SayFoo();
        }
    }
}
