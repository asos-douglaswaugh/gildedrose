using NUnit.Framework;
using System.Reflection;
using System.Text;

namespace GildedRose
{
    [TestFixture]
    public class ApprovalTest
    {
        [Test]
        public void ThirtyDays()
        {
            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ThirtyDays.txt");

            var referenceLines = File.ReadAllLines(filePath);

            var fakeoutput = new StringBuilder();
            Console.SetOut(new StringWriter(fakeoutput));
            Console.SetIn(new StringReader("\n"));

            Program.Main(new string[] { });
            var output = fakeoutput.ToString();

            var outputLines = output.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            for (var i = 0; i < Math.Min(referenceLines.Length, outputLines.Length); i++)
            {
                Assert.AreEqual(referenceLines[i], outputLines[i]);
            }
        }
    }
}
