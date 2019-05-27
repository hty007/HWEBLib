using System;

namespace htyWEBlib.Tag
{
    public class BodyTag : HTag
    {
        public HTag Header { get => (HTag)Content[0]; set=> Content[0]= (HTag)value; }
        public HTag Main { get => (HTag)Content[1]; set => Content[1] = (HTag)value; }
        public HTag Footer { get => (HTag)Content[2]; set => Content[2] = (HTag)value; }

        public BodyTag() : base(TypeTAG.body)
        {
            Header = HTag.Build(TypeTAG.header);
            Main = HTag.Build(TypeTAG.main);
            Footer = HTag.Build(TypeTAG.footer);
        }

        
    }



}
