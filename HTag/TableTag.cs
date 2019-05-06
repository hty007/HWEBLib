using System;
using System.Collections.Generic;
using System.Linq;

namespace htyWEBlib.Tag
{
    public class TableTag : HTag
    {        
        public int CountRow { get => this.Content.Count; }
        //public int CountCol(int i) => data[i].Count;
        public TR_Tag this[int indexer] { get => (TR_Tag)Content[indexer]; set => Content[indexer] = (BuilderTag)value; }


        public TableTag():base(TypeTAG.table)
        {            

        }

        public TR_Tag AddRow(params string[] items)
        {
            Content.Add(new TR_Tag());
            int Index = CountRow - 1;
            var row = (TR_Tag)Content[Index];

            foreach (var item in items)
            {
                row.Add(new TD_Tag(item));
            }
            return row;
        }
        /// <summary>Добавить строку в таблицу</summary>
        public TR_Tag AddTR()
        {
            var tr = new TR_Tag();
            this.AddContent(tr);
            return tr;
            //throw new NotImplementedException();
        }
    }

    public class TR_Tag : HTag
    {        
        public int Count { get => this.Content.Count; }
        public TD_Tag this[int indexer]
        {
            get
            {   if (indexer< Count)
                    return (TD_Tag)Content[indexer];
                throw new IndexOutOfRangeException();
            }
            set
            {
                if (indexer == Count)
                    AddContent((BuilderTag)value);
                else if (indexer < Count)
                    Content[indexer] = value;
                else throw new IndexOutOfRangeException();
            }
        }

        public TR_Tag():base(TypeTAG.tr)
        {
            
        }

        public TD_Tag AddTD(string NameID =null, string width = null)
        {
            var td = new TD_Tag();
            if (NameID != null) td.SetNameID(NameID);
            if (width != null) td.Width = width;
            AddContent(td);
            return td;
        }

        public TD_Tag AddTD_Text(string text)
        {
            var td = new TD_Tag{Text = text};
            AddContent(td);
            return td;
        }
    }

    public class TD_Tag : HTag
    {
        public TD_Tag():base(TypeTAG.td)
        {
        }

        public TD_Tag(string text) : this()
        {
            Text = text;
        }

        public string Width { get=> this["width"]; set=> this["width"] = value; }

        
    }
}
