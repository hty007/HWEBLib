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
    }
}
