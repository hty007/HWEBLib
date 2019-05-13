using htyWEBlib.eduDisciplines;
using htyWEBlib.Tag;
using System.Collections.Generic;

namespace htyWEBlib.HelpersTag
{
    public static class PhisicHelpers
    {
        public static HtmlTag HtmlHead(string title)
        {
            var html = new HtmlTag();
            html.Head.Title = title;
            html.Head.StyleFile = "~/css/site.css";
            html.Head.ScriptFile = @"https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.5/latest.js?config=TeX-MML-AM_CHTML";
            html.Head.Script.Text = "  ";
            html.Head.Author = "Ренат Мирошников";
            return html;
        }

        /// <summary>
        /// Завернуть Тему в HTag
        /// </summary>
        /// <param name="science">Непосредственно тема</param>
        /// <param name="pathCode">нужна ли ссылка для кода, если нужна путь</param>
        /// <param name="pathName">нужна ли ссылка для названия, если нужна путь</param>
        /// <param name="primer">сколько "шагов" срезать в коде</param>
        /// <returns></returns>
        public static HTag ShowTheme(this Science science, string pathCode = null, string pathName = null, int primer = - 1)
        {
            HTag tag = HTag.Build(TypeTAG.div);
            // Код
            if (science.GetCode(primer) != "")
            { 
                if (pathCode == null)
                    tag.AddText(science.GetCode(primer));
                else
                    tag.AddA(science.GetCode(primer), pathCode);
                tag.AddText(" : ");
            }

            if (pathName == null)
                tag.AddText(science.Name);
            else
                tag.AddA(science.Name, pathName);
            return tag;
        }
        /// <summary>
        /// Показать все подтемы в теме 
        /// </summary>
        /// <param name="science"></param>
        /// <returns></returns>
        public static HTag ShowSciences(this Science science)
        {
            HTag tag = HTag.Build(TypeTAG.div);            
            var ul = tag.AddUl();
            foreach (Science the in science)
                ul.AddLiTag(the.ShowTheme(primer: 1));
            return tag;
        }

        public static HTag EditSciences(this Science science, string action)
        {
            string nameID = string.Format("science");
            FormTag form = HTag.BuildForm(action);
            form.SetNameID(nameID);
            var tab = form.AddTable(nameID);
            var tr = tab.AddTR();

            var td1 = tr.AddTD();
            if (science.GetMaster() != null)
                td1.AddText(science.GetMaster().GetCode());
            td1.AddTextInput(nameID: "ID", value: science.ID.ToString());
            td1.AddHiddenInput(nameID:"master", value: science.GetMaster().GetCode());

            var td2 = tr.AddTD();
            td2.AddTextInput(nameID: "name", value: science.Name);

            var td3 = tr.AddTD();
            td3.AddSubmit("Ок");

            return form;
        }

        public static HTag ListScience(this Science sc)
        {
            if (sc.Count == 0)
                return HTag.Build_Null(sc.ToString());

            UlTag tag = HTag.BuildUlTag();
            tag.AddP(sc.ToString());
            foreach (Science s in sc)
            {
                tag.AddLiTag(s.ListScience());
            }
            return tag;
        }

        public static void ParsForm(this Science sc, Science head, Dictionary<string, string> dic)
        {
            foreach (var e in dic)
            {
                switch (e.Key)
                {
                    case "ID":
                        sc.ID = int.Parse(e.Value);
                        break;
                    case "name":
                        sc.Name = e.Value;
                        break;
                    case "master":
                        var code = e.Value;
                        var master = head.GetScience(code);
                        master.Add(sc);
                        break;
                }
            }/**/
            //throw new NotImplementedException();
        }




    }
}
