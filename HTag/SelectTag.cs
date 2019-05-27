namespace htyWEBlib.Tag
{
    public class SelectTag : HTag
    {
        public HTag AddOption(string text, string value)
        {
            //<option value=\"{DidacticType.Definition}\">Определение</option>
            HTag opt = HTag.Build(TypeTAG.option, text: text);
            opt["value"] = value;
            this.AddContent(opt);
            return opt;
        }
        public SelectTag() : base(TypeTAG.select)
        {
        }
    }
}
