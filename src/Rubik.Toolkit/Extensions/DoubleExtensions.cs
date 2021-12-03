using Rubik.Toolkit.Utils;

namespace Rubik.Toolkit.Extensions
{
    public static class DoubleExtensions
    {
        public static bool IsValid(this double doubleValue, bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
        {
            if (!allowNegative)
            {
                if (doubleValue < 0d)
                {
                    return false;
                }
            }

            if (!allowNaN)
            {
                if (DoubleUtil.IsNaN(doubleValue))
                {
                    return false;
                }
            }

            if (!allowPositiveInfinity)
            {
                if (double.IsPositiveInfinity(doubleValue))
                {
                    return false;
                }
            }

            if (!allowNegativeInfinity)
            {
                if (double.IsNegativeInfinity(doubleValue))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
