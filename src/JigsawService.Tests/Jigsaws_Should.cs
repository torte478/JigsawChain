using System.Linq;
using NUnit.Framework;
using SixLabors.ImageSharp;
using JigsawService.Images.Pieces;

namespace JigsawService.Images.Tests
{
    internal sealed class Jigsaws_Should
    {
        [Test]
        [TestCase(10, 10, 960, 960)]
        [TestCase(30, 20, 1260, 840)]
        public void ResizeImagesToSquarePieces_WhenBuildPreview(
                                                int countX,
                                                int countY,
                                                int expectedX,
                                                int expectedY)
        {
            var jigsaws = Jigsaws.Create(
                size: new Size(1280, 960),
                pieces: new Size(countX, countY),
                buildPieces: (_, __) => Enumerable.Empty<IPiece>());

            Assert.That(jigsaws.Size, Is.EqualTo(new Size(expectedX, expectedY)));
        }
    }
}