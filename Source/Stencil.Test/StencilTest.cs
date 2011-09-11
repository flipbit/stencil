using NUnit.Framework;
using Stencil.Samples;

namespace Stencil
{
    [TestFixture]
    public class StencilTest
    {
        [SetUp]
        public void SetUp()
        {
            Stencil.Defaults.Assemblies.Clear();
            Stencil.Defaults.Assemblies.Add(typeof(StencilTest).Assembly);
        }

        [Test]
        public void TestResolveClass()
        {
            var service = Stencil.Instance.Resolve<IFoo>();

            Assert.IsNotNull(service);
            Assert.AreEqual("foo", service.SayFoo());
        }

        [Test]
        public void TestResolveChildClasses()
        {
            var service = Stencil.Instance.Resolve<IBar>();

            Assert.IsNotNull(service);
            Assert.AreEqual("foo", service.SayBar());
        }

        [Test]
        public void TestResolveRecursiveClasses()
        {
            var service = Stencil.Instance.Resolve<IBaz>() as Baz;

            Assert.IsNotNull(service);
            Assert.IsNull(service.RecursiveBaz);
            Assert.AreEqual("baz", service.SayBaz());
        }

        [Test]
        public void TestSingletons()
        {
            var one = Stencil.Instance.Resolve<IBar>();
            var two = Stencil.Instance.Resolve<IBar>();

            Assert.AreEqual(one, two);
        }

        [Test]
        public void TestNoneSingletons()
        {
            var options = new Options { UseSingletons = false };
            options.Assemblies.Add(typeof(StencilTest).Assembly);

            var stencil = new Stencil();
            stencil.Initilize(options);

            var one = stencil.Resolve<IBar>();
            var two = stencil.Resolve<IBar>();

            Assert.AreNotEqual(one, two);
        }

        [Test]
        public void TestCreateConcreteTypeWithDependencies()
        {
            var concrete = Stencil.Instance.Resolve<Concrete>();

            Assert.IsNotNull(concrete);
            Assert.IsNotNull(concrete.Foo);
            Assert.AreEqual("foo", concrete.SayFoo());
        }

        [Test]
        public void TestCreateConcreteTypeWithListOfDependencies()
        {
            var fizzbuzz = Stencil.Instance.Resolve<FizzBuzz>();

            Assert.IsNotNull(fizzbuzz);
            Assert.AreEqual(2, fizzbuzz.FizzBuzzers.Count);
            Assert.AreEqual("buzz", fizzbuzz.FizzBuzzers[0].SayFizzBuzz());
            Assert.AreEqual("fizz", fizzbuzz.FizzBuzzers[1].SayFizzBuzz());
        }

        [Test]
        public void TestResolveMultipleType()
        {
            var fizzbuzzers = Stencil.Instance.ResolveAll<IFizzBuzz>();

            Assert.AreEqual(2, fizzbuzzers.Count);
            Assert.AreEqual("buzz", fizzbuzzers[0].SayFizzBuzz());
            Assert.AreEqual("fizz", fizzbuzzers[1].SayFizzBuzz());
        }
    }
}
