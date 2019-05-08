using System;

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
        public HTag Add(TypeTAG type, string text, string cssClass = null, string id = null)
        {
            var tag = HTag.Build(type, text:text, css: cssClass);
            if (id != null) tag.ID = id;

            this.AddContent((BuilderTag)tag);
            return tag;

        }
        public HTag Add(TypeTAG type)
        {
            var tag = HTag.Build(type);
            this.AddContent((BuilderTag)tag);
            return tag;
        }
        /// <summary>Добавить на страницу таблицу</summary>
        public TableTag AddTable(string nameID = null)
        {
            TableTag table = new TableTag();
            this.AddContent((BuilderTag)table);
            if (nameID != null) table.SetNameID(nameID);
            return table;
        }
        /// <summary>Установить ID и name тега </summary>
        public void SetNameID (string text)
        {
            ID = Name = text;
        }
        /// <summary>Добавить на форму метку</summary>
        public HTag AddLabel(string text, string nameID = null)
        {
            var label = HTag.Build(TypeTAG.label);
            label.Text = text;
            if (nameID != null) label.SetNameID(nameID);
            return label;
        }
        public InputTag AddTextInput(string nameID = null, string value = null)
        {
            var text = new InputTag(TypeInput.text);
            if (nameID != null) text.SetNameID(nameID);
            if (value != null) text.Value = value;
            this.AddContent(text);
            return text;
        }
        public InputTag AddCheckInput(string nameID = null, string value = null)
        {
            var input = new InputTag(TypeInput.checkbox);
            if (nameID != null) input.SetNameID(nameID);
            if (value != null) input.Value = value;
            this.AddContent(input);
            return input;
        }
        public SelectTag AddSelect(string nameID = null)
        {
            var select = new SelectTag();
            if (nameID != null) select.SetNameID(nameID);
            this.AddContent(select);
            return select;
        }

        public HTag AddScript(string stript)
        {
            var tag = HTag.Build(TypeTAG.script, stript);
            AddContent(tag);
            return tag;           
        }

        public static FormTag BuildForm(string action, string auto = "on", string enctype = null)
        {
            FormTag tag = new FormTag();
            tag.Action = action;
            tag.Autocomplete = auto;
            if (enctype != null) tag.Enctype = enctype;

            return tag;
            //throw new NotImplementedException();
        }

        public HTag AddSubmit(string value = null)
        {
            var sub = new InputTag(TypeInput.submit);
            if (value != null) sub.Value = value;
            this.AddContent(sub);
            return sub;
        }
        public HTag AddBr()
        {
            var br = HTag.Build(TypeTAG.br);
            this.AddContent(br);
            return br;
        }
        public HTag AddH(int index, string text, string css = null)
        {
            TypeTAG tag;
            switch (index)
            {
                case 1: tag = TypeTAG.h1; break;
                case 2: tag = TypeTAG.h2; break;
                case 3: tag = TypeTAG.h3; break;
                case 4: tag = TypeTAG.h4; break;
                case 5: tag = TypeTAG.h5; break;
                case 6: tag = TypeTAG.h6; break;
                default: throw new ArgumentException("Не пойти ли тебе в задницу, с такими аргументами?");
            }
            var tagH = HTag.Build(tag,css:css);
            tagH.Text = text;
            this.AddContent(tagH);
            return tagH;
        }
        public HTag AddDiv(string css = null)
        {
            var div = HTag.Build(TypeTAG.div, css: css); 
            this.AddContent(div);
            return div;
        }
        public HTag AddP(string text = null, string css = null)
        {
            var tag = HTag.Build(TypeTAG.p, text:text, css: css);
            this.AddContent(tag);
            return tag;
        }
        public HTag AddA(string text, string path, string css = null)
        {
            var tag = HTag.Tag_a(path, text);
            if (css != null) tag.CSSClass = css;
            this.AddContent(tag);
            return tag;
        }
        /// <summary>Добовляет список</summary>
        /// <returns>ссылка на список</returns>
        /// <param name="css">Css</param>
        /// <param name="nameID">Name identifier.</param>
        public UlTag AddUl(string css =null, string nameID = null)
        {
            var tag = new UlTag();
            if (css != null) tag.CSSClass = css;
            if (nameID != null) tag.SetNameID(nameID);
            this.AddContent(tag);
            return tag;
        }




        #endregion
        #region Статик функции
        public static HTag Build(TypeTAG type, string text = null, string css = null, string nameID=null )
        {
            HTag tag = new HTag(type);
            if (text != null) tag.AddText(text);
            if (css != null) tag.CSSClass = css;
            if (nameID != null) tag.SetNameID(nameID);
            return tag;
        }
        public static HTag Tag_a(string path, string text, string style=null)
        {
            HTag tagA = Build(TypeTAG.a);
            tagA["href"] = path;
            tagA.Text = text;
            if (style != null) tagA.CSSClass = style;
            return tagA;
            //throw new NotImplementedException();
        }
        #endregion

    }


    public class SelectTag:HTag
    {
        public HTag AddOption(string text, string value)
        {
            //<option value=\"{DidacticType.Definition}\">Определение</option>
            HTag opt = HTag.Build(TypeTAG.option, text: text);
            opt["value"] = value;
            this.AddContent(opt);
            return opt;
        }
        public SelectTag():base(TypeTAG.select)
        {            
        }
    }
}
