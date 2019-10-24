using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using FEP.Model;
using FEP.WebApiModel.RnP;


namespace FEP.Intranet.Areas.RnP.Controllers
{
    public class HomeController : FEPController
    {
        // Landing page - redirects to dashboard or browse publications based on login state
        [AllowAnonymous]
        public ActionResult Index()
        {
            var view = View();
            view.MasterName = "~/Views/Shared/_LayoutLandingPage.cshtml";

            if (CurrentUser.IsAuthenticated())
            {
                return RedirectToAction("Dashboard", "Home", new { area = "" });
            }

            return RedirectToAction("BrowsePublications", "Home", new { area = "RnP" });
        }

        // Browse publications
        // TODO: Handle search/filtering, include star rating
        // GET: RnP/Home/BrowsePublications
        [AllowAnonymous]
        public async Task<ActionResult> BrowsePublications(string keyword, string sorting, bool? articles, bool? books, bool? factssheet, bool? journals, bool? reviews, bool? reports, bool? researchpapers, bool? digital, bool? hardcopy, bool? bahasamalaysia, bool? english)
        {
            if (keyword == null) keyword = "";
            if (sorting == null) sorting = "default";
            if (articles == null) articles = true;
            if (books == null) books = true;
            if (factssheet == null) factssheet = true;
            if (journals == null) journals = true;
            if (reviews == null) reviews = true;
            if (reports == null) reports = true;
            if (researchpapers == null) researchpapers = true;
            if (digital == null) digital = false;
            if (hardcopy == null) hardcopy = false;
            if (bahasamalaysia == null) bahasamalaysia = false;
            if (english == null) english = false;

            // get anonymous surveys
            var resPubs = await WepApiMethod.SendApiAsync<BrowsePublicationModel>(HttpVerbs.Get, $"RnP/Publication/GetPublications?keyword={keyword}&sorting={sorting}&articles={articles}&books={books}&factssheet={factssheet}&journals={journals}&reviews={reviews}&reports={reports}&researchpapers={researchpapers}&digital={digital}&hardcopy={hardcopy}&bahasamalaysia={bahasamalaysia}&english={english}");

            if (!resPubs.isSuccess)
            {
                return HttpNotFound();
            }

            var browser = resPubs.Data;

            if (browser == null)
            {
                return HttpNotFound();
            }

            if (browser.Sorting == "title")
            {
                ViewBag.DefaultSorting = "";
                ViewBag.TitleSorting = "selected";
                ViewBag.YearSorting = "";
                ViewBag.AddedSorting = "";
            }
            else if (browser.Sorting == "year")
            {
                ViewBag.DefaultSorting = "";
                ViewBag.TitleSorting = "";
                ViewBag.YearSorting = "selected";
                ViewBag.AddedSorting = "";
            }
            else if (browser.Sorting == "added")
            {
                ViewBag.DefaultSorting = "";
                ViewBag.TitleSorting = "";
                ViewBag.YearSorting = "";
                ViewBag.AddedSorting = "selected";
            }
            else
            {
                ViewBag.DefaultSorting = "selected";
                ViewBag.TitleSorting = "";
                ViewBag.YearSorting = "";
                ViewBag.AddedSorting = "";
            }

            ViewBag.TypeArticles = "";
            ViewBag.TypeBooks = "";
            ViewBag.TypeFactsSheet = "";
            ViewBag.TypeJournals = "";
            ViewBag.TypeReviews = "";
            ViewBag.TypeReports = "";
            ViewBag.TypeResearchPapers = "";
            ViewBag.FormatDigital = "";
            ViewBag.FormatHardcopy = "";
            ViewBag.LanguageBahasaMalaysia = "";
            ViewBag.LanguageEnglish = "";

            if ((bool)articles) { ViewBag.TypeArticles = "checked"; }
            if ((bool)books) { ViewBag.TypeBooks = "checked"; }
            if ((bool)factssheet) { ViewBag.TypeFactsSheet = "checked"; }
            if ((bool)journals) { ViewBag.TypeJournals = "checked"; }
            if ((bool)reviews) { ViewBag.TypeReviews = "checked"; }
            if ((bool)reports) { ViewBag.TypeReports = "checked"; }
            if ((bool)researchpapers) { ViewBag.TypeResearchPapers = "checked"; }
            if ((bool)digital) { ViewBag.FormatDigital = "checked"; }
            if ((bool)hardcopy) { ViewBag.FormatHardcopy = "checked"; }
            if ((bool)bahasamalaysia) { ViewBag.LanguageBahasaMalaysia = "checked"; }
            if ((bool)english) { ViewBag.LanguageEnglish = "checked"; }

            return View(browser);
        }

        // Publication details
        // TODO: include ratings and reviews info
        // GET: RnP/Home/PublicationDetails
        [AllowAnonymous]
        public async Task<ActionResult> PublicationDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationModel>(HttpVerbs.Get, $"RnP/Publication?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            var publication = resPub.Data;

            if (publication == null)
            {
                return HttpNotFound();
            }

            return View(publication);
        }

        // Select format to purchase
        // GET: RnP/Home/SelectFormat
        //[AllowAnonymous]
        public async Task<ActionResult> SelectFormat(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationModel>(HttpVerbs.Get, $"RnP/Publication?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            var publication = resPub.Data;

            if (publication == null)
            {
                return HttpNotFound();
            }

            return View(publication);
        }

        // Purchase publication
        // GET: RnP/Home/PurchasePublication
        //[AllowAnonymous]
        //public async Task<ActionResult> PurchasePublication(int? id, string formats)
        public async Task<ActionResult> PurchasePublication(string puid, string dbuy, string hbuy, string hbil, string pbuy)
        {
            var id = int.Parse(puid);
            /*
            if (id == null)
            {
                return HttpNotFound();
            }
            */

            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationModel>(HttpVerbs.Get, $"RnP/Publication?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            var publication = resPub.Data;

            if (publication == null)
            {
                return HttpNotFound();
            }

            ViewBag.PubID = publication.ID;
            ViewBag.PubCategory = publication.Category;
            ViewBag.PubTitle = publication.Title;
            ViewBag.PubAuthor = publication.Author;
            ViewBag.PubYear = publication.Year;
            ViewBag.PubLanguage = publication.Language;
            ViewBag.PubISBN = publication.ISBN;
            ViewBag.PubHardcopy = publication.Hardcopy;
            ViewBag.PubDigitalcopy = publication.Digitalcopy;
            ViewBag.PubHDcopy = publication.HDcopy;

            ViewBag.PubDPrice = publication.DPrice;
            ViewBag.PubHPrice = publication.HPrice;
            ViewBag.PubHDPrice = publication.HDPrice;

            ViewBag.DBuy = dbuy;
            ViewBag.HBuy = hbuy;
            ViewBag.HBil = hbil;
            ViewBag.PBuy = pbuy;

            //var pitems = new List<PublicationPurchaseItemModel> { };

            if (dbuy == "true")
            {
                ViewBag.DAmt = publication.DPrice;
            }
            else
            {
                ViewBag.DAmt = 0;
            }
            if (hbuy == "true")
            {
                ViewBag.HAmt = (publication.HPrice * int.Parse(hbil));
            }
            else
            {
                ViewBag.HAmt = 0;
            }
            if (pbuy == "true")
            {
                ViewBag.PAmt = publication.HDPrice;
            }
            else
            {
                ViewBag.PAmt = 0;
            }

            ViewBag.TAmt = ViewBag.DAmt + ViewBag.HAmt + ViewBag.PAmt;

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

            var purchase = new PurchasePublicationModel
            {
                PublicationID = publication.ID,
                FormatDigital = bool.Parse(dbuy),
                FormatHardcopy = bool.Parse(hbuy),
                FormatPromotion = bool.Parse(pbuy),
                HardcopyQuantity = int.Parse(hbil),
                DeliveryID = pdelivery.ID,
                UserId = CurrentUser.UserId.Value,
                FirstName = pdelivery.FirstName,
                LastName = pdelivery.LastName,
                Address1 = pdelivery.Address1,
                Address2 = pdelivery.Address2,
                Postcode = pdelivery.Postcode,
                City = pdelivery.City,
                State = pdelivery.State,
                PhoneNumber = pdelivery.PhoneNumber
            };

            /*
            var purchase = new UpdatePublicationDeliveryModel
            {
                Items = pitems,
                Delivery = pdelivery
            };
            */

            return View(purchase);
        }

        // Process publication purchasing (add to cart too)
        // POST: RnP/Home/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> AddToCart(PurchasePublicationModel model)
        {
            if (ModelState.IsValid)
            {
                // get publication info (use fresh price)
                var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationModel>(HttpVerbs.Get, $"RnP/Publication?id={model.PublicationID}");

                if (!resPub.isSuccess)
                {
                    return "notfound";
                }

                var publication = resPub.Data;

                if (publication == null)
                {
                    return "notfound";
                }

                // create cart
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
                    return "notfound";
                }

                var cartid = response_cart.Data;

                var addsuccess = true;

                // add items to cart
                if (model.FormatDigital)
                {
                    var ditem1 = new PublicationPurchaseItemModel
                    {
                        PurchaseOrderId = cartid,
                        PublicationID = publication.ID,
                        UserId = CurrentUser.UserId.Value,
                        Format = PublicationFormats.Digital,
                        Price = publication.DPrice,
                        Quantity = 1
                    };
                    var response_digital = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"RnP/Publication/AddOrderItem", ditem1);

                    var citem1 = new PurchaseOrderItemModel
                    {
                        PurchaseOrderId = cartid,
                        ItemId = publication.ID,
                        Description = publication.Title + " (Digital)",
                        PurchaseType = PurchaseType.Publication,
                        Price = publication.DPrice,
                        Quantity = 1
                    };
                    var cart_digital = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"Commerce/Cart/AddItem", citem1);
                }

                if (model.FormatHardcopy)
                {
                    var ditem2 = new PublicationPurchaseItemModel
                    {
                        PurchaseOrderId = cartid,
                        PublicationID = publication.ID,
                        UserId = CurrentUser.UserId.Value,
                        Format = PublicationFormats.Hardcopy,
                        Price = publication.HPrice,
                        Quantity = model.HardcopyQuantity
                    };
                    var response_hardcopy = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"RnP/Publication/AddOrderItem", ditem2);

                    var citem2 = new PurchaseOrderItemModel
                    {
                        PurchaseOrderId = cartid,
                        ItemId = publication.ID,
                        Description = publication.Title + " (Hard copy)",
                        PurchaseType = PurchaseType.Publication,
                        Price = publication.HPrice,
                        Quantity = model.HardcopyQuantity
                    };
                    var cart_hardcopy = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"Commerce/Cart/AddItem", citem2);
                }

                if (model.FormatPromotion)
                {
                    var ditem3 = new PublicationPurchaseItemModel
                    {
                        PurchaseOrderId = cartid,
                        PublicationID = publication.ID,
                        UserId = CurrentUser.UserId.Value,
                        Format = PublicationFormats.Promotion,
                        Price = publication.HDPrice,
                        Quantity = 1
                    };
                    var response_promotion = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"RnP/Publication/AddOrderItem", ditem3);

                    var citem3 = new PurchaseOrderItemModel
                    {
                        PurchaseOrderId = cartid,
                        ItemId = publication.ID,
                        Description = publication.Title + " (Promotion 1+1)",
                        PurchaseType = PurchaseType.Publication,
                        Price = publication.HDPrice,
                        Quantity = 1
                    };
                    var cart_promotion = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"Commerce/Cart/AddItem", citem3);
                }

                // delivery address
                var deliverymodel = new PublicationDeliveryModel
                {
                    UserId = CurrentUser.UserId.Value,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    Postcode = model.Postcode,
                    City = model.City,
                    State = model.State,
                    PhoneNumber = model.PhoneNumber
                };

                var response_delivery = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"RnP/Publication/UpdateDeliveryInfo", deliverymodel);

                if (addsuccess)
                {
                    await LogActivity(Model.Modules.RnP, "Purchase Publication", publication);

                    return "success";
                }
                else
                {
                    return "failure";
                }
            }
            return "invalid";
        }

        // Browse surveys (anonymous)
        // TODO: Handle search/filtering
        // GET: RnP/Home/BrowseSurveys
        [AllowAnonymous]
        public async Task<ActionResult> BrowseSurveys(string keyword, string sorting)
        {
            if (keyword == null) keyword = "";
            if (sorting == null) sorting = "default";

            // get anonymous surveys
            var resSurveys = await WepApiMethod.SendApiAsync<BrowseSurveyModel>(HttpVerbs.Get, $"RnP/Survey/GetAnonymousSurveys?keyword={keyword}&sorting={sorting}");

            if (!resSurveys.isSuccess)
            {
                return HttpNotFound();
            }

            var browser = resSurveys.Data;

            if (browser == null)
            {
                return HttpNotFound();
            }

            if (browser.Sorting == "expiry")
            {
                ViewBag.DefaultSorting = "";
                ViewBag.ExpirySorting = "selected";
            }
            else
            {
                ViewBag.DefaultSorting = "selected";
                ViewBag.ExpirySorting = "";
            }

            return View(browser);
        }

        // Browse surveys (logged in)
        // TODO: Handle search/filtering
        // GET: RnP/Home/BrowseSurveysLoggedIn
        public async Task<ActionResult> BrowseSurveysLoggedIn(string keyword, string sorting)
        {
            if (keyword == null) keyword = "";
            if (sorting == null) sorting = "default";

            // get non-anonymous surveys
            var resSurveys = await WepApiMethod.SendApiAsync<BrowseSurveyModel>(HttpVerbs.Get, $"RnP/Survey/GetLoginSurveys?useremail={CurrentUser.Email}&keyword={keyword}&sorting={sorting}");

            if (!resSurveys.isSuccess)
            {
                return HttpNotFound();
            }

            var browser = resSurveys.Data;

            if (browser == null)
            {
                return HttpNotFound();
            }

            if (browser.Sorting == "expiry")
            {
                ViewBag.DefaultSorting = "";
                ViewBag.ExpirySorting = "selected";
            }
            else
            {
                ViewBag.DefaultSorting = "selected";
                ViewBag.ExpirySorting = "";
            }

            return View(browser);
        }

        // Answer public survey
        // GET: RnP/Home/PublicSurvey
        [AllowAnonymous]
        public async Task<ActionResult> PublicSurvey(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey?id={id}");

            if (!resSurvey.isSuccess)
            {
                return HttpNotFound();
            }

            var survey = resSurvey.Data;

            if (survey == null)
            {
                return HttpNotFound();
            }

            var sresp = new UpdateSurveyResponseModel
            {
                SurveyID = survey.ID,
                Type = Model.SurveyResponseTypes.Actual,
                Contents = ""
            };

            var srmodel = new ReturnSurveyResponseModel
            {
                Survey = survey,
                Response = sresp
            };

            if (survey.StartDate > DateTime.Now)
            {
                return RedirectToAction("SurveyNotStarted", "Home", new { area = "RnP", id = survey.ID });
            }
            if (survey.EndDate < DateTime.Now)
            {
                return RedirectToAction("SurveyExpired", "Home", new { area = "RnP", id = survey.ID });
            }

            return View(srmodel);
        }

        // Answer targeted survey
        // GET: RnP/Home/PrivateSurvey
        public async Task<ActionResult> PrivateSurvey(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey?id={id}");

            if (!resSurvey.isSuccess)
            {
                return HttpNotFound();
            }

            var survey = resSurvey.Data;

            if (survey == null)
            {
                return HttpNotFound();
            }

            var sresp = new UpdateSurveyResponseModel
            {
                SurveyID = survey.ID,
                Type = Model.SurveyResponseTypes.Actual,
                Contents = ""
            };

            var srmodel = new ReturnSurveyResponseModel
            {
                Survey = survey,
                Response = sresp
            };

            if (survey.StartDate > DateTime.Now)
            {
                return RedirectToAction("SurveyNotStarted", "Home", new { area = "RnP", id = survey.ID });
            }
            if (survey.EndDate < DateTime.Now)
            {
                return RedirectToAction("SurveyExpired", "Home", new { area = "RnP", id = survey.ID });
            }

            return View(srmodel);
        }

        // Open and answer targeted survey
        // GET: RnP/Home/LinkedSurvey or TakeSurvey
        public async Task<ActionResult> TakeSurvey(string refno, string email)
        {
            if (String.IsNullOrEmpty(refno) || String.IsNullOrEmpty(email))
            {
                return HttpNotFound();
            }

            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey/GetLinkedSurvey?refno={refno}&email={email}");

            if (!resSurvey.isSuccess)
            {
                return HttpNotFound();
            }

            var survey = resSurvey.Data;

            if (survey == null)
            {
                return HttpNotFound();
            }

            var sresp = new UpdateSurveyResponseModel
            {
                SurveyID = survey.ID,
                Type = Model.SurveyResponseTypes.Actual,
                Email = email,
                Contents = ""
            };

            var srmodel = new ReturnSurveyResponseModel
            {
                Survey = survey,
                Response = sresp
            };

            if (survey.StartDate > DateTime.Now)
            {
                return RedirectToAction("SurveyNotStarted", "Home", new { area = "RnP", id = survey.ID });
            }
            if (survey.EndDate < DateTime.Now)
            {
                return RedirectToAction("SurveyExpired", "Home", new { area = "RnP", id = survey.ID });
            }

            if (survey.RequireLogin && (!CurrentUser.IsAuthenticated()))
            {
                return RedirectToAction("LoginAndReturn", "Auth", new { area = "", returnurl = "~/RnP/Home/TakeSurvey?refno=" + refno + "&email=" + email });
            }
            else
            {
                return View(srmodel);
            }
        }

        // Thank you for filling in (public?) survey
        // GET: RnP/Home/ThankYou
        [AllowAnonymous]
        public async Task<ActionResult> ThankYou(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey?id={id}");

            if (!resSurvey.isSuccess)
            {
                return HttpNotFound();
            }

            var survey = resSurvey.Data;

            if (survey == null)
            {
                return HttpNotFound();
            }

            return View(survey);
        }

        // Survey not yet active
        // GET: RnP/Home/SurveyNotStarted
        [AllowAnonymous]
        public async Task<ActionResult> SurveyNotStarted(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey?id={id}");

            if (!resSurvey.isSuccess)
            {
                return HttpNotFound();
            }

            var survey = resSurvey.Data;

            if (survey == null)
            {
                return HttpNotFound();
            }

            return View(survey);
        }

        // Survey expired
        // GET: RnP/Home/SurveyExpired
        [AllowAnonymous]
        public async Task<ActionResult> SurveyExpired(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey?id={id}");

            if (!resSurvey.isSuccess)
            {
                return HttpNotFound();
            }

            var survey = resSurvey.Data;

            if (survey == null)
            {
                return HttpNotFound();
            }

            return View(survey);
        }

        // Process survey answers (actual) submission
        // Redirects to thank you page?
        // POST: Survey/SubmitAnswers/5
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitSurvey(UpdateSurveyResponseModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/SubmitAnswers", model);

                if (response.isSuccess)
                {
                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    await LogActivity(Model.Modules.RnP, "Response submitted for Survey", model);      // titled: " + response.Data, model);

                    TempData["SuccessMessage"] = "Response submitted successfully for Survey";   // titled: " + response.Data + ".";

                    // dashboard

                    return RedirectToAction("ThankYou", "Home", new { area = "RnP", @id = model.SurveyID });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to submit response for survey.";

                    return RedirectToAction("BrowseSurveys", "Home", new { area = "RnP" });
                }
            }

            return View(model);
        }
    }
}