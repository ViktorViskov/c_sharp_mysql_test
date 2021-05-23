// libs


namespace utils
{
    // class container for UI
    class pageData
    {
        // variables
        public string header;
        public string footer;
        public string[] items;


        // constuctor
        public pageData(string header, string footer, string[] items)
        {
            this.header = header;
            this.footer = footer;
            this.items = items;
        }
    }
}