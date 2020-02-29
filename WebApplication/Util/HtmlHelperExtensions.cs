using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WebApplication.Util
{
    public static class HtmlHelperExtension
    {
        /// <summary>
        /// Render um elemento HTML bootstrap-multiselect para cada propriedade no objeto representado pela expressão especificada, usando os itens da lista e a label da opção especificados.
        /// </summary>
        /// <typeparam name="TModel">O tipo de modelo</typeparam>
        /// <typeparam name="TProperty">O tipo do valor</typeparam>
        /// <param name="helper">A instância auxiliar do HTML que este método estende</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém as propriedades a serem exibidas</param>
        /// <param name="selectList">A coleção de objetos SelectListItem que é usado para popular o dropdownlist</param>
        /// <param name="optionLabel">O texto padrão para item vazio. Este parâmetro pode ser null</param>
        /// <param name="htmlAttributes">Um objeto que contém os atributos HTML a serem definidos para o elemento</param>
        /// <returns>retorna um elemento select para cada propriedade no objecto que é representado pela expressão</returns>
        public static MvcHtmlString DropDownListMultipleCheckBoxesFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string propertyName = metadata.PropertyName;

            var selectElement = new MultiLevelHtmlTag("select");
            selectElement.Attributes.Add("name", propertyName);
            selectElement.Attributes.Add("class", "listbox");
            selectElement.Attributes.Add("multiple", "multiple");

            foreach(var item in selectList)
            {
                var selectOptionElem = new MultiLevelHtmlTag("option");
                selectOptionElem.Attributes.Add("value", item.Value);
                selectOptionElem.InnerHtml = item.Text;
                selectElement.InnerTags.Add(selectOptionElem);
            }

            if(htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                selectElement.MergeAttributes(attributes);
            }

            return new MvcHtmlString(selectElement.ToString());
        }

        public static MvcHtmlString DropDownListMultipleCheckBoxFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string propertyName = metadata.PropertyName;

            var div = new MultiLevelHtmlTag("div");
            div.Attributes.Add("class", "multiselect");

            #region Div 1
            var div1 = new MultiLevelHtmlTag("div");
            div1.Attributes.Add("class", "selectBox");
            div1.Attributes.Add("onclick", "showCheckboxes()");

            var selectElem = new MultiLevelHtmlTag("select");
            selectElem.Attributes.Add("name", propertyName);
            selectElem.Attributes.Add("id", "your-id-element");
            selectElem.Attributes.Add("class", "dropdown");
            selectElem.Attributes.Add("style", "width: 100%; margin-bottom: 0px;");

            var selectOptionElem = new MultiLevelHtmlTag("option");
            selectOptionElem.Attributes.Add("value", "0");
            selectOptionElem.InnerHtml = "Selecione...";
            selectElem.InnerTags.Add(selectOptionElem);

            var div_1 = new MultiLevelHtmlTag("div");
            div_1.Attributes.Add("class", "overSelect");

            div_1.InnerTags.Add(selectElem);
            div1.InnerTags.Add(div_1);
            #endregion
                        
            #region Div 2
            var div2 = new MultiLevelHtmlTag("div");
            div2.Attributes.Add("id", "checkboxes");
            div2.Attributes.Add("style", "display: none;");

            foreach(var item in selectList)
            {
                var label = new MultiLevelHtmlTag("label");
                label.Attributes.Add("class", "select-checkboxes");

                var input = new MultiLevelHtmlTag("input");
                input.Attributes.Add("type", "checkbox");
                input.Attributes.Add("value", item.Value);

                var span = new MultiLevelHtmlTag("span");
                span.Attributes.Add("style", "padding: 5px;");
                span.InnerHtml = item.Text;

                label.InnerTags.Add(input);
                label.InnerTags.Add(span);

                div2.InnerTags.Add(label);

            }
            #endregion

            if(htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                div.MergeAttributes(attributes);
            }

            return new MvcHtmlString(div.ToString());
        }


        public static MvcHtmlString CustomValidateMessageFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            TagBuilder containerDivBuilder = new TagBuilder("div");
            containerDivBuilder.AddCssClass("field-error-box");

            TagBuilder topDivBuilder = new TagBuilder("div");
            topDivBuilder.AddCssClass("top");

            TagBuilder midDivBuilder = new TagBuilder("div");
            midDivBuilder.AddCssClass("mid");
            midDivBuilder.InnerHtml = helper.ValidationMessageFor(expression).ToString();

            containerDivBuilder.InnerHtml += topDivBuilder.ToString(TagRenderMode.Normal);
            containerDivBuilder.InnerHtml += midDivBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(containerDivBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Renderiza um elemento HTML span de mensagem de alerta de validação, exibindo o label da mensagem especificados.
        /// </summary>
        /// <param name="helper">A instância auxiliar de HTML que este metodo estende</param>
        /// <param name="messageText">O texto que será exibido para alerta</param>
        /// <param name="htmlAttributes">Um objeto que contém os atributos HTML a serem definidos para o elemento</param>
        /// <returns>retorna um elemento HTML span de mensagem de alerta de validação</returns>
        public static MvcHtmlString AlertValidateMessage(this HtmlHelper helper, string messageText, object htmlAttributes = null)
        {
            var spanElem = new MultiLevelHtmlTag("span");
            spanElem.Attributes.Add("class", "text-danger custom-field-validation-error");
            spanElem.InnerHtml = messageText;

            if(htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                spanElem.MergeAttributes(attributes);
            }

            return new MvcHtmlString(spanElem.ToString());
        }
    }

    internal class MultiLevelHtmlTag : TagBuilder
    {
        public MultiLevelHtmlTag(string tagName) : base(tagName) { }

        public List<MultiLevelHtmlTag> InnerTags = new List<MultiLevelHtmlTag>();

        public override string ToString()
        {
            if(InnerTags.Count > 0)
            {
                foreach(MultiLevelHtmlTag tag in InnerTags)
                {
                    this.InnerHtml += tag.ToString();
                }
            }
            return base.ToString();
        }
    }
}