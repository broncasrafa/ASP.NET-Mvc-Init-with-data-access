using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace WebApplication.Util.ValidationAttributes
{
    public class DeleteFileAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Flush();
            string filePath = (filterContext.Result as FilePathResult)?.FileName;

            if (!String.IsNullOrEmpty(filePath))
                File.Delete(filePath);

            if (!String.IsNullOrEmpty(filePath))
            {
                List<string> listPath = filePath.Split('\\').ToList();
                listPath.RemoveAt(listPath.Count - 1);
                string serverPath = string.Join("\\", listPath);
                DeletePastTempFiles(serverPath);
            }
        }

        /// <summary>
        /// Deleta os arquivos antigos (menor que a data de hoje) da pasta Temp
        /// </summary>
        /// <param name="serverPath">caminho do arquivo</param>
        private void DeletePastTempFiles(string serverPath)
        {
            var files = Directory.GetFiles(serverPath);
            foreach(var file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.CreationTime.Date < DateTime.Now.Date)
                    File.Delete(file);
            }
        }
    }
}