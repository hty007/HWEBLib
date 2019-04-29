namespace htyWEBlib.Tag
{
    public class HtmlTag : BuilderTag
    {
        public HeadTag Head { get => (HeadTag)this.Content[0];set => this.Content[0] = (HeadTag)value; }
        public BodyTag Body { get => (BodyTag)this.Content[1]; set => this.Content[1] = (BodyTag)value; }

        public HtmlTag() : base(TypeTAG.html)
        {
            HeadTag head = new HeadTag();
            BodyTag body = new BodyTag();

            this.AddTegs(head, body);
        }

        public override string ToString()
        {
            var res = base.ToString();
            return "<!DOCTYPE html> \n" + res;
        }
    }

}
