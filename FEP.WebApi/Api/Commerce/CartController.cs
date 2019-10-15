using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.RnP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;


namespace FEP.WebApi.Api.Commerce
{
    [Route("api/Commerce/Cart")]
    public class CartController : ApiController
    {
        private DbEntities db = new DbEntities();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Create cart if not exists (checked out/paid carts do not count)
        // Returns cart Id (existing or new)
        // POST: api/Commerce/Cart/Create
        [Route("api/Commerce/Cart/Create")]
        [HttpPost]
        [ValidationActionFilter]
        public int Create([FromBody] CreatePurchaseOrderModel model)
        {
            if (ModelState.IsValid)
            {
                var cart = db.PurchaseOrder.Where(p => p.UserId == model.UserId && p.Status == CheckoutStatus.Shopping).FirstOrDefault();

                if (cart != null)
                {
                    return cart.Id;
                }
                else
                {
                    var newcart = new PurchaseOrder
                    {
                        UserId = model.UserId,
                        DiscountCode = "",
                        ProformaInvoiceNo = "",
                        ReceiptNo = "",
                        PaymentMode = PaymentModes.Online,
                        CreatedDate = DateTime.Now,
                        TotalPrice = 0,
                        Status = CheckoutStatus.Shopping,
                        DeliveryStatus = DeliveryStatus.None
                    };

                    db.PurchaseOrder.Add(newcart);
                    db.SaveChanges();

                    return newcart.Id;
                }
            }

            return 0;
        }

        // Add discount code to cart
        // TODO: check for discount code expiry
        // POST: api/Commerce/Cart/SetDiscountCode
        [Route("api/Commerce/Cart/SetDiscountCode")]
        [HttpPost]
        [ValidationActionFilter]
        public bool SetDiscountCode([FromBody] EditPurchaseOrderDiscountCodeModel model)
        {
            if (ModelState.IsValid)
            {
                var cart = db.PurchaseOrder.Where(p => p.Id == model.Id).FirstOrDefault();

                if (cart != null)
                {
                    cart.DiscountCode = model.DiscountCode;
                    db.Entry(cart).State = EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
            }

            return false;
        }

        // Update payment info of cart
        // POST: api/Commerce/Cart/UpdatePaymentInfo
        [Route("api/Commerce/Cart/UpdatePaymentInfo")]
        [HttpPost]
        [ValidationActionFilter]
        public bool UpdatePaymentInfo([FromBody] EditPurchaseOrderPaymentInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var cart = db.PurchaseOrder.Where(p => p.Id == model.Id).FirstOrDefault();

                if (cart != null)
                {
                    cart.PaymentMode = model.PaymentMode;
                    cart.TotalPrice = model.TotalPrice;
                    db.Entry(cart).State = EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
            }

            return false;
        }

        // Update proforma invoice no
        // POST: api/Commerce/Cart/UpdateInvoiceNo
        [Route("api/Commerce/Cart/UpdateInvoiceNo")]
        [HttpPost]
        [ValidationActionFilter]
        public bool UpdateInvoiceNo([FromBody] EditPurchaseOrderInvoiceNoModel model)
        {
            if (ModelState.IsValid)
            {
                var cart = db.PurchaseOrder.Where(p => p.Id == model.Id).FirstOrDefault();

                if (cart != null)
                {
                    cart.ProformaInvoiceNo = model.ProformaInvoiceNo;
                    db.Entry(cart).State = EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
            }

            return false;
        }

        // Update receipt no
        // POST: api/Commerce/Cart/UpdateReceiptNo
        [Route("api/Commerce/Cart/UpdateReceiptNo")]
        [HttpPost]
        [ValidationActionFilter]
        public bool UpdateReceiptNo([FromBody] EditPurchaseOrderReceiptNoModel model)
        {
            if (ModelState.IsValid)
            {
                var cart = db.PurchaseOrder.Where(p => p.Id == model.Id).FirstOrDefault();

                if (cart != null)
                {
                    cart.ReceiptNo = model.ReceiptNo;
                    db.Entry(cart).State = EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
            }

            return false;
        }

        // Update status of cart
        // POST: api/Commerce/Cart/UpdateStatus
        [Route("api/Commerce/Cart/UpdateStatus")]
        [HttpPost]
        [ValidationActionFilter]
        public bool UpdateStatus([FromBody] EditPurchaseOrderStatusModel model)
        {
            if (ModelState.IsValid)
            {
                var cart = db.PurchaseOrder.Where(p => p.Id == model.Id).FirstOrDefault();

                if (cart != null)
                {
                    cart.Status = model.Status;
                    db.Entry(cart).State = EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
            }

            return false;
        }

        // Empty out cart
        // POST: api/Commerce/Cart/Empty
        [Route("api/Commerce/Cart/Empty")]
        public bool Empty(int cartid)
        {
            var items = db.PurchaseOrderItem.Where(pi => pi.PurchaseOrderId == cartid).ToList();

            if (items != null)
            {
                db.PurchaseOrderItem.RemoveRange(items);
                db.SaveChanges();
            }

            return true;
        }

        // Add order item to cart
        // POST: api/Commerce/Cart/AddItem
        [Route("api/Commerce/Cart/AddItem")]
        [HttpPost]
        [ValidationActionFilter]
        public bool AddItem([FromBody] PurchaseOrderItemModel model)
        {
            if (ModelState.IsValid)
            {
                var pitem = new PurchaseOrderItem
                {
                    PurchaseOrderId = model.PurchaseOrderId,
                    Description = model.Description,
                    PurchaseType = model.PurchaseType,
                    ItemId = model.ItemId,
                    Price = model.Price,
                    Quantity = model.Quantity
                };

                db.PurchaseOrderItem.Add(pitem);
                db.SaveChanges();

                return true;
            }

            return false;
        }

        // Remove order item from cart
        // POST: api/Commerce/Cart/RemoveItem
        [Route("api/Commerce/Cart/RemoveItem")]
        [HttpGet]
        public bool RemoveItem(int itemid)
        {
            var item = db.PurchaseOrderItem.Where(pi => pi.Id == itemid).FirstOrDefault();

            if (item != null)
            {
                db.PurchaseOrderItem.Remove(item);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        // Retrieve full cart info
        // GET: api/Commerce/Cart/Retrieve/1
        [HttpGet]
        [Route("api/Commerce/Cart/Retrieve")]
        public PurchaseOrderFullModel Retrieve(int userid)
        {
            var cart = db.PurchaseOrder.Where(po => po.UserId == userid && po.Status == CheckoutStatus.Shopping).Select(s => new PurchaseOrderModel
            {
                Id = s.Id,
                UserId = s.UserId,
                PaymentMode = s.PaymentMode,
                ProformaInvoiceNo = s.ProformaInvoiceNo,
                ReceiptNo = s.ReceiptNo,
                DiscountCode = s.DiscountCode,
                TotalPrice = s.TotalPrice,
                CreatedDate = s.CreatedDate,
                Status = s.Status,
                DeliveryStatus = s.DeliveryStatus
            }).FirstOrDefault();

            var items = db.PurchaseOrderItem.Where(i => i.PurchaseOrderId == cart.Id).Select(s => new PurchaseOrderItemModel
            {
                Id = s.Id,
                PurchaseOrderId = s.PurchaseOrderId,
                ItemId = s.ItemId,
                Description = s.Description,
                PurchaseType = s.PurchaseType,
                Price = s.Price,
                Quantity = s.Quantity
            }).ToList();

            var publications = db.PurchaseOrderItem.Where(i => i.PurchaseOrderId == cart.Id && i.PurchaseType == PurchaseType.Publication).Select(s => new PurchaseOrderItemModel
            {
                Id = s.Id,
                PurchaseOrderId = s.PurchaseOrderId,
                ItemId = s.ItemId,
                Description = s.Description,
                PurchaseType = s.PurchaseType,
                Price = s.Price,
                Quantity = s.Quantity
            }).ToList();

            var events = db.PurchaseOrderItem.Where(i => i.PurchaseOrderId == cart.Id && i.PurchaseType == PurchaseType.Event).Select(s => new PurchaseOrderItemModel
            {
                Id = s.Id,
                PurchaseOrderId = s.PurchaseOrderId,
                ItemId = s.ItemId,
                Description = s.Description,
                PurchaseType = s.PurchaseType,
                Price = s.Price,
                Quantity = s.Quantity
            }).ToList();

            var courses = db.PurchaseOrderItem.Where(i => i.PurchaseOrderId == cart.Id && i.PurchaseType == PurchaseType.Course).Select(s => new PurchaseOrderItemModel
            {
                Id = s.Id,
                PurchaseOrderId = s.PurchaseOrderId,
                ItemId = s.ItemId,
                Description = s.Description,
                PurchaseType = s.PurchaseType,
                Price = s.Price,
                Quantity = s.Quantity
            }).ToList();

            var mycart = new PurchaseOrderFullModel
            {
                Cart = cart,
                Items = items,
                Publications = publications,
                Events = events,
                Courses = courses
            };

            return mycart;
        }

        // Retrieve discount code value
        // GET: api/Commerce/Cart/GetPromotion/xxx
        [HttpGet]
        [Route("api/Commerce/Cart/GetPromotion")]
        public PromotionCodeModel GetPromotion(string code)
        {
            var promo = db.PromotionCode.Where(p => p.Code == code).Select(s => new PromotionCodeModel
            {
                ID = s.ID,
                Code = s.Code,
                MoneyValue = s.MoneyValue,
                PercentageValue = s.PercentageValue,
                ExpiryDate = s.ExpiryDate,
                Used = s.Used
            }).FirstOrDefault();

            return promo;
        }

        // Refunds

        // Retrieve bank info
        // GET: api/Commerce/Cart/GetBanks
        [HttpGet]
        [Route("api/Commerce/Cart/GetBanks")]
        public List<BankInformationModel> GetBanks()
        {
            var banks = db.BankInformation.OrderBy(b => b.ShortName).Select(s => new BankInformationModel
            {
                ID = s.ID,
                ShortName = s.ShortName,
                FullName = s.FullName
            }).ToList();

            return banks;
        }

        // DataTable function for listing and filtering purchase history
        // POST: api/Commerce/Cart/PurchaseHistory (DataTable)
        [Route("api/Commerce/Cart/PurchaseHistory")]
        [HttpPost]
        public IHttpActionResult PurchaseHistory(FilterPurchaseHistoryModel request)
        {

            var query = db.PurchaseOrderItem.Join(db.PurchaseOrder, pi => pi.PurchaseOrderId, po => po.Id, (pi, po) => new { pi.PurchaseOrderId, pi.Id, po.UserId, po.ReceiptNo, pi.PurchaseType, pi.Description, pi.Quantity, pi.Price, po.Status, po.DeliveryStatus, po.PaymentDate }).Where(ph => ph.Status == CheckoutStatus.Paid);

            var totalCount = query.Count();

            //advance search
            query = query.Where(p => (request.Description == null || p.Description.Contains(request.Description))
               && (request.ReceiptNo == null || p.ReceiptNo.Contains(request.ReceiptNo))
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(p => p.Description.Contains(value)
                || p.ReceiptNo.Contains(value)
                );
            }

            var filteredCount = query.Count();

            //order
            if (request.order != null)
            {
                string sortBy = request.columns[request.order[0].column].data;
                bool sortAscending = request.order[0].dir.ToLower() == "asc";

                switch (sortBy)
                {
                    case "ReceiptNo":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.ReceiptNo);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.ReceiptNo);
                        }

                        break;

                    case "Description":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Description);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Description);
                        }

                        break;

                    case "PaymentDate":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.PaymentDate);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.PaymentDate);
                        }

                        break;

                    case "Status":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Status);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Status);
                        }

                        break;

                    case "DeliveryStatus":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.DeliveryStatus);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.DeliveryStatus);
                        }

                        break;

                    default:
                        query = query.OrderBy(o => o.PaymentDate).OrderBy(o => o.Description);
                        break;
                }

            }
            else
            {
                query = query.OrderBy(o => o.PaymentDate).OrderBy(o => o.Description);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new PurchaseHistoryModel
                {
                    PurchaseOrderId = s.PurchaseOrderId,
                    OrderItemId = s.Id,
                    UserId = s.UserId,
                    ReceiptNo = s.ReceiptNo,
                    PurchaseType = s.PurchaseType,
                    Description = s.Description,
                    Quantity = s.Quantity,
                    Amount = s.Price * s.Quantity,
                    PaymentDate = s.PaymentDate,
                    Status = s.Status,
                    DeliveryStatus = s.DeliveryStatus,
                    RefundStatus = db.Refund.Where(rf => rf.ItemId == s.Id && rf.UserId == s.UserId).FirstOrDefault().Status
                }).ToList();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }

    }
}
