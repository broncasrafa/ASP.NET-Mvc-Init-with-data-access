using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Util;
using Domain.Entities;

namespace WebApplication.Controllers
{
    public class BaseController : Controller
    {
        private string _ServerPath;
        public Usuario _UserProperty { get; set; }

        public string ServerPath
        {
            get
            {
                _ServerPath = Server.MapPath(ConfigurationManager.AppSettings["ArquivoTemp"]);
                return _ServerPath;
            }
        }
        public Usuario UsuarioLogado
        {
            get
            {
                _UserProperty = new UserProperty().GetUserLogged();
                return _UserProperty;
            }
        }

        public string MessageSuccess { get { return ErrorMessageType.Success; } }
        public string MessageInfo { get { return ErrorMessageType.Info; } }
        public string MessageWarning { get { return ErrorMessageType.Warning; } }
        public string MessageError { get { return ErrorMessageType.Error; } }

        private string GetErrorsFromModelState()
        {
            string message = string.Empty;

            foreach (ModelState modelState in ModelState.Values)
            {
                foreach (ModelError modelError in modelState.Errors)
                {
                    message = modelError.ErrorMessage;
                    break;
                }

                if (!message.Equals(""))
                    break;
            }

            return message;
        }

        public ActionResult DownloadFile(string filename)
        {
            string fullPath = Path.Combine(ServerPath, filename);
            return File(fullPath, MimeMapping.GetMimeMapping(filename), filename);
        }
        public JsonResult Message(string message, string messageType)
        {
            dynamic messageStr = new { Message = message, Type = messageType };
            return Json(messageStr, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Message(SystemException exception)
        {
            string message = string.Empty;
            string messageType = string.Empty;

            if (exception.Message.Contains(";"))
            {
                message = exception.Message.Split(';')[1];
                messageType = exception.Message.Split(';')[0];
            }
            else
            {
                message = "Ocorreu um erro ao executar a ação.";
                messageType = MessageError;
            }

            return Message(message, messageType);
        }
        public JsonResult ReturnModal(string modalName)
        {
            dynamic messageStr = new { ModalName = modalName };
            return Json(messageStr, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnView(string actionName, string controllerName)
        {
            return Json(new { RedirectUrl = Url.Action(actionName, controllerName) });
        }
        public JsonResult ReturnPartialView(string partialViewName, string divRender, string message, string messageType, object model = null)
        {
            dynamic viewReturn = new
            {
                View = RenderRazorViewToString(ControllerContext, partialViewName, model),
                RenderView = "true",
                DivRender = divRender,
                Message = message,
                MessageType = messageType
            };

            JsonResult jsonResult = Json(viewReturn, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        public JsonResult ReturnPartialView(string partialViewName, string divRender, string modal, object model = null)
        {
            dynamic viewReturn = new
            {
                View = RenderRazorViewToString(ControllerContext, partialViewName, model),
                RenderView = "true",
                DivRender = divRender,
                ModalName = modal
            };

            JsonResult jsonResult = Json(viewReturn, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        public JsonResult ReturnPartialView(string partialViewName, string divRender, object model = null)
        {
            dynamic viewReturn = new
            {
                View = RenderRazorViewToString(ControllerContext, partialViewName, model),
                RenderView = "true",
                DivRender = divRender,
                ShowDiv = "true"
            };

            JsonResult jsonResult = Json(viewReturn, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        public JsonResult ErrorMessageModel()
        {
            string message = GetErrorsFromModelState();

            return Message(message, MessageError);
        }
        public ViewResult ErrorMessageViewModel(string view, object model, string messageType = ErrorMessageType.Warning)
        {
            string message = GetErrorsFromModelState();

            if (!String.IsNullOrEmpty(message))
            {
                ViewBag.TipoMessage = messageType;
                ViewBag.Message = message;
            }

            return View(view, model);
        }

        public static string RenderRazorViewToString(ControllerContext controllerContext, string viewName, object model)
        {
            controllerContext.Controller.ViewData.Model = model;
            using (var writer = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var viewContext = new ViewContext(controllerContext, viewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, writer);
                viewResult.View.Render(viewContext, writer);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                return writer.GetStringBuilder().ToString();
            }
        }        

        public MultiSelectList ConvertToMultiSelectListItems<T>(string dataValueField, string dataTextField, List<T> items, List<T> selectedValues)
        {
            MultiSelectList multiSelectList = new MultiSelectList(items, dataValueField, dataTextField, selectedValues.Select(c => c.GetType().GetProperty(dataValueField).GetValue(c).ToString().ToArray()));
            return multiSelectList;
        }

        public List<SelectListItem> ConvertToListItem<T>(string dataValueField, string dataTextField, List<T> items, bool isSelecione, string initialValue = "", string initialItem = "", string select = "0", List<int> listChecked = null)
        {
            List<SelectListItem> ListSelectItem = new List<SelectListItem>();

            if (!String.IsNullOrEmpty(initialItem))
                ListSelectItem.Add(new SelectListItem { Text = initialItem, Value = initialValue });

            ListSelectItem.AddRange((from item in items
                                     select new SelectListItem
                                     {
                                         Text = item.GetType().GetProperty(dataTextField).GetValue(item).ToString(),
                                         Value = item.GetType().GetProperty(dataValueField).GetValue(item).ToString()
                                     }).ToList());

            if (isSelecione)
                ListSelectItem.Where(c => c.Value == select).First().Selected = true;
            else if (listChecked != null)
                ListSelectItem.Where(c => listChecked.Contains(Convert.ToInt32(c.Value))).ToList().ForEach(x => x.Selected = true);

            return ListSelectItem;
        }
    }

    public static class ErrorMessageType
    {
        public const string Success = "success";
        public const string Info = "info";
        public const string Warning = "warning";
        public const string Error = "error";
    }
}

