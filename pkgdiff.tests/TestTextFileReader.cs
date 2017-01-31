using System.Text;
using NUnit.Framework;
using PeanutButter.Utils;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace pkgdiff.tests
{
    [TestFixture]
    public class TestTextFileReader: AssertionHelper
    {
        [Test]
        public void Read_GivenPathToExistingFile_ShouldReturnContents()
        {
            //--------------- Arrange -------------------
            var expected = GetRandomString(50, 100);
            var sut = Create();
            using (var tempFile = new AutoTempFile(Encoding.UTF8.GetBytes(expected)))
            {
                //--------------- Assume ----------------

                //--------------- Act ----------------------
                var result = sut.Read(tempFile.Path);

                //--------------- Assert -----------------------
                Expect(result, Is.EqualTo(expected));
            }
        }

        private ITextFileReader Create()
        {
            return new TextFileReader();
        }
    }
}