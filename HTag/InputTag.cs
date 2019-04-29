namespace htyWEBlib.Tag
{
    public class InputTag : HTag
    {
        #region поля и свойства
        public TypeInput Type { get => this["type"].ToTypeInput(); set => this["type"] = value.ToString(); }
        /// <summary>атрибут value позволяет установить значение поля ввода</summary>
        public string Value { get => this["value"]; set => this["value"] = value; }
        #endregion
        #region конструкторы
        public InputTag(TypeInput type) : base(TypeTAG.input)
        {
            Type = type;
        }
        #endregion
        #region функционал
        #endregion
        #region Статичные функционал
        #endregion

    }
}
