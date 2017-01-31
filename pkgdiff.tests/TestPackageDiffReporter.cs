using System;
using NSubstitute;
using NUnit.Framework;

namespace pkgdiff.tests
{
    [TestFixture]
    public class TestPackageDiffReporter: AssertionHelper
    {
        [Test]
        public void Versions_CanTheyBeCompared()
        {
            //--------------- Arrange -------------------
            var left = new Version(1, 2, 3);
            var right = new Version(1, 2, 4);

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = left < right;

            //--------------- Assert -----------------------
            Expect(result, Is.True);
        }

        [TestCase(null, "1", "2", "No package id")]
        [TestCase("MooCakes", null, null, "Both package versions missing")]
        public void Translate_InvalidInput_ShouldShowError(string packageId, string left, string right, string submessage)
        {
            //--------------- Arrange -------------------
            var diff = CreateDiffFor(packageId, left, right);
            var sut = Create();

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = sut.Translate(diff);

            //--------------- Assert -----------------------
            Expect(result, Is.Not.Null);
            Expect(result.Color, Is.EqualTo(ConsoleColor.DarkRed));
            Expect(result.Message, Does.Contain(submessage));
        }


        [Test]
        public void Translate_GivenNoDiffDiff_ShouldReport_NoChange()
        {
            //--------------- Arrange -------------------
            var diff = CreateDiffFor("SomePackage", "1.2.3", "1.2.3");
            var sut = Create();

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = sut.Translate(diff);

            //--------------- Assert -----------------------
            Expect(result.Message, Is.EqualTo("Unchanged:   SomePackage"));
            Expect(result.Color, Is.EqualTo(ConsoleColor.Gray));
        }

        [Test]
        public void Translate_GivenUpgrade_ShouldReport_Upgraded()
        {
            //--------------- Arrange -------------------
            var diff = CreateDiffFor("APackage", "1.2.3", "1.2.4");
            var sut = Create();

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = sut.Translate(diff);

            //--------------- Assert -----------------------
            Expect(result.Message, Is.EqualTo("Upgraded:   APackage: 1.2.3 => 1.2.4"));
            Expect(result.Color, Is.EqualTo(ConsoleColor.Green));
        }

        [Test]
        public void Translate_GivenDowngrade_ShouldReport_Downgraded()
        {
            //--------------- Arrange -------------------
            var diff = CreateDiffFor("APackage", "1.2.4", "1.2.3");
            var sut = Create();

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = sut.Translate(diff);

            //--------------- Assert -----------------------
            Expect(result.Message, Is.EqualTo("Downgraded: APackage: 1.2.4 => 1.2.3"));
            Expect(result.Color, Is.EqualTo(ConsoleColor.Red));
        }

        [Test]
        public void Translate_GivenLefOnly_ShouldReport_LeftOnly()
        {
            //--------------- Arrange -------------------
            var diff = CreateDiffFor("APackage", "1.2.4", null);
            var sut = Create();

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = sut.Translate(diff);

            //--------------- Assert -----------------------
            Expect(result.Message, Is.EqualTo("Left only:  APackage: 1.2.4"));
            Expect(result.Color, Is.EqualTo(ConsoleColor.DarkYellow));
        }

        [Test]
        public void Translate_GivenRightOnly_ShouldReport_RightOnly()
        {
            //--------------- Arrange -------------------
            var diff = CreateDiffFor("APackage", null, "1.2.4");
            var sut = Create();

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = sut.Translate(diff);

            //--------------- Assert -----------------------
            Expect(result.Message, Is.EqualTo("Right only: APackage: 1.2.4"));
            Expect(result.Color, Is.EqualTo(ConsoleColor.Cyan));
        }


        private static IPackageDifference CreateDiffFor(string id, string leftVersion, string rightVersion)
        {
            var diff = Substitute.For<IPackageDifference>();
            diff.PackageId.Returns(id);
            diff.LeftVersion.Returns(leftVersion);
            diff.RightVersion.Returns(rightVersion);
            return diff;
        }

        private IPackageDiffReporter Create()
        {
            return new PackageDiffReporter();
        }
    }
}