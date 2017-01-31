using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.Utils;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace pkgdiff.tests
{
    [TestFixture]
    public class TestPackageDiffCommandlineTool
    {
        [Test]
        public void Diff_GivenFilePaths_ShouldReadFiles()
        {
            //--------------- Arrange -------------------
            var left = GetRandomString();
            var right = GetRandomString();
            var reader = Substitute.For<ITextFileReader>();
            var sut = Create(reader);

            //--------------- Assume ----------------

            //--------------- Act ----------------------

            sut.Diff(left, right);

            //--------------- Assert -----------------------
            reader.Received(1).Read(left);
            reader.Received(1).Read(right);
        }

        [Test]
        public void Diff_ShouldPassFileContentsToGenerator()
        {
            //--------------- Arrange -------------------
            var left = GetRandomString();
            var right = GetRandomString();
            var leftContents = GetRandomString();
            var rightContents = GetRandomString();
            var reader = CreateTextFileReaderFor(left, leftContents, right, rightContents);
            var generator = Substitute.For<IPackageDiffGenerator>();
            var sut = Create(reader, generator);

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            sut.Diff(left, right);

            //--------------- Assert -----------------------
            generator.Received(1).Diff(leftContents, rightContents);
        }

        [Test]
        public void Diff_ShouldPassGeneratedDiffsTo_Reporter()
        {
            //--------------- Arrange -------------------
            var expected = GetRandomCollection<IPackageDifference>(2, 4);
            var generator = Substitute.For<IPackageDiffGenerator>();
            generator.Diff(Arg.Any<string>(), Arg.Any<string>()).Returns(expected);
            var reporter = Substitute.For<IPackageDiffReporter>();
            var sut = Create(generator: generator, reporter: reporter);

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            sut.Diff(GetRandomString(), GetRandomString());

            //--------------- Assert -----------------------
            expected.ForEach(e => reporter.Received(1).Translate(e));
        }

        [Test]
        public void Diff_ShouldPrintOutResultsWithConsoleWriter()
        {
            //--------------- Arrange -------------------
            var diff1 = Substitute.For<IPackageDifference>();
            var diff2 = Substitute.For<IPackageDifference>();
            var generator = Substitute.For<IPackageDiffGenerator>();
            generator.Diff(Arg.Any<string>(), Arg.Any<string>())
                .Returns(new[] { diff1, diff2 });
            var expected1 = GetRandom<IConsoleMessage>();
            var expected2 = GetRandom<IConsoleMessage>();
            var reporter = Substitute.For<IPackageDiffReporter>();
            reporter.Translate(diff1).Returns(expected1);
            reporter.Translate(diff2).Returns(expected2);
            var writer = Substitute.For<IPackageDiffItemWriter>();
            var sut = Create(generator: generator, reporter: reporter, writer: writer);

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            sut.Diff(GetRandomString(), GetRandomString());

            //--------------- Assert -----------------------
            Received.InOrder(() =>
            {
                writer.Write(expected1.Color, expected1.Message);
                writer.Write(expected2.Color, expected2.Message);
            });
        }



        private static ITextFileReader CreateTextFileReaderFor(string left, string leftContents, string right, string rightContents)
        {
            var reader = Substitute.For<ITextFileReader>();
            reader.Read(left).Returns(leftContents);
            reader.Read(right).Returns(rightContents);
            return reader;
        }


        private IPackageDiffCommandlineTool Create(
            ITextFileReader reader = null,
            IPackageDiffGenerator generator = null,
            IPackageDiffReporter reporter = null,
            IPackageDiffItemWriter writer = null)
        {
            return new PackageDiffCommandlineTool(
                reader ?? Substitute.For<ITextFileReader>(),
                generator ?? Substitute.For<IPackageDiffGenerator>(),
                reporter ?? Substitute.For<IPackageDiffReporter>(),
                writer ?? Substitute.For<IPackageDiffItemWriter>()
            );
        }
    }
}