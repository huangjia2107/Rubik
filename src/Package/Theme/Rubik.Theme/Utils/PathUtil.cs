using System.Linq;
using System.Text.RegularExpressions;

namespace Rubik.Toolkit.Utils
{
    public static class PathUtil
    {
        public static string ResizeData(string originalData, double curWidth, double curHeight, double desiredWidth, double desiredHeight)
        {
            var pattern = @"^[a-zA-Z][0-9]+(\.[0-9]+)?$";
            var rw = curWidth / desiredWidth;
            var rh = curHeight / desiredHeight;

            var pathSpans = originalData.Split(' ');

            return string.Join(" ", pathSpans.Select(span =>
            {
                var pos = span.Split(',');
                var match = Regex.IsMatch(pos[0], pattern);
                var match1 = Regex.IsMatch(pos[1], pattern);

                return string.Format("{0}{1:#0.##},{2}{3:#0.##}",
                              match ? pos[0].Substring(0, 1) : "",
                              double.Parse(match ? pos[0].Remove(0, 1) : pos[0]) / rw,
                              match1 ? pos[1].Substring(0, 1) : "",
                              double.Parse(match1 ? pos[1].Remove(0, 1) : pos[1]) / rh);
            }).ToArray());
        }
    }
}
