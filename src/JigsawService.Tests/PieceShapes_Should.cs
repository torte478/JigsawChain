using NUnit.Framework;

namespace JigsawService.Images.Pieces.Tests
{
    [TestFixture]
    internal sealed class PieceShapes_Should
    {
        [Test]
        public void AddSignleUnitOffset_WhenCalculateCanvas()
        {
            var shapes = PieceShapes.Create(3);

            Assert.That(shapes.Width, Is.EqualTo(5));
        }
    }
}