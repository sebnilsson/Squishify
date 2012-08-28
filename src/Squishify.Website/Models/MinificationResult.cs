namespace Squishify.Website.Models {
    public class MinificationResult {
        public int OriginalSize { get; set; }
        public int MinifiedSize { get; set; }
        public string MinifiedContent { get; set; }
        public string Minifier { get; set; }
        public int Difference {
            get {
                if(OriginalSize == 0) {
                    return 0;
                }

                return (MinifiedSize * 100) / OriginalSize;
            }
        }
    }
}