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

                double org = OriginalSize;
                double min = MinifiedSize;
                
                return (int)((1 - (min / org)) * 100);
            }
        }
    }
}