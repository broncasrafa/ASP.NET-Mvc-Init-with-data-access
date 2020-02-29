using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace WebApplication.Util
{
    public static class ComboBoxUtil
    {
        /// <summary>
        /// Converte a lista de objectos em SelectList (Combobox)
        /// </summary>
        /// <typeparam name="T">objeto da Entidade</typeparam>
        /// <param name="list">lista de objetos da Entidade</param>
        /// <returns>retorna selectlistitems</returns>
        public static IEnumerable<SelectListItem> ToSelectList<T>(this IList<T> list) where T : class
        {
            var selectList = new List<SelectListItem>();
            selectList.Add(new SelectListItem { Value = "-1", Text = "Selecione..." });

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            foreach (T item in list)
            {
                string Id = null;
                string Descricao = null;

                foreach (PropertyDescriptor prop in properties)
                {
                    if (prop.Name == "Id")
                        Id = prop.GetValue(item).ToString();
                    if (prop.Name == "Descricao")
                        Descricao = prop.GetValue(item).ToString();
                }

                selectList.Add(new SelectListItem
                {
                    Value = Id,
                    Text = Descricao
                });
            }
            return selectList;
        }


        /// <summary>
        /// Converte a lista de valores enums em SelectList (Combobox)
        /// </summary>
        /// <param name="enumValue">valores do enum</param>
        /// <returns>retorna selectlistitems</returns>
        public static List<SelectListItem> ToSelectList(this Enum enumValue)
        {
            var selectList = (from Enum e in Enum.GetValues(enumValue.GetType())
                              select new SelectListItem
                              {
                                  Selected = e.Equals(enumValue),
                                  Text = e.ToDisplayName(),
                                  Value = Convert.ToInt32(e).ToString()
                              }).ToList();
            return selectList;
        }

        private static string ToDisplayName(this Enum value)
        {
            var attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
        private static string ToDescription(this Enum value)
        {
            var attributes = (DisplayAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Name : value.ToString();
        }
    }
}