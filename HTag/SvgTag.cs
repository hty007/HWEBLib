namespace htyWEBlib.Tag
{
    public class SvgTag : HTag
    {
        public string Height { get => this["height"]; set => this["height"] = value; }
        public string Width { get => this["width"]; set => this["width"] = value; }

        public SvgTag() : base(TypeTAG.svg)
        {
        }
        public SvgTag(string nameID) : this()
        {
            SetNameID(nameID);
        }
    }
}
