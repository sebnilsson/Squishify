using System;

namespace Squishify.Website.Models {
    public class FileSize {
        public FileSize(long size) {
            this.Value = size;
        }

        public override string ToString() {
            return ReadableFileSize(this.Value);
        }

        public long Value { get; set; }

        private static string ReadableFileSize(double size) {
            string[] units = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            int unit = 0;
            while(size >= 1024) {
                size /= 1024;
                ++unit;
            }

            return String.Format("{0:0.#} {1}", size, units[unit]);
        }
    }
}