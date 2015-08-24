using InstepTechnologies.Models;
using InstepTechnologies.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;

namespace InstepTechnologies.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.pageName = "index";
            return View();
        }

        public ActionResult Aboutus()
        {
            ViewBag.pageName = "aboutus";

            return View();
        }

        public ActionResult Services()
        {
            ViewBag.pageName = "services";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.pageName = "contact";

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
    
        public ActionResult Contact(ContactModel contact)
        {
            if (ModelState.IsValid)
            {
                RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();

                if (String.IsNullOrEmpty(recaptchaHelper.Response))
                {
                    ModelState.AddModelError("", "Captcha answer cannot be empty.");
                    ViewBag.recaptchaError = "Captcha answer cannot be empty.";
                   
                }

                RecaptchaVerificationResult recaptchaResult =  recaptchaHelper.VerifyRecaptchaResponse();

                if (recaptchaResult != RecaptchaVerificationResult.Success)
                {
                    ModelState.AddModelError("", "Incorrect captcha answer.");
                    ViewBag.recaptchaError = "Incorrect captcha answer.";
                }
                else
                {
                    ViewBag.pageName = "contact";
                    string emailBody = System.IO.File.ReadAllText(Server.MapPath("~/utils/ResetPassword.html"));
                    emailBody = emailBody.Replace("[Name]", contact.Name);
                    emailBody = emailBody.Replace("[Email]", contact.Email);
                    emailBody = emailBody.Replace("[Technology]", contact.Topics);
                    emailBody = emailBody.Replace("[Message]", contact.Message);
                    EMailHelper.SendEmail(System.Configuration.ConfigurationManager.AppSettings["officealEmail"], System.Configuration.ConfigurationManager.AppSettings["EmailSubject"], emailBody);
                    ViewBag.emailsent = "Thanks for contacting us. We will contact you as soon.";
                    contact.Email = "";
                    contact.Message = "";
                    contact.Name = "";
                    contact.Topics = "";
                }
            }
            return View();
           
        }

        public ActionResult Projects()
        {
            ViewBag.pageName = "projects";
            return View();
        }

        public ActionResult Blog()
        {
            ViewBag.pageName = "blog";
            return View();
        }

        public ActionResult Training()
        {
            ViewBag.pageName = "Training";
            return View();
        }

        public ActionResult TrainingAngularJS()
        {
            ViewBag.pageName = "Training-AngularJS";
            return View();
        }

        public ActionResult TrainingMicrosoft()
        {
            ViewBag.pageName = "Training-Microsoft Technologies";
            return View();
        }

        public ActionResult TrainingMean()
        {
            ViewBag.pageName = "Training-MEAN Stack";
            return View();
        }
    }
}