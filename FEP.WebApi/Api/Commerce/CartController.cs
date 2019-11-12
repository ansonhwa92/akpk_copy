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
using System.Web;
using FEP.WebApiModel.SLAReminder;


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
        [HttpGet]
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

            if (cart != null)
            {

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

            return null;
        }

        // Get items list only (no cart info)
        // GET: api/Commerce/Cart/GetItems/1
        [HttpGet]
        [Route("api/Commerce/Cart/GetItems")]
        public List<PurchaseOrderItemModel> GetItems(int cartid)
        {
            var items = db.PurchaseOrderItem.Where(i => i.PurchaseOrderId == cartid).Select(s => new PurchaseOrderItemModel
            {
                Id = s.Id,
                PurchaseOrderId = s.PurchaseOrderId,
                ItemId = s.ItemId,
                Description = s.Description,
                PurchaseType = s.PurchaseType,
                Price = s.Price,
                Quantity = s.Quantity
            }).ToList();

            return items;
        }

        // Get publication items list only (no cart info)
        // GET: api/Commerce/Cart/GetPublications/1
        [HttpGet]
        [Route("api/Commerce/Cart/GetPublications")]
        public List<PurchaseOrderItemModel> GetPublications(int cartid)
        {
            var items = db.PurchaseOrderItem.Where(i => i.PurchaseOrderId == cartid && i.PurchaseType == PurchaseType.Publication).Select(s => new PurchaseOrderItemModel
            {
                Id = s.Id,
                PurchaseOrderId = s.PurchaseOrderId,
                ItemId = s.ItemId,
                Description = s.Description,
                PurchaseType = s.PurchaseType,
                Price = s.Price,
                Quantity = s.Quantity
            }).ToList();

            return items;
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

        // TESTING ONLY!!!!

        // Update cart status to paid
        // POST: api/Commerce/Cart/TestPay
        [Route("api/Commerce/Cart/TestPay")]
        [HttpGet]
        public bool TestPay(int id)
        {
            var cart = db.PurchaseOrder.Where(p => p.Id == id).FirstOrDefault();

            if (cart != null)
            {
                cart.ReceiptNo = "PB0030" + DateTime.Now.ToString("HHmm");
                cart.PaymentDate = DateTime.Now;
                cart.Status = CheckoutStatus.Paid;
                cart.DeliveryStatus = DeliveryStatus.Delivered;
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }

            return false;
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
        /*
        [Route("api/Commerce/Cart/PurchaseHistory")]
        [HttpPost]
        public IHttpActionResult PurchaseHistory(FilterPurchaseHistoryModel request, int userid)
        {

            var query = db.PurchaseOrderItem.Join(db.PurchaseOrder, pi => pi.PurchaseOrderId, po => po.Id, (pi, po) => new { pi.PurchaseOrderId, pi.Id, po.UserId, po.ReceiptNo, pi.PurchaseType, pi.Description, pi.Quantity, pi.Price, po.Status, po.DeliveryStatus, po.PaymentDate }).Where(ph => ph.UserId == userid && ph.Status == CheckoutStatus.Paid);

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
                    RefundStatus = db.Refund.Where(rf => rf.ItemId == s.Id && rf.UserId == s.UserId).FirstOrDefault().RefundStatus
                }).ToList();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }
        */

        // DataTable function for listing and filtering purchase history (purchase orders)
        // POST: api/Commerce/Cart/PurchaseHistory (DataTable)
        [Route("api/Commerce/Cart/PurchaseHistory")]
        [HttpPost]
        public IHttpActionResult PurchaseHistory(FilterPurchaseHistoryModel request, int userid)
        {
            // get po count
            var poquery = db.PurchaseOrder.Where(po => po.UserId == userid && po.Status == CheckoutStatus.Paid);
            var totalCount = poquery.Count();

            // get item count
            var piquery = db.PurchaseOrderItem.Join(db.PurchaseOrder, pi => pi.PurchaseOrderId, po => po.Id, (pi, po) => new { pi.PurchaseOrderId, pi.Id, po.UserId, po.ReceiptNo, pi.PurchaseType, pi.Description, pi.Quantity, pi.Price, po.Status, po.DeliveryStatus, po.PaymentDate }).Where(ph => ph.UserId == userid && ph.Status == CheckoutStatus.Paid);

            //advance search
            piquery = piquery.Where(p => (request.Description == null || p.Description.Contains(request.Description))
               && (request.ReceiptNo == null || p.ReceiptNo.Contains(request.ReceiptNo))
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                piquery = piquery.Where(p => p.Description.Contains(value)
                || p.ReceiptNo.Contains(value)
                );
            }

            // count unique po
            piquery = piquery.OrderBy(o => o.ReceiptNo);
            var pidata = piquery.ToList();
            var lastreceiptno = "";
            List<string> receiptnos = new List<string> { };

            foreach (var mydata in pidata)
            {
                if (mydata.ReceiptNo != lastreceiptno)
                {
                    receiptnos.Add(mydata.ReceiptNo);
                    lastreceiptno = mydata.ReceiptNo;
                }
            }

            var filteredCount = receiptnos.Count;

            if (receiptnos.Count == 0) receiptnos.Add("dummyreceiptno");

            // catenate receiptnos
            //string receiptstring = string.Join(",", receiptnos);

            var query = db.PurchaseOrder.Where(po => receiptnos.Contains(po.ReceiptNo));

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
                        query = query.OrderBy(o => o.PaymentDate).OrderBy(o => o.ReceiptNo);
                        break;
                }

            }
            else
            {
                query = query.OrderBy(o => o.PaymentDate).OrderBy(o => o.ReceiptNo);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new PurchaseHistoryModel
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    ReceiptNo = s.ReceiptNo,
                    PaymentDate = s.PaymentDate,
                    Status = s.Status,
                    DeliveryStatus = s.DeliveryStatus,
                    ItemCount = db.PurchaseOrderItem.Where(pi => pi.PurchaseOrderId == s.Id).Count()
                }).ToList();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }

        // Get purchase order info
        // GET: api/Commerce/Cart/GetPurchaseOrder/1
        [HttpGet]
        [Route("api/Commerce/Cart/GetPurchaseOrder")]
        public PurchaseHistoryModel GetPurchaseOrder(int cartid)
        {
            var cart = db.PurchaseOrder.Where(po => po.Id == cartid).Select(s => new PurchaseHistoryModel
            {
                Id = s.Id,
                UserId = s.UserId,
                ReceiptNo = s.ReceiptNo,
                PaymentDate = s.PaymentDate,
                ItemCount = db.PurchaseOrderItem.Where(pi => pi.PurchaseOrderId == s.Id).Count(),
                Status = s.Status,
                DeliveryStatus = s.DeliveryStatus
            }).FirstOrDefault();

            return cart;
        }

        // Get purchase order items
        // GET: api/Commerce/Cart/GetPurchaseOrderItems/1
        [HttpGet]
        [Route("api/Commerce/Cart/GetPurchaseOrderItems")]
        public List<PurchaseDetailsModel> GetPurchaseOrderItems(int cartid)
        {
            var cart = db.PurchaseOrder.Where(po => po.Id == cartid).Select(s => new PurchaseHistoryModel
            {
                Id = s.Id,
                UserId = s.UserId,
                ReceiptNo = s.ReceiptNo,
                PaymentDate = s.PaymentDate,
                ItemCount = db.PurchaseOrderItem.Where(pi => pi.PurchaseOrderId == s.Id).Count(),
                Status = s.Status,
                DeliveryStatus = s.DeliveryStatus
            }).FirstOrDefault();

            if (cart != null)
            {
                var items = db.PurchaseOrderItem.Where(i => i.PurchaseOrderId == cartid).Select(s => new PurchaseDetailsModel
                {
                    PurchaseOrderId = s.PurchaseOrderId,
                    OrderItemId = s.Id,
                    UserId = cart.UserId,
                    ReceiptNo = cart.ReceiptNo,
                    PurchaseType = s.PurchaseType,
                    Description = s.Description,
                    Quantity = s.Quantity,
                    Amount = s.Quantity * s.Price,
                    RefundStatus = db.Refund.Where(rf => rf.ItemId == s.Id && rf.UserId == cart.UserId).FirstOrDefault().RefundStatus
                }).ToList();

                return items;
            }

            return null;
        }

        // Check if publication was purchased by user
        // GET: api/Commerce/Cart/DigitalPublicationPurchased/1
        [HttpGet]
        [Route("api/Commerce/Cart/DigitalPublicationPurchased")]
        public bool DigitalPublicationPurchased(int id, int userid)
        {
            var carts = db.PurchaseOrder.Where(po => po.UserId == userid && po.Status == CheckoutStatus.Paid).Select(s => new PurchaseHistoryModel
            {
                Id = s.Id,
                UserId = s.UserId,
                ReceiptNo = s.ReceiptNo,
                PaymentDate = s.PaymentDate,
                ItemCount = 0,
                Status = s.Status,
                DeliveryStatus = s.DeliveryStatus
            }).ToList();

            foreach (PurchaseHistoryModel cart in carts)
            {
                var item = db.PurchaseOrderItem.Where(i => i.PurchaseOrderId == cart.Id && i.PurchaseType == PurchaseType.Publication && i.ItemId == id).FirstOrDefault();
                if (item != null)
                {
                    return true;
                }
            }

            return false;
        }

        // Add refund request
        // POST: api/Commerce/Cart/RequestRefund
        [Route("api/Commerce/Cart/RequestRefund")]
        [HttpPost]
        [ValidationActionFilter]
        public bool RequestRefund([FromBody] CreateRefundModel model)
        {
            if (ModelState.IsValid)
            {
                var prefund = new Refund
                {
                    ItemId = model.ItemId,
                    UserId = model.UserId,
                    PurchaseType = model.PurchaseType,
                    FullName = model.FullName,
                    BankID = model.BankID,
                    BankAccountNo = model.BankAccountNo,
                    ReferenceNo = model.ReferenceNo,
                    CreatedDate = DateTime.Now,
                    RefundStatus = RefundStatus.Requested
                };

                db.Refund.Add(prefund);
                db.SaveChanges();

                var emailres = SendEmailNotificationRefundRequest(prefund);

                return true;
            }

            return false;
        }

        // DataTable function for listing and filtering refund request
        // POST: api/Commerce/Cart/ListRefund (DataTable)
        [Route("api/Commerce/Cart/ListRefund")]
        [HttpPost]
        public IHttpActionResult ListRefund(FilterRefundRequestModel request)
        {

            var query = db.Refund.Join(db.PurchaseOrderItem, r => r.ItemId, pi => pi.Id, (r, pi) =>
                new {
                    pi.PurchaseOrderId,
                    pi.Id,
                    r.ReferenceNo,
                    pi.PurchaseType,
                    pi.Description,
                    pi.Quantity,
                    pi.Price,
                    r.ID,
                    r.FullName,
                    r.BankID,
                    r.BankAccountNo,
                    r.ReturnStatus,
                    r.RefundStatus
                }).Where(
                      rp => rp.RefundStatus >= RefundStatus.Requested && rp.PurchaseType == request.ItemType).Join(
                      db.PurchaseOrder, rp => rp.PurchaseOrderId, po => po.Id, (rp, po) =>
                      new {
                          rp.PurchaseOrderId,
                          rp.Id,
                          rp.ReferenceNo,
                          rp.PurchaseType,
                          rp.Description,
                          rp.Quantity,
                          rp.Price,
                          rp.ID,
                          rp.FullName,
                          rp.BankID,
                          rp.BankAccountNo,
                          rp.ReturnStatus,
                          rp.RefundStatus,
                          po.UserId,
                          po.ReceiptNo
                      }).Join(
                            db.User, rpo => rpo.UserId, u => u.Id, (rpo, u) =>
                          new {
                              rpo.PurchaseOrderId,
                              rpo.Id,
                              rpo.ReferenceNo,
                              rpo.PurchaseType,
                              rpo.Description,
                              rpo.Quantity,
                              rpo.Price,
                              rpo.ID,
                              rpo.FullName,
                              rpo.BankID,
                              rpo.BankAccountNo,
                              rpo.ReturnStatus,
                              rpo.RefundStatus,
                              rpo.UserId,
                              rpo.ReceiptNo,
                              u.Name
                          });

            var totalCount = query.Count();

            //advance search
            query = query.Where(p => (request.Description == null || p.Description.Contains(request.Description))
               && (request.ReceiptNo == null || p.ReferenceNo.Contains(request.ReceiptNo))
               && (request.BuyerName == null || p.FullName.Contains(request.BuyerName))
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(p => p.Description.Contains(value)
                || p.ReferenceNo.Contains(value)
                || p.FullName.Contains(value)
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
                            query = query.OrderBy(o => o.ReferenceNo);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.ReferenceNo);
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

                    case "FullName":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.FullName);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.FullName);
                        }

                        break;

                    case "ReturnStatus":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.ReturnStatus);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.ReturnStatus);
                        }

                        break;

                    case "RefundStatus":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.RefundStatus);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.RefundStatus);
                        }

                        break;

                    default:
                        query = query.OrderBy(o => o.RefundStatus).OrderBy(o => o.FullName);
                        break;
                }

            }
            else
            {
                query = query.OrderBy(o => o.RefundStatus).OrderBy(o => o.FullName);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new RefundRequestModel
                {
                    ID = s.ID,
                    PurchaseOrderId = s.PurchaseOrderId,
                    OrderItemId = s.Id,
                    UserId = s.UserId,
                    BuyerName = s.Name,
                    ReceiptNo = s.ReceiptNo,
                    PurchaseType = s.PurchaseType,
                    Description = s.Description,
                    Quantity = s.Quantity,
                    Amount = s.Price * s.Quantity,
                    FullName = s.FullName,
                    BankID = s.BankID,
                    BankAccountNo = s.BankAccountNo,
                    ReturnStatus = s.ReturnStatus,
                    RefundStatus = s.RefundStatus
                }).ToList();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }

        /*
        // Update refund request to approved
        // POST: api/Commerce/Cart/ApproveRefund
        [Route("api/Commerce/Cart/ApproveRefund")]
        [HttpPost]
        [ValidationActionFilter]
        public bool ApproveRefund([FromBody] UpdateRefundStatusModel model)
        {
            if (ModelState.IsValid)
            {
                var prefund = db.Refund.Where(r => r.ID == model.ID).FirstOrDefault();

                if (prefund != null)
                {
                    prefund.RefundStatus = RefundStatus.Complete;
                    prefund.Remarks = model.Remarks;
                    prefund.RefundReferenceNo = model.RefundReferenceNo;
                    db.Entry(prefund).State = EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }

            }

            return false;
        }
        */

        // Update refund request status
        // POST: api/Commerce/Cart/UpdateRefundStatus
        [Route("api/Commerce/Cart/UpdateRefundStatus")]
        [HttpPost]
        [ValidationActionFilter]
        public bool UpdateRefundStatus([FromBody] UpdateRefundStatusModel model)
        {
            if (ModelState.IsValid)
            {
                var prefund = db.Refund.Where(r => r.ID == model.ID).FirstOrDefault();

                if (prefund != null)
                {
                    prefund.RefundStatus = model.Status;
                    prefund.Remarks = model.Remarks;
                    prefund.RefundReferenceNo = model.RefundReferenceNo;
                    db.Entry(prefund).State = EntityState.Modified;
                    db.SaveChanges();

                    var emailres = SendEmailNotificationRefundStatusUpdate(prefund);

                    return true;
                }

            }

            return false;
        }

        // BULK EMAIL

        [NonAction]
        private string GetRefundType(PurchaseType ptype)
        {
            if (ptype == PurchaseType.Event)
            {
                return "eEvent";
            }
            else if (ptype == PurchaseType.Course)
            {
                return "eLearning";
            }
            else if (ptype == PurchaseType.Publication)
            {
                return "Publication";
            }
            else
            {
                return "Unknown";
            }
        }

        [NonAction]
        public bool SendEmailNotificationRefundRequest(Refund refund)
        {
            ParameterListToSend paramToSend = new ParameterListToSend();
            paramToSend.RefundType = GetRefundType(refund.PurchaseType);
            paramToSend.RefundFullName = refund.FullName;
            paramToSend.RefundReferenceNo = refund.ReferenceNo;
            paramToSend.RefundRemarks = "Refund not processed yet.";

            var template = db.NotificationTemplates.Where(t => t.NotificationType == NotificationType.Submit_Publication_Refund).FirstOrDefault();
            string Subject = generateBodyMessage("Publication Refund Request", NotificationType.Submit_Publication_Refund, paramToSend);
            string Body = generateBodyMessage(template.TemplateMessage, NotificationType.Submit_Publication_Refund, paramToSend);

            List<string> Email = new List<string> { };
            List<string> Email1 = new List<string> { };
            List<string> Email2 = new List<string> { };

            Email1 = GetEmailsByAccess(UserAccess.Refunds);
            Email2 = GetEmailsByAccess(UserAccess.RnPPublicationEdit);
            Email = Email1.Concat(Email2).ToList();

            if (Email.Count > 0)
            {
                List<string> uniqueemails = Email.Distinct().ToList();
                Email = uniqueemails;
            }

            var sendresult = SendBulkEmail(NotificationType.Submit_Publication_Refund, NotificationCategory.System, Email, paramToSend, Subject, Body);
            return true;
        }

        [NonAction]
        public bool SendEmailNotificationRefundStatusUpdate(Refund refund)
        {
            ParameterListToSend paramToSend = new ParameterListToSend();
            paramToSend.RefundType = GetRefundType(refund.PurchaseType);
            paramToSend.RefundFullName = refund.FullName;
            paramToSend.RefundReferenceNo = refund.ReferenceNo;
            paramToSend.RefundRemarks = refund.Remarks;

            string Subject;
            string Body;

            if (refund.RefundStatus == RefundStatus.Incomplete)
            {
                var template = db.NotificationTemplates.Where(t => t.NotificationType == NotificationType.Approve_Publication_Refund_Incomplete).FirstOrDefault();
                Subject = generateBodyMessage("Publication Refund Incomplete", NotificationType.Approve_Publication_Refund_Incomplete, paramToSend);
                Body = generateBodyMessage(template.TemplateMessage, NotificationType.Approve_Publication_Refund_Incomplete, paramToSend);
            }
            else
            {
                var template = db.NotificationTemplates.Where(t => t.NotificationType == NotificationType.Approve_Publication_Refund_Complete).FirstOrDefault();
                Subject = generateBodyMessage("Publication Refund Complete", NotificationType.Approve_Publication_Refund_Complete, paramToSend);
                Body = generateBodyMessage(template.TemplateMessage, NotificationType.Approve_Publication_Refund_Complete, paramToSend);
            }

            List<string> Email = new List<string> { };
            List<string> Email1 = new List<string> { };
            List<string> Email2 = new List<string> { };

            Email1 = GetEmailsById(refund.UserId);
            Email2 = GetEmailsByAccess(UserAccess.RnPPublicationEdit);
            Email = Email1.Concat(Email2).ToList();

            if (Email.Count > 0)
            {
                List<string> uniqueemails = Email.Distinct().ToList();
                Email = uniqueemails;
            }

            if (refund.RefundStatus == RefundStatus.Incomplete)
            {
                var sendresult = SendBulkEmail(NotificationType.Approve_Publication_Refund_Incomplete, NotificationCategory.System, Email, paramToSend, Subject, Body);
            }
            else
            {
                var sendresult = SendBulkEmail(NotificationType.Approve_Publication_Refund_Complete, NotificationCategory.System, Email, paramToSend, Subject, Body);
            }
            return true;
        }

        [NonAction]
        public List<string> GetEmailsByAccess(UserAccess UAccess)
        {
            List<string> emails = new List<string> { };

            //var allusers = db.User.Where(u => u.Display).ToList();
            var allusers = db.User.Where(u => u.Display && (u.UserType == UserType.Staff || u.UserType == UserType.SystemAdmin)).ToList();

            foreach (FEP.Model.User myuser in allusers)
            {
                if (myuser.UserAccount.IsEnable)
                {
                    var myroles = myuser.UserAccount.UserRoles;
                    foreach (UserRole myrole in myroles)
                    {
                        var myroleid = myrole.RoleId;
                        var myaccesses = db.RoleAccess.Where(ra => ra.RoleId == myroleid).ToList();
                        foreach (RoleAccess myaccess in myaccesses)
                        {
                            UserAccess myfunction = myaccess.UserAccess;
                            if (myfunction == UAccess)
                            {
                                emails.Add(myuser.Email);
                            }
                        }
                    }
                }
            }

            return emails;
        }

        [NonAction]
        public List<string> GetEmailsById(int userid)
        {
            List<string> emails = new List<string> { };

            var allusers = db.User.Where(u => u.Display && u.Id == userid).ToList();

            foreach (FEP.Model.User myuser in allusers)
            {
                emails.Add(myuser.Email);
            }

            return emails;
        }

        [NonAction]
        public string GetPropertyValues(Object obj, string propertyName)
        {
            Type t = obj.GetType();
            System.Reflection.PropertyInfo[] props = t.GetProperties();
            string value = "";
            foreach (var prop in props)
                if (prop.Name == propertyName)
                {
                    value = (prop.GetValue(obj))?.ToString();
                    break;
                }
                else
                    value = "";

            return value;
        }

        [NonAction]
        public string generateBodyMessage(string TemplateText, NotificationType NotificationType, ParameterListToSend paramToSend)
        {
            var ParamList = db.TemplateParameters.Where(p => p.NotificationType == NotificationType).ToList();
            string WholeText = TemplateText;
            foreach (var item in ParamList)
            {
                string theValue = GetPropertyValues(paramToSend, item.TemplateParameterType);
                string textToReplace = "[#" + item.TemplateParameterType + "]";
                WholeText = WholeText.Replace(textToReplace, theValue);
            }

            return WholeText;
        }

        [NonAction]
        public string generateSubjectMessage(string TemplateText, NotificationType NotificationType, ParameterListToSend paramToSend)
        {
            var ParamList = db.TemplateParameters.Where(p => p.NotificationType == NotificationType).ToList();
            string WholeText = TemplateText;
            foreach (var item in ParamList)
            {
                string theValue = GetPropertyValues(paramToSend, item.TemplateParameterType);
                string textToReplace = "[#" + item.TemplateParameterType + "]";
                WholeText = WholeText.Replace(textToReplace, theValue);
            }

            return WholeText;
        }

        [NonAction]
        public async System.Threading.Tasks.Task<IHttpActionResult> SendBulkEmail(NotificationType NotificationType, NotificationCategory NotificationCategory, List<string> Emails, ParameterListToSend ParameterListToSend, string emailSubject, string emailBody, bool customlink = false)
        {
            bool success = true;
            foreach (string receiverEmailAddress in Emails)
            {
                int counter = 1;
                if (customlink)
                {
                    var template = db.NotificationTemplates.Where(t => t.NotificationType == NotificationType).FirstOrDefault();
                    ParameterListToSend.SurveyLink = ParameterListToSend.SurveyLink.Replace("{email}", receiverEmailAddress);
                    emailBody = generateBodyMessage(template.TemplateMessage, NotificationType, ParameterListToSend);
                }
                var response = await sendEmailUsingAPIAsync(DateTime.Now, (int)NotificationCategory, (int)NotificationType, receiverEmailAddress, emailSubject, emailBody, counter);
                if (response == null)
                {
                    success = false;
                }
            }

            return Ok(success);
        }

        [NonAction]
        public async System.Threading.Tasks.Task<EmailClass> sendEmailUsingAPIAsync(DateTime emailDate, int notifyCategory, int notifyType, string emailAddress, string emailSubject, string emailBody, int counter)
        {
            DateTime myTimeNow = DateTime.Now;
            int epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            EmailClass emailObj = new EmailClass
            {
                datID = emailAddress + "-email" + counter + "-" + epoch.ToString(),
                datType = notifyCategory,
                datNotify = notifyType,
                dtInsert = myTimeNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                dtSchedule = emailDate.AddMinutes(1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                dtExpired = emailDate.AddYears(1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                emailTo = emailAddress,
                subject = HttpUtility.HtmlDecode(emailSubject),
                body = HttpUtility.HtmlDecode(emailBody)
            };
            var response = await FEP.Intranet.WepApiMethod.SendApiAsync<EmailClass>(System.Web.Mvc.HttpVerbs.Post, $"BulkEmail", emailObj, FEP.Intranet.WepApiMethod.APIEngine.EmailSMSAPI);

            if (response.isSuccess)
                return response.Data;
            else
                return null;
        }

    }
}
