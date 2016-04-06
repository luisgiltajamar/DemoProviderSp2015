using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoProviderWeb.Models;

namespace DemoProviderWeb.Controllers
{
    public class HomeController : Controller
    {
        [SharePointContextFilter]
        public ActionResult Index()
        {
           

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            var data=new List<TelefonoViewModel>();
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {

                    var telefonosList = clientContext.Web.Lists.GetByTitle("Telefonos");
                    clientContext.Load(telefonosList);
                    clientContext.ExecuteQuery();

                    var query=new CamlQuery();
                    var telefonosItems = telefonosList.GetItems(query);
                    clientContext.Load(telefonosItems);
                    clientContext.ExecuteQuery();

                    foreach (var telefonosItem in telefonosItems)
                    {
                        data.Add(TelefonoViewModel.FromListItem(telefonosItem));
                    }
                 }
            }

            return View(data);
        }

        public ActionResult Add()
        {
            return View(new TelefonoViewModel());
        }

        [HttpPost]
        public ActionResult Add(TelefonoViewModel model)
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
           
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {

                    var telefonosList = clientContext.Web.Lists.GetByTitle("Telefonos");
                    clientContext.Load(telefonosList);
                    clientContext.ExecuteQuery();
                    var listCreationInfo=new ListItemCreationInformation();
                    var item = telefonosList.AddItem(listCreationInfo);
                    item["Title"] = model.Nombre;
                    item["Numero"] = model.Numero;
                    item.Update();
                    clientContext.ExecuteQuery();
                }
            }
            return RedirectToAction("Index",
                new {SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri});
        }

        public ActionResult Delete(int id)
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
           
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {

                    var telefonosList = clientContext.Web.Lists.GetByTitle("Telefonos");
                    var telefonosItem = telefonosList.GetItemById(id);
                    
                    telefonosItem.DeleteObject();
                    clientContext.ExecuteQuery();
                 }
            }
            return RedirectToAction("Index",
               new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }

        public ActionResult Update(int id)
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            TelefonoViewModel model = null;
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {

                    var telefonosList = clientContext.Web.Lists.GetByTitle("Telefonos");
                    clientContext.Load(telefonosList);
                    
                    var telefonosItem = telefonosList.GetItemById(id);
                    clientContext.Load(telefonosItem);
                    clientContext.ExecuteQuery();
                    model = TelefonoViewModel.FromListItem(telefonosItem);

          
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(TelefonoViewModel model)
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);

            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    var telefonosList = clientContext.Web.Lists.GetByTitle("Telefonos");
                    //clientContext.Load(telefonosList);
                    var telefonosItem = telefonosList.GetItemById(model.Id);
                    
                    telefonosItem["Title"] = model.Nombre;
                    telefonosItem["Numero"] = model.Numero;
                    telefonosItem.Update();
                    clientContext.ExecuteQuery();
                }
            }
            return RedirectToAction("Index",
               new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }
      
    }
}
