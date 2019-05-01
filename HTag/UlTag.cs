namespace htyWEBlib.Tag
{
    public class UlTag : HTag
    {
        public UlTag() : base(TypeTAG.ul)
        {
        }

        public HTag AddLiText(string text, string nameID = null, string style = null)
        {
            var tag = (HTag)Build(TypeTAG.li);
            tag.Text = text;
            if (style != null) tag.CSSClass = style;
            if (nameID != null) tag.SetNameID(nameID);
            this.AddContent(tag);
            return tag;
        }
        public HTag AddLi_A(string caption, string href, string nameID = null, string style = null)
        {
            HTag tag = (HTag)Build(TypeTAG.li);
            tag.AddA(caption, href,  css: style);
            if (nameID != null) tag.SetNameID(nameID);
            this.AddContent(tag);
            return tag;
        }

    }
}
