using System;
using System.Buffers;
using System.Collections.Generic;

using Rubik.Toolkit.Text;

namespace Rubik.Toolkit.Extensions
{
    public static class ReadOnlySequenceExtension
    {
        public static SequencePosition? PositionOf(this ReadOnlySequence<byte> source, ReadOnlySpan<byte> delimiter)
        {
            if (delimiter == null)
                throw new ArgumentNullException(nameof(delimiter));

            if (delimiter.Length == 0)
                throw new ArgumentException($"{nameof(delimiter)} is empty", nameof(delimiter));

            var reader = new SequenceReader<byte>(source);

#if NET6_0_OR_GREATER
            if (reader.TryReadTo(out ReadOnlySpan<byte> value, delimiter, true))
#else
            if (reader.TryReadTo(out ReadOnlySequence<byte> value, delimiter, true))
#endif
            {
                return reader.Sequence.GetPosition(reader.Consumed - delimiter.Length);
            }

            return null;
        }

        public static SequencePosition? LastPositionOf(this ReadOnlySequence<byte> source, ReadOnlySpan<byte> delimiter)
        {
            if (delimiter == null)
                throw new ArgumentNullException(nameof(delimiter));

            if (delimiter.Length == 0)
                throw new ArgumentException($"{nameof(delimiter)} is empty", nameof(delimiter));

            var reader = new SequenceReader<byte>(source);
            var delimiterFound = false;

#if NET6_0_OR_GREATER
            while (reader.TryReadTo(out ReadOnlySpan<byte> value, delimiter, true))
#else
            while (reader.TryReadTo(out ReadOnlySequence<byte> value, delimiter, true))
#endif
            {
                delimiterFound = true;
            }

            if (!delimiterFound)
                return null;

            return reader.Sequence.GetPosition(reader.Consumed - delimiter.Length);
        }

        public static IEnumerable<SequencePosition> AllPositionsOf(this ReadOnlySequence<byte> source, ReadOnlySpan<byte> delimiter)
        {
            if (delimiter == null)
                throw new ArgumentNullException(nameof(delimiter));

            if (delimiter.Length == 0)
                throw new ArgumentException($"{nameof(delimiter)} is empty", nameof(delimiter));

            var reader = new SequenceReader<byte>(source);
            var results = new List<SequencePosition>();

#if NET6_0_OR_GREATER
            while (reader.TryReadTo(out ReadOnlySpan<byte> value, delimiter, true))
#else
            while (reader.TryReadTo(out ReadOnlySequence<byte> value, delimiter, true))
#endif
            {
                results.Add(reader.Sequence.GetPosition(reader.Consumed - delimiter.Length));
            }

            return results;
        }

        public static bool Copy(in this ReadOnlySequence<byte> source, byte[] dst, int offset, int count)
        {
            var reader = new SequenceReader<byte>(source);
            var index = offset;

            while (!reader.End && index - offset < count)
            {
                if (reader.TryRead(out byte b))
                {
                    dst[index] = b;
                    index++;
                }
            }

            return true;
        }

        public static int CopyAll(in this ReadOnlySequence<byte> source, byte[] dst, int offset)
        {
            var reader = new SequenceReader<byte>(source);
            var index = offset;

            while (!reader.End)
            {
                if (reader.TryRead(out byte b))
                {
                    dst[index] = b;
                    index++;
                }
            }

            return index - offset;
        }

        public static string ToHexString(this ReadOnlySequence<byte> source)
        {
            var byteCount = (int)source.Length;

            var charCount = byteCount * 2;
            var splitCount = byteCount - 1;

            var vsb = new ValueStringBuilder(stackalloc char[charCount + splitCount]);
            var reader = new SequenceReader<byte>(source);

            while (true)
            {
                if (!reader.TryRead(out byte b))
                    break;

                vsb.Append(b.ToString("X2"));

                if (reader.End)
                    break;

                vsb.Append(' ');
            }

            var result = vsb.ToString();
            vsb.Dispose();

            return result;
        }
    }
}
