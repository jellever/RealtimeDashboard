using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace RealtimeDashboard.Core.General
{
	public class Utils
	{
		private static readonly Random rand = new Random((int)DateTime.Now.Ticks);

		public static byte[] ReadByteArray(Stream stream, int length)
		{
			byte[] buffer = new byte[length];
			int totalRead = 0;
			while (totalRead < length)
			{
				int read = stream.Read(buffer, totalRead, length - totalRead);
				if (read <= 0)
				{
					throw new EndOfStreamException("Can not read from stream! Input stream is closed.");
				}
				totalRead += read;
			}
			return buffer;
		}



		public static List<int> SplitNumberInChunks(int val, int nOfParts)
		{
			List<int> chunks = new List<int>();
			int remaining = val;
			for (int i = nOfParts; i > 1; i--)
			{
				int smallestSize = (int)Math.Floor((double)remaining / i);
				int r = rand.Next(smallestSize / 2, smallestSize);
				chunks.Add(r);
				remaining -= r;
			}
			chunks.Add(remaining);

			int sum = chunks.Sum();
			Debug.Assert(val == sum);
			return chunks;
		}

		public static string RandomString(int size)
		{
			StringBuilder builder = new StringBuilder();
			char ch;
			for (int i = 0; i < size; i++)
			{
				ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 65)));
				builder.Append(ch);
			}
			return builder.ToString();
		}

		public static string RandomString(int minSze, int maxSize)
		{
			int size = rand.Next(minSze, maxSize);
			return RandomString(size);
		}

       
	}
}
