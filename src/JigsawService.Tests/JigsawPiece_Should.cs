using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;

namespace JigsawService.Images.Pieces.Tests
{
    [TestFixture]
    internal sealed class JigsawPiece_Should
    {
        [Test]
        public void IgnoreOutboundsPixel_WhenEnumeratePixels()
        {
            var piece = new JigsawPiece(
                origin: new Image<Rgba32>(10, 10),
                location: new Point(0, 0),
                jigsawPosition: new Point(0, 0),
                shape: new PathBuilder()
                            .AddLine(0, 0, 100, 0)
                            .AddLine(100, 0, 50, 100)
                            .AddLine(50, 100, 0, 0)
                            .Build(),
                canvas: new Size(20, 20),
                edges: new Edges());

            Assert.DoesNotThrow(() => piece.ToPixels());
        }
    }
}