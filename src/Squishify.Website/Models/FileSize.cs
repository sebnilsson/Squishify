using System;

namespace Squishify.Website.Models
{
    public class FileSize
    {
        private static readonly string[] Units = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public FileSize(long size)
        {
            this.Value = size;
        }

        public override string ToString()
        {
            return ReadableFileSize(this.Value);
        }

        public long Value { get; set; }

        private static string ReadableFileSize(double size)
        {
            int unitIndex = 0;
            while (size >= 1024)
            {
                size /= 1024;
                ++unitIndex;
            }

            string unit = Units[unitIndex];
            return string.Format("{0:0.#} {1}", size, unit);
        }
    }
}