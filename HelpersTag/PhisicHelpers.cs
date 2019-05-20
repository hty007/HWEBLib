using htyWEBlib.eduDisciplines;
using htyWEBlib.Tag;
using System;
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
        /// Показать подтему 
        /// </summary>
        /// <param name="science"></param>
        /// <returns></returns>
        public static HTag ShowSciences(this Science science)
        {
            HTag tag = HTag.Build(TypeTAG.div);
            tag.AddContent(science.ShowTheme());
            var ul = tag.AddUl();
            foreach (Science the in science)
                ul.AddLiTag(the.ShowTheme());
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
                td1.AddText(science.GetMaster().GetCode(0));
            td1.AddTextInput(nameID: "ID", value: science.ID.ToString());
            td1.AddHiddenInput(nameID:"master", value: science.GetMaster().GetCode(0));

            var td2 = tr.AddTD();
            td2.AddTextInput(nameID: "name", value: science.Name);

            var td3 = tr.AddTD();
            td3.AddSubmit("Ок");

            return form;
        }

        public static HTag ListScience(this Science sc)
        {
            if (sc.Count == 0)
                return HTag.Build(TypeTAG.p, sc.GetCode(0) + " - " + sc.Name);

            UlTag tag = HTag.BuildUlTag();
            tag.AddP(sc.GetCode(0)+" - "+ sc.Name);
            foreach (Science s in sc)
            {
                tag.AddTegs(s.ListScience());
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


        public static HTag Show(this Science s, string href = null, bool code = true, string style = null, string nameID = null)
        {
            HTag tag = HTag.Build(TypeTAG.div, css : style, nameID:nameID);

            if (code) tag.AddText(s.GetCode(1), "\t");
            if (href != null)                 
                tag.AddA(s.Name, href);
            else tag.AddText(s.Name);
                    
            return tag;
        }

        public static HTag ShowFormula(this Science s)
        {
            HTag tag = HTag.Build(TypeTAG.p, text:s.Name );

            foreach (Science sc in s)
            {
                tag.AddBr();
                tag.AddText(sc.Name);                
            }
            return tag;
        }



    }
}
