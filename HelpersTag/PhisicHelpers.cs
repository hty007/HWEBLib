using htyWEBlib.eduDisciplines;
using htyWEBlib.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static HTag ShowSciences(this Science science)
        {
            HTag tag = HTag.Build(TypeTAG.div);
            var head = HTag.Build(TypeTAG.h3);
            head.AddContent(science.ShowTheme(primer: 1));
            tag.AddContent(head);
            var ul = tag.AddUl();
            foreach (Science the in science)
                ul.AddLiTag(the.ShowTheme(pathName: "/Home/Index?code=" + the.GetCode(), primer: 1));
            return tag;
        }

        public static HTag EditSciences(this Science science, string action)
        {
            string nameID = string.Format("science-{0}", science.GetCode());
            FormTag form = HTag.BuildForm(action);
            form.SetNameID(nameID);
            var tab = form.AddTable(nameID);
            var tr = tab.AddTR();

            var td1 = tr.AddTD();
            if (science.GetMaster() != null)
                td1.AddText(science.GetMaster().GetCode());
            td1.AddTextInput(nameID: "code-" + nameID, value: science.ID.ToString());

            var td2 = tr.AddTD();
            td2.AddTextInput(nameID: "name-" + nameID, value: science.Name);

            var td3 = tr.AddTD();
            td3.AddSubmit("Ок");

            return form;
        }



    }
}
