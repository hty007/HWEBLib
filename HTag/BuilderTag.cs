using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htyWEBlib.Tag
{
    public class BuilderTag
    {
        #region Поля и свойства
        private TypeTAG tag;
        private string text;
        private Dictionary<string, string> attributes;
        private List<BuilderTag> content;
        // <summary> Тип тега </summary>
        public TypeTAG Tag { get => tag; set => tag = value; }
        /// <summary> Текст, если присвоент внутрение теги не учитываются </summary>
        public string Text { get => text; set => text = value; }
        /// <summary> Словарь атрибутов </summary>
        public Dictionary<string, string> Attributes { get => attributes; set => attributes = value; }

        internal void Add(TD_Tag tD_Tag)
        {
            throw new NotImplementedException();
        }

        /// <summary> Внутренние теги </summary>
        public List<BuilderTag> Content { get => content; set => content = value; }

        public string this[string key]
        {
            get
            {
                if (Attributes.ContainsKey(key))
                    return Attributes[key];
                return null;
            }
            set
            {
                if (Attributes.ContainsKey(key))
                    Attributes[key] = value;
                else Attributes.Add(key, value);
            }
        }
        #endregion
        #region Конструкторы
        public BuilderTag(TypeTAG tag)
        {
            Tag = tag;
            Attributes = new Dictionary<string, string>();
            Content = new List<BuilderTag>();
        }

        public BuilderTag(TypeTAG teg, Dictionary<string, string> attributes) : this(teg)
        {
            Attributes = attributes;
        }

        public BuilderTag(TypeTAG tag, string text) : this(tag)
        {
            Text = text;
        }

        #endregion
        #region В строку и в Html
        public override string ToString()
        {
            if (Tag == TypeTAG.NULL)
                return Text;

            var atribut = GetAtribute();
            string result;

            if (Text != null && Text != "")
            {
                string br = (Text.Length > 30) ? "\n" : "";
                result = string.Format("<{0}{1}>{3}{2}{3}</{0}>", Tag.ToString(), atribut, Text, br);
                return result;
            }

            if (Content.Count == 0) // Короткий тег   
                result = string.Format("<{0}{1} />", Tag.ToString(), atribut);
            else
            {// Длинный тег
                string content = GetContent();
                //string br = (content.Length > 30) ? "\n" : "";
                result = string.Format("<{0}{1}>{2}{3}</{0}>", Tag.ToString(), atribut, content,"\n");
            }
            return result;
        }

        private string GetContent()
        {
            StringBuilder con = new StringBuilder();
            foreach (BuilderTag co in Content)
            {
                con.Append("\n"+co.ToString());
            }
            return con.ToString();
        }

        private string GetAtribute()
        {
            StringBuilder atribut = new StringBuilder();
            foreach (var atr in Attributes)
            {
                if (atr.Value != null)
                    atribut.Append(string.Format(" {0}=\"{1}\"", atr.Key, atr.Value));
                else
                    atribut.Append(atr.Key);
            }
            return atribut.ToString();
        }
        #endregion
        #region Функционал

        public static BuilderTag operator +(BuilderTag tag1, BuilderTag tag2)
        {
            tag1.AddContent(tag2);
            return tag1;
        }

        

        public void AddContent(BuilderTag tag)
        {
            Content.Add(tag);
        }
        public void AddTegs(params BuilderTag[] tags)
        {

            foreach (BuilderTag t in tags)
                Content.Add(t);
        }

        public void AddText(string text)
        {
            BuilderTag tag = new BuilderTag(TypeTAG.NULL, text);
            Content.Add(tag);
        }
        static public BuilderTag Build(TypeTAG type) => new BuilderTag(type);

        #endregion
    }
}
