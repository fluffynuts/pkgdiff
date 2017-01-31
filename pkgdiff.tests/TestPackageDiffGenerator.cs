using System.Linq;
using NUnit.Framework;

namespace pkgdiff.tests
{
    [TestFixture]
    public class TestPackageDiffGenerator: AssertionHelper
    {
        [Test]
        public void Diff_GivenTwoEmptyPackageFiles_ShouldGiveBackNoDiffs()
        {
            //--------------- Arrange -------------------
            var left = "<packages></packages>";
            var sut = Create();

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = sut.Diff(left, left);

            //--------------- Assert -----------------------
            Expect(result, Is.Not.Null);
            Expect(result, Is.Empty);
        }

        [Test]
        public void Diff_GivenIdenticalPackageFiles_ShouldGiveBackNoDiffs()
        {
            //--------------- Arrange -------------------
            var left = @"<packages>
  <package id=""AutoMapper"" version=""5.1.1"" targetFramework=""net452"" />
</packages>";
            var sut = Create();

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = sut.Diff(left, left);

            //--------------- Assert -----------------------
            Expect(result, Is.Not.Null);
            Expect(result, Is.Empty);
        }

        [Test]
        public void Diff_GivenOneUpgradedPackageVersionDifference_ShouldReportIt()
        {
            //--------------- Arrange -------------------
            var left = @"<packages>
  <package id=""AutoMapper"" version=""5.1.1"" targetFramework=""net452"" /></packages>";
            var right = @"<packages>
  <package id=""AutoMapper"" version=""5.1.2"" targetFramework=""net452"" />
</packages>";
            var sut = Create();

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = sut.Diff(left, right);

            //--------------- Assert -----------------------
            Expect(result, Is.Not.Empty);
            var diff = result.Single();
            Expect(diff.PackageId, Is.EqualTo("AutoMapper"));
            Expect(diff.LeftVersion, Is.EqualTo("5.1.1"));
            Expect(diff.RightVersion, Is.EqualTo("5.1.2"));
        }

        [Test]
        public void Diff_GivenOneDowngradedPackageVersionDifference_ShouldReportIt()
        {
            //--------------- Arrange -------------------
            var left = @"<packages>
  <package id=""AutoMapper"" version=""5.1.2"" targetFramework=""net452"" /></packages>";
            var right = @"<packages>
  <package id=""AutoMapper"" version=""5.1.1"" targetFramework=""net452"" /></packages>";
            var sut = Create();

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = sut.Diff(left, right);

            //--------------- Assert -----------------------
            Expect(result, Is.Not.Empty);
            var diff = result.Single();
            Expect(diff.PackageId, Is.EqualTo("AutoMapper"));
            Expect(diff.LeftVersion, Is.EqualTo("5.1.2"));
            Expect(diff.RightVersion, Is.EqualTo("5.1.1"));
        }

        [Test]
        public void Diff_GivenOneRemovedPackage_ShouldReportItWith_NullRightVersion()
        {
            //--------------- Arrange -------------------
            var left = @"<packages>
  <package id=""AutoMapper"" version=""5.1.2"" targetFramework=""net452"" /></packages>";
            var right = @"<packages></packages>";
            var sut = Create();

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = sut.Diff(left, right);

            //--------------- Assert -----------------------
            Expect(result, Is.Not.Empty);
            var diff = result.Single();
            Expect(diff.PackageId, Is.EqualTo("AutoMapper"));
            Expect(diff.LeftVersion, Is.EqualTo("5.1.2"));
            Expect(diff.RightVersion, Is.Null);
        }

        [Test]
        public void Diff_GivenOneAddedPackage_ShouldReportItWith_NullLeftVersion()
        {
            //--------------- Arrange -------------------
            var left = @"<packages></packages>";
            var right = @"<packages>
  <package id=""AutoMapper"" version=""5.1.2"" targetFramework=""net452"" /></packages>";
            var sut = Create();

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = sut.Diff(left, right);

            //--------------- Assert -----------------------
            Expect(result, Is.Not.Empty);
            var diff = result.Single();
            Expect(diff.PackageId, Is.EqualTo("AutoMapper"));
            Expect(diff.LeftVersion, Is.Null);
            Expect(diff.RightVersion, Is.EqualTo("5.1.2"));
        }

        [Test]
        public void Diff_ShouldReturnResultsInOrderOfPackageId()
        {
            //--------------- Arrange -------------------
            var left = @"<packages>
  <package id=""AutoMapper"" version=""5.1.2"" targetFramework=""net452"" /></packages>";
            var right = @"<packages>
  <package id=""AutoFapper"" version=""5.1.2"" targetFramework=""net452"" /></packages>";
            var sut = Create();

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = sut.Diff(left, right);

            //--------------- Assert -----------------------
            Expect(result.Count(), Is.EqualTo(2));
            Expect(result.First().PackageId, Is.EqualTo("AutoFapper"));
            Expect(result.Last().PackageId, Is.EqualTo("AutoMapper"));
        }




        private IPackageDiffGenerator Create()
        {
            return new PackageDiffGenerator();
        }
    }
}
