using System.Collections.Generic;

namespace htyWEBlib.Tag
{
    public class HeadTag : BuilderTag
    {
        #region Поля и свойства
        private List<BuilderTag> ListHead;
        private List<BuilderTag> ListHeadUser;
        /// <summary>Кодировка документа</summary>
        public BuilderTag Charset       { get => ListHead[0]; set => ListHead[0] = value; }
        /// <summary>generator: название программы, которая сгенерировала данный документ</summary>
        public BuilderTag Generator     { get => ListHead[1]; set => ListHead[1] = value; }
        /// <summary>application name: название веб-приложения, частью которого является данный документ</summary>
        public BuilderTag NameApp       { get => ListHead[2]; set => ListHead[2] = value; }
        /// <summary>description: краткое описание документа</summary>
        public BuilderTag DescriptionTag   { get => ListHead[3]; set => ListHead[3] = value; }
        /// <summary>author: автор документа</summary>
        public BuilderTag AuthorTag     { get => ListHead[4]; set => ListHead[4] = value; }
        /// <summary>keywords: ключевые слова документа</summary>
        public BuilderTag KeywordsTag      { get => ListHead[5]; set => ListHead[5] = value; }
        /// <summary>заголовка документа, который отображается на вкладке браузера</summary>
        public BuilderTag TitleTag         { get => ListHead[6]; set => ListHead[6] = value; }
        /// <summary>Стили CSS </summary>
        public BuilderTag CssStyle      { get => ListHead[7]; set => ListHead[7] = value; }
        /// <summary>Java Script </summary>
        public BuilderTag Script { get => ListHead[8]; set => ListHead[8] = value; }
        
        
        /// <summary>Путь к файлу Css стилей </summary>
        public string StyleFile { get => CssStyle["href"]; set => CssStyle["href"] = value; }
        /// <summary>Путь к файлу Java Script </summary>
        public string ScriptFile { get=> Script["src"]; set => Script ["src"] = value; }
        /// <summary>Автор страницы </summary>
        public string Author { get => AuthorTag["content"]; set => AuthorTag["content"] = value; }
        /// <summary>краткое описание документа </summary>
        public string Description { get => DescriptionTag["content"]; set => DescriptionTag["content"] = value; }
        /// <summary>краткое описание документа </summary>
        public string Title { get => TitleTag.Text; set => TitleTag.Text = value; }
        /// <summary>Автор страницы </summary>
        public string Keywords { get => KeywordsTag["content"]; set => KeywordsTag["content"] = value; }

        public int Count { get => ListHeadUser.Count; }
        public BuilderTag this[int index] { get => ListHeadUser[index]; set => ListHeadUser[index] = value; }
        #endregion
        #region Конструкторы


        public HeadTag() : base(TypeTAG.head)
        {
            ListHead = new List<BuilderTag>();

            var charse = HTag.Build(TypeTAG.meta);
            charse["http-equiv"] = "content-type";
            charse["content"] = "text/html; charset=UTF-8";
            ListHead.Add(charse);
            ListHead.Add(Meta("generator", "Сформировано htyWebLib"));//1
            ListHead.Add(Meta("application name", ""));
            ListHead.Add(Meta("description", ""));
            ListHead.Add(Meta("author", ""));
            ListHead.Add(Meta("keywords", ""));//5
            ListHead.Add(HTag.Build(TypeTAG.title));//6

            var cssF = HTag.Build(TypeTAG.link);
            cssF["rel"] = "stylesheet";
            cssF["type"] = "text/css";
            cssF["href"] = "";
            ListHead.Add(cssF);

            var script = HTag.Build(TypeTAG.script);
            script["src"] = "";
            ListHead.Add(script);

            ListHeadUser = new List<BuilderTag>();

        }

        #endregion
        #region Методы

        public void AddHead(BuilderTag tag)
        {
            ListHeadUser.Add(tag);
        }
        public override string ToString()
        {
            this.Content.Clear();
            AddContent(this.Charset);
            AddContent(this.Generator);
            if (this.NameApp["content"]!="")
                AddContent(this.NameApp);
            if (this.AuthorTag["content"] != "")
                AddContent(this.AuthorTag);
            if (this.DescriptionTag["content"] != "")
                AddContent(this.DescriptionTag);
            if (this.KeywordsTag["content"] != "")
                AddContent(this.KeywordsTag);
            if (this.TitleTag.Text != "")
                AddContent(this.TitleTag);
            if (this.CssStyle["href"] != "")
                AddContent(this.CssStyle);
            if (this.Script["src"] != "")
                AddContent(this.Script);
            for (int i = 0; i < ListHeadUser.Count; i++)
                AddContent(ListHeadUser[i]);
            return base.ToString();
        }







        #endregion
        #region Static - методы
        /// <summary>
        /// Meta the specified name and content.
        /// </summary>
        /// <returns>The meta.</returns>
        /// <param name="name">Name.</param>
        /// <param name="content">Content.</param>
        /// /// <remarks>https://metanit.com/web/html5/1.5.php</remarks>
        internal static BuilderTag Meta(string name, string content)
        {
            var meta = htyWEBlib.Tag.HTag.Build(TypeTAG.meta);
            meta["name"] = name;
            meta["content"] = content;
            return meta;
        }


        #endregion

    }



}
