using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;
using FEP.Model;
using FEP.WebApiModel.RnP;
using FEP.WebApiModel.SLAReminder;


namespace FEP.Intranet.Areas.Commerce.Controllers
{
    public class CartController : FEPController
    {
        private DbEntities db = new DbEntities();

        // GET: Commerce/Cart
        // Landing page - redirects to items
        public ActionResult Index()
        {
            //var view = View();
            //view.MasterName = "~/Views/Shared/_LayoutLandingPage.cshtml";

            return RedirectToAction("Items", "Cart", new { area = "Commerce" });
        }

        // Browse cart items
        // GET: Commerce/Cart/Items
        [HttpGet]
        public async Task<ActionResult> Items()
        {
            var resCart = await WepApiMethod.SendApiAsync<PurchaseOrderFullModel>(HttpVerbs.Get, $"Commerce/Cart/Retrieve?userid={CurrentUser.UserId.Value}");

            if (!resCart.isSuccess)
            {
                return HttpNotFound();
            }

            var mycart = resCart.Data;

            if (mycart == null)
            {
                var order = new PurchaseOrderModel
                {
                    UserId = CurrentUser.UserId.Value,
                    DiscountCode = "",
                    ProformaInvoiceNo = "",
                    PaymentMode = PaymentModes.Online,
                    CreatedDate = DateTime.Now,
                    TotalPrice = 0,
                    Status = CheckoutStatus.Shopping
                };

                var response_cart = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"Commerce/Cart/Create", order);

                if (!response_cart.isSuccess)
                {
                    return HttpNotFound();
                }

                var resCart2 = await WepApiMethod.SendApiAsync<PurchaseOrderFullModel>(HttpVerbs.Get, $"Commerce/Cart/Retrieve?userid={CurrentUser.UserId.Value}");

                if (!resCart2.isSuccess)
                {
                    return HttpNotFound();
                }

                mycart = resCart2.Data;
            }

            return View(mycart);
        }

        // Remove cart item
        // GET: Commerce/Cart/RemoveItem/1
        [HttpGet]
        public async Task<string> RemoveItem(int itemid)
        {
            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Commerce/Cart/RemoveItem?itemid={itemid}");

            if (!response.isSuccess)
            {
                return "failure";  // HttpNotFound();
            }

            if (response.Data == true)
            {
                return "success";  // RedirectToAction("Items", "Cart", new { area = "Commerce" });
            }
            else
            {
                return "failure";
            }
        }

        // Empty cart
        // GET: Commerce/Cart/Empty/1
        [HttpGet]
        public async Task<ActionResult> Empty(int cartid)
        {
            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Commerce/Cart/Empty?cartid={cartid}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            if (response.Data == true)
            {
                return RedirectToAction("Items", "Cart", new { area = "Commerce" });
            }
            else
            {
                return HttpNotFound();
            }

        }

        // Add discount code and go to review
        // POST: Commerce/Cart/SetDiscountCode
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetDiscountCode(EditPurchaseOrderDiscountCodeModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"Commerce/Cart/SetDiscountCode", model);

                if (response.isSuccess)
                {
                    return RedirectToAction("Review", "Cart", new { area = "Commerce" });
                }
            }

            return View(model);
        }

        // Review cart items
        // GET: Commerce/Cart/Review
        [HttpGet]
        public async Task<ActionResult> Review()
        {
            var resCart = await WepApiMethod.SendApiAsync<PurchaseOrderFullModel>(HttpVerbs.Get, $"Commerce/Cart/Retrieve?userid={CurrentUser.UserId.Value}");

            if (!resCart.isSuccess)
            {
                return HttpNotFound();
            }

            var mycart = resCart.Data;

            if (mycart == null)
            {
                return HttpNotFound();
            }

            var resDel = await WepApiMethod.SendApiAsync<PublicationDeliveryModel>(HttpVerbs.Get, $"RnP/Publication/GetDeliveryInfo?userid={CurrentUser.UserId.Value}");

            if (!resDel.isSuccess)
            {
                return HttpNotFound();
            }

            var pdelivery = resDel.Data;

            if (pdelivery == null)
            {
                return HttpNotFound();
            }

            var resPromo = await WepApiMethod.SendApiAsync<PromotionCodeModel>(HttpVerbs.Get, $"Commerce/Cart/GetPromotion?code={mycart.Cart.DiscountCode}");

            if (!resPromo.isSuccess)
            {
                return HttpNotFound();
            }

            var ppromo = resPromo.Data;

            if (ppromo == null)
            {
                ViewBag.PromoExpired = false;
                ViewBag.Discount = 0;
            }
            else
            {
                if ((ppromo.ExpiryDate > DateTime.Now) && (!ppromo.Used))
                {
                    ViewBag.PromoExpired = false;
                    ViewBag.Discount = ppromo.MoneyValue;
                }
                else
                {
                    ViewBag.PromoExpired = true;
                    ViewBag.Discount = 0;
                }
            }

            ViewBag.FirstName = pdelivery.FirstName;
            ViewBag.LastName = pdelivery.LastName;
            ViewBag.Address1 = pdelivery.Address1;
            ViewBag.Address2 = pdelivery.Address2;
            ViewBag.Postcode = pdelivery.Postcode;
            ViewBag.City = pdelivery.City;
            ViewBag.State = pdelivery.State.GetDisplayName();
            ViewBag.PhoneNumber = pdelivery.PhoneNumber;

            return View(mycart);
        }

        // Edit delivery address
        // GET: Commerce/Cart/EditDelivery
        [HttpGet]
        public async Task<ActionResult> EditDelivery()
        {
            var resDel = await WepApiMethod.SendApiAsync<PublicationDeliveryModel>(HttpVerbs.Get, $"RnP/Publication/GetDeliveryInfo?userid={CurrentUser.UserId.Value}");

            if (!resDel.isSuccess)
            {
                return HttpNotFound();
            }

            var pdelivery = resDel.Data;

            if (pdelivery == null)
            {
                return HttpNotFound();
            }

            return View(pdelivery);
        }

        // Process edit delivery address then go back to review cart
        // POST: Commerce/Cart/EditDelivery
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDelivery(PublicationDeliveryModel model)
        {
            if (ModelState.IsValid)
            {
                var response_delivery = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"RnP/Publication/UpdateDeliveryInfo", model);

                if (!response_delivery.isSuccess)
                {
                    return HttpNotFound();
                }

                return RedirectToAction("Review", "Cart", new { area = "Commerce" });
            }

            return View(model);
        }

        // Select payment method
        // GET: Commerce/Cart/PaymentMethod
        [HttpGet]
        public async Task<ActionResult> PaymentMethod()
        {
            var resCart = await WepApiMethod.SendApiAsync<PurchaseOrderFullModel>(HttpVerbs.Get, $"Commerce/Cart/Retrieve?userid={CurrentUser.UserId.Value}");

            if (!resCart.isSuccess)
            {
                return HttpNotFound();
            }

            var mycart = resCart.Data;

            if (mycart == null)
            {
                return HttpNotFound();
            }

            var resDel = await WepApiMethod.SendApiAsync<PublicationDeliveryModel>(HttpVerbs.Get, $"RnP/Publication/GetDeliveryInfo?userid={CurrentUser.UserId.Value}");

            if (!resDel.isSuccess)
            {
                return HttpNotFound();
            }

            var pdelivery = resDel.Data;

            if (pdelivery == null)
            {
                return HttpNotFound();
            }

            var resPromo = await WepApiMethod.SendApiAsync<PromotionCodeModel>(HttpVerbs.Get, $"Commerce/Cart/GetPromotion?code={mycart.Cart.DiscountCode}");

            if (!resPromo.isSuccess)
            {
                return HttpNotFound();
            }

            var ppromo = resPromo.Data;

            if (ppromo == null)
            {
                ViewBag.PromoExpired = false;
                ViewBag.Discount = 0;
            }
            else
            {
                if ((ppromo.ExpiryDate > DateTime.Now) && (!ppromo.Used))
                {
                    ViewBag.PromoExpired = false;
                    ViewBag.Discount = ppromo.MoneyValue;
                }
                else
                {
                    ViewBag.PromoExpired = true;
                    ViewBag.Discount = 0;
                }
            }

            ViewBag.FirstName = pdelivery.FirstName;
            ViewBag.LastName = pdelivery.LastName;
            ViewBag.Address1 = pdelivery.Address1;
            ViewBag.Address2 = pdelivery.Address2;
            ViewBag.Postcode = pdelivery.Postcode;
            ViewBag.City = pdelivery.City;
            ViewBag.State = pdelivery.State.GetDisplayName();
            ViewBag.PhoneNumber = pdelivery.PhoneNumber;

            return View(mycart);
        }

        // TESTING ONLY!!! (for refund testing)

        // Change cart status to paid
        // GET: Commerce/Cart/TestPay
        [HttpGet]
        public async Task<ActionResult> TestPay(int id)
        {
            var resTest = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Commerce/Cart/TestPay?id={id}");

            if (!resTest.isSuccess)
            {
                return HttpNotFound();
            }

            // update purchase count (+1 per purchase or +1 per unit??)

            var resItems = await WepApiMethod.SendApiAsync<List<PurchaseOrderItemModel>>(HttpVerbs.Get, $"Commerce/Cart/GetPublications?id={id}");

            if (resItems.isSuccess)
            {
                var items = resItems.Data;

                foreach (PurchaseOrderItemModel item in items)
                {
                    var resIncrement = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"RnP/Publication/IncrementPurchase?id={id}&incrementvalue={item.Quantity}");
                }
            }

            return RedirectToAction("Items", "Cart", new { area = "Commerce" });
        }

        // Refunds

        // GET: Commerce/Cart/PurchaseHistory
        public ActionResult PurchaseHistory()
        {
            /*
            var resBank = await WepApiMethod.SendApiAsync<List<BankInformationModel>>(HttpVerbs.Get, $"Commerce/Cart/GetBanks");

            if (!resBank.isSuccess)
            {
                return HttpNotFound();
            }

            ViewBag.Banks = resBank.Data;
            */

            return View();
        }

        // GET: Commerce/Cart/PurchaseDetails
        public async Task<ActionResult> PurchaseDetails(int cartid)
        {
            var resBank = await WepApiMethod.SendApiAsync<List<BankInformationModel>>(HttpVerbs.Get, $"Commerce/Cart/GetBanks");

            if (!resBank.isSuccess)
            {
                return HttpNotFound();
            }

            ViewBag.Banks = resBank.Data;

            var resPurchase = await WepApiMethod.SendApiAsync<PurchaseHistoryModel>(HttpVerbs.Get, $"Commerce/Cart/GetPurchaseOrder?cartid={cartid}");

            if (!resPurchase.isSuccess)
            {
                return HttpNotFound();
            }

            var purchase = resPurchase.Data;

            var resItems = await WepApiMethod.SendApiAsync<List<PurchaseDetailsModel>>(HttpVerbs.Get, $"Commerce/Cart/GetPurchaseOrderItems?cartid={cartid}");

            if (!resItems.isSuccess)
            {
                return HttpNotFound();
            }

            var items = resItems.Data;

            //var refund = new CreateRefundModel
            //{
            //    ItemId =
            //};

            var details = new ListPurchaseDetailsModel
            {
                Purchase = purchase,
                Items = items,
                Refund = null
            };

            return View(details);
        }

        // Process add refund request, then refresh list
        // POST: Commerce/Cart/RequestRefund
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> RequestRefund(CreateRefundModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"Commerce/Cart/RequestRefund", model);

                if (!response.isSuccess)
                {
                    return "error";
                }

                if (response.Data == true)
                {
                    return "success";
                }
                else
                {
                    return "error";
                }
            }

            return "error";
        }

        // Process update refund action, then refresh list
        // POST: Commerce/Cart/UpdateRefundStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> UpdateRefundStatus(UpdateRefundStatusModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"Commerce/Cart/UpdateRefundStatus", model);

                if (!response.isSuccess)
                {
                    return "error";
                }

                if (response.Data == true)
                {
                    return "success";
                }
                else
                {
                    return "error";
                }
            }

            return "error";
        }

    }
}
