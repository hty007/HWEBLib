using System;
using System.Text;
using System.Threading.Tasks;

namespace htyWEBlib.Tag
{
    public class HTag : BuilderTag
    {
        public HTag(TypeTAG tag) : base(tag)
        {}
        #region поля и свойства
        //Поля и свойства
        public string ID { get => this["id"]; set => this["id"] = value; }
        public string CSSClass { get => this["class"]; set => this["class"] = value; }
        public string Name { get => this["name"]; set => this["name"] = value; }
        #endregion
        #region Функционал
        public void Add(TypeTAG type, string text, string cssClass = "", string id = "")
        {
            var tag = HTag.Build(type, text, cssClass);
            tag.ID = id;
            this.AddContent((BuilderTag)tag);

        }

        public HTag Add(TypeTAG type)
        {
            var tag = HTag.Build(type);
            this.AddContent((BuilderTag)tag);
            return tag;
        }

        public TableTag AddTable()
        {
            TableTag table = new TableTag();
            this.AddContent((BuilderTag)table);
            return table;
        }
        public void SetNameID (string text)
        {
            ID = Name = text;
        }

        #endregion
        #region Статик функции
        public static HTag Build(TypeTAG p, string text, string cssClass = "")
        {
            var tag =Build(p);
            tag.Text = text;
            if (cssClass != "")
            {
                tag["class"] = cssClass;
            }
            return tag;
        }
        public new static HTag Build(TypeTAG type) => new HTag(type);

        public static HTag Tag_a(string path, string text)
        {
            HTag tagA = Build(TypeTAG.a);
            tagA["href"] = path;
            tagA.Text = text;
            return tagA;
            //throw new NotImplementedException();
        }
        #endregion

    }



}
