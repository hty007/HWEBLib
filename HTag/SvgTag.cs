namespace htyWEBlib.Tag
{
    public class SvgTag : BuilderTag
    {
        #region Конструкторы и поля
        public string Height { get => this["height"]; set => this["height"] = value; }
        public string Width { get => this["width"]; set => this["width"] = value; }

        public SvgTag() : base(TypeTAG.svg)
        {
        }
        public SvgTag(string name) : this()
        {
            this["name"] = name;
        }
        public SvgTag(double height, double width, string name = null) : this()
        {
            if (name != null)
                this["name"] = name;
            Height = height.ToString();
            Width = width.ToString();
        }
        #endregion
        #region Функционал
        public SvgContent Rect
        #endregion
    }

    public class SvgContent : BuilderTag
    {
        public SvgContent(TypeTAG tag) : base(tag)
        {
        }
    }
}
