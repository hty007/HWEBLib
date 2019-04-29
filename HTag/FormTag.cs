using System;

namespace htyWEBlib.Tag
{
    public class FormTag : BuilderTag
    {
        #region поля и свойства
        /// <summary>
        /// action: устанавливает адрес, на который передаются данные формы
        /// </summary>
        /// <value>The action.</value>
        public string Action { get => this["action"]; set => this["action"] = value; }
        /// <summary>
        /// method: устанавливает метод отправки данных на сервер. Допустимы два значения: post и get.
        /// Значение post позволяет передать данные на веб-сервер через специальные заголовки.
        /// А значение get позволяет передать данные через строку запроса.
        /// </summary>
        /// <value>The metod.</value>
        public string Metod { get => this["metod"]; set => this["metod"] = value; }
        /// <summary>
        /// enctype: устанавливает тип передаваемых данных. Он свою очередь может принимать следующие значения:
        /// application/x-www-form-urlencoded: кодировка отправляемых данных по умолчанию
        /// multipart/form-data: эта кодировка применяется при отправке файлов
        /// text/plain: эта кодировка применяется при отправке простой текстовой информации
        /// </summary>
        /// <value>The enctype.</value>
        public string Enctype { get => this["enctype"]; set => this["enctype"] = value; }
        /// <summary>
        /// Если нам надо включить автодополнение только для каких-то определенных полей, 
        /// то мы можем применить к ним атрибут autocomplete="on":
        /// </summary>
        /// <value>on/off</value>
        public string Autocomplete { get => this["autocomplete"]; set => this["autocomplete"] = value; }

        #endregion
        #region конСТРУКТОРЫ 
        public FormTag(string metod = "post") : base(TypeTAG.form)
        { Metod = metod; }

        public FormTag(string metod = "post", string action = "", string enctype = "", string autocomplete = "") : this(metod)
        {
            if (action != "") Action = action;
            if (enctype != "") Enctype = enctype;
            if (autocomplete != "") Autocomplete = autocomplete;
        }

        #endregion
        #region фУНКЦИОНАЛ
        public HTag AddLabel(string text)
        {
            var label = HTag.Build(TypeTAG.label);
            label.Text = text;
            return label;
        }
        public HTag AddTextInput(string nameID = "", string value = "")
        {
            var text = new InputTag(TypeInput.text);
            if (nameID != "")   text.SetNameID(nameID);
            if (value != "") text.Value = value;
            this.AddContent(text);
            return text;
        }
        public HTag AddSubmit(string nameID = "", string value = "")
        {
            var sub = new InputTag(TypeInput.submit);
            if (value != "") sub.Value = value;
            this.AddContent(sub);
            return sub;

        }

        #endregion
        #region Статичные функционал

        #endregion

        /*
    <form method = "post" action="http://localhost:8080/login.php">
    <input name = "login" />
    < input type="submit" value="Войти" />
    </form>
    */
    }
    public enum TypeInput
    {
        /// <summary>text: обычное текстовое поле</summary>
        text,
        /// <summary>текстовое поле, только вместо вводимых символов отображаются звездочки</summary>
        password,
        /// <summary>radio: радиокнопка или переключатель. Из группы радиокнопок можно выбрать только одну</summary>
        radio,
        /// <summary>checkbox: элемент флажок, который может находиться в отмеченном или неотмеченном состоянии</summary>
        checkbox,
        /// <summary>hidden: скрытое поле</summary>
        hidden,
        /// <summary>submit: кнопка отправки формы</summary>
        submit,
        /// <summary>color: поле для ввода цвета</summary>
        color,
        /// <summary>date: поле для ввода даты</summary>
        date,
        /// <summary>datetime: поле для ввода даты и времени с учетом часового пояса</summary>
        datetime,
        /// <summary>datetime-local: поле для ввода даты и времени без учета часового пояса</summary>
        datetime_local, 
        /// <summary>email: поле для ввода адреса электронной почты</summary>
        email,
        /// <summary>month: поле для ввода года и месяца</summary>
        month,
        /// <summary>number: поле для ввода чисел</summary>
        number,
        /// <summary>range: ползунок для выбора числа из некоторого диапазона</summary>
        range,
        /// <summary>image: создает кнопку в виде картинки</summary>
        image,
        /// <summary>tel: поле для ввода телефона</summary>
        tel,
        /// <summary>time: поле для ввода времени</summary>
        time,
        /// <summary>week: поле для ввода года и недели</summary>
        week,
        /// <summary>url: поле для ввода адреса url</summary>
        url,
        /// <summary>file: поле для выбора отправляемого файла</summary>
        file      
    }

    public static class TypeInputHelper
    { 
        public static TypeInput ToTypeInput (this string text)
        {
            Array inputs = Enum.GetValues(typeof(TypeInput));
            foreach (var inp in inputs) {
                if (inp.ToString() == text)
                    return inp;
            }
            throw new ArgumentException($"Значение {text} не присутвует в TypeInput.");
        }
    }
}
