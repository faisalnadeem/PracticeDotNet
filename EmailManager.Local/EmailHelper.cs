using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using EmailManager.Local.Helpers;
using GenerateTestUsers;
using InMemoryFileDemo;
using Yellow.ApplicationServices.Infrastructure.Mail;

namespace EmailManager.Local
{
    public static class EmailHelper
    {
        private const string CONTENT_IMAGES_EMAIL_PATH = "/content/Desktop/images/email/";
        private static string _fcaStatement = @"
            <p class=""small-text"">
                Noddle | One Park Lane | Leeds | LS3 1EP | United Kingdom
            </p>
            <p class=""small-text"">
                Please consider the environment before printing this email.
            </p>
            <p class=""small-text"">
                The information in this email and any attachments must be kept confidential.
                If you are not the intended recipient, please let the sender know and then delete it.
            </p>
            <p class=""small-text"">
                Callcredit Information Group is a trading name of Callcredit Information Group Limited (4968328), Callcredit Limited (3961870), 
                Callcredit Consumer Limited (7891157), Callcredit Marketing Limited (2733070), DecisionMetrics Limited (5202547), process benchmarking ltd (2944342),
                Callcredit Data Solutions Limited (5749125), Latitude Digital Marketing Limited (7050923), Callcredit Lead Generation Limited (5373447) and Callcredit Public Sector Limited (4152031). 
                Noddle is a trading name of Callcredit Consumer Limited (now part of TransUnion &reg;). Registered in England and Wales with registered office: One Park Lane, Leeds, West Yorkshire, LS3 1EP.
            </p>
            <p class=""small-text"">
                Callcredit Limited and Callcredit Consumer Limited are authorised and regulated by the Financial Conduct Authority.
            </p>";

        public static string Styles()
        {
            return @"
            <style>
                strong { font-weight: bold; }
                p { margin-bottom: 15px; }
                li { margin-bottom: 15px; }
                .small-text { font-size: 12px; }
                .underlined { text-decoration: underline; }
                .title {color: #6a2969;font-weight: bold;}
            </style>";
        }

        public static string FcaStatement()
        {
            return _fcaStatement;
        }

        public static string Footer(EmailModel emailModel)
        {
            var footer = new StringBuilder();

            footer.AppendLine(
                string.Format(
                    "<p><span class=\"strong\">{0}</span><br /><a href=\"{1}\" target=\"_blank\">{2}</a><p>",
                    emailModel.PageName,
                    emailModel.RootUrl,
                    emailModel.HostName));

            if (emailModel.ShowSocialMedia)
                footer.AppendLine(string.Format("<p>{0}</p>", FollowUs()));

            footer.AppendLine(_fcaStatement);

            return footer.ToString();
        }

        public static string FooterNationwide(EmailModel emailModel)
        {
            var footer = new StringBuilder();

            if (emailModel.ShowSocialMedia)
                footer.AppendLine(string.Format("<p>{0}</p>", FollowUs()));

            return footer.ToString();
        }

        private static EmailColorHelper _colorHelper;

        public static EmailColorHelper Color()
        {
            return _colorHelper ?? (_colorHelper = new EmailColorHelper());
        }
       
        public static string HtmlHead(string domainName, string title)
        {
            string spacer = Spacer(domainName, height: "15");
            string head = string.Format(@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
    <title>{0}</title>
</head>
<body style=""margin: 0; padding: 0;"">
    <table width=""100%"" height=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" bgcolor=""#F0F1F1"">
        <tr><td height=""15"" bgcolor=""{1}"">{2}</td></tr>
        <tr><td align=""center"">
                <table width=""600"" cellpadding=""0"" cellspacing=""0"" border=""0"" bgcolor=""#FFFFFF"">",
                title,
                Color().Primary(),
                spacer);

            return head;
        }

        public static string HtmlHeadSevenDayEmail(string domainName, string title)
        {
            string head = string.Format(@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
    <title>{0}</title>
</head>
<body style=""margin: 0; padding: 0; background-color: {1}"" align=""center"">
    <table width=""100%"" height=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" bgcolor=""{1}"">
        <tr>
            <td align=""center"">",
                title, Color().Primary());

            return head;
        }

         public static string HtmlFooterSevenDayEmail()
        {
            return @"
</td></tr></table>
</body>
</html>";
        }

        /// <summary>
        /// This method is not so generic as it may seem, because it specifies table elements for specific group of templates.
        /// Use BeginResponsiveTemplate method for generic responsive templates instead.
        /// </summary>
        public static string HtmlResponsiveHead(string title, string domainName)
        {
            string head = string.Format(@"
<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <meta name = ""format-detection"" content = ""telephone=no;address=no;email=no"" />
    <title>{0}</title>
</head>
<body style=""margin: 0; padding: 0;"">
    <table width=""100%"" height=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" bgcolor=""{1}"">
        <tr><td>
            <table style=""font-size: 11px; font-family: Arial, Helvetica, sans-serif; color: #ffffff; width: 600px;"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"">
                <tbody>
                    {2}
                    <tr><td valign=""middle"" width=""><br /><span style=""color: #ffffff;"">Welcome to your free for life credit report</span><br /><br /></td></tr>
                </tbody>
            </table>
        </td></tr>
        <tr><td align=""center"">
                <table width=""600"" cellpadding=""0"" cellspacing=""0"" border=""0"" bgcolor=""#FFFFFF"">",
                title,
                Color().Primary(),
                Mvs(domainName, Color().Primary()));

            return head;
        }

        public static string HtmlFooter(string domainName, string noReplyEmail, string recipient)
        {
            string spacer = Spacer(domainName, height: "15");
            string footer = string.Format(@"
<tr><td height=""15"" bgcolor=""{0}"" colspan=""3"">{1}</td></tr></table></td></tr>
<tr><td align=""center"">
    <table width=""600"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td>
        <font face=""Arial, Helvetica, sans-serif"" size=""1"" color=""#BBBBBB"">
            <br/>
            Please add Noddle {2} to your address safe list to make sure you get our emails.<br/>
            <br/>
            This email was sent to {3}.<br/>
            The information in this email and any attachments must be kept confidential.
            If you are not the intended recipient, please let the sender know and then delete it.
            <br /><br />
            Callcredit Information Group is a trading name of Callcredit Information Group Limited (4968328), Callcredit Limited (3961870),
            Callcredit Consumer Limited (7891157), Callcredit Marketing Limited (2733070), DecisionMetrics Limited (5202547), process benchmarking ltd (2944342),
            Callcredit Data Solutions Limited (5749125), Latitude Digital Marketing Limited (7050923),
            Callcredit Lead Generation Limited (5373447) and Callcredit Public Sector Limited (4152031). Noddle is a trading name of Callcredit Consumer Limited (now part of TransUnion &reg;).
            Registered in England and Wales with registered office: One Park Lane, Leeds, West Yorkshire, LS3 1EP.
            <br/>
            Callcredit Limited and Callcredit Consumer Limited are authorised and regulated by the Financial Conduct Authority.
            <br/><br/>
            &#169; Callcredit Consumer Limited. All rights reserved.
            <br/><br/>
        </font>
    </td></tr></table>
</td></tr></table>
</body>
</html>",
                Color().Primary(),
                spacer,
                noReplyEmail,
                recipient
                );

            return footer;
        }

        public static string BeginResponsiveTemplate(string title = null, Dictionary<string, string> bodyAttributes = null)
        {
            var titleTag = title != null ? $"<title>{title}</title>" : "";
            var bodyAttributesAsString = BodyAttributesToString(bodyAttributes);

            var content = $@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml""
      xmlns:v=""urn:schemas-microsoft-com:vml""
      xmlns:o=""urn:schemas-microsoft-com:office:office"">
<head>
    <meta name=""viewport"" content=""width=device-width"" />
    {titleTag}
    <!--[if gte mso 9]><xml>
        <o:OfficeDocumentSettings>
            <o:AllowPNG/>
            <o:PixelsPerInch>96</o:PixelsPerInch>
        </o:OfficeDocumentSettings>
    </xml><![endif]-->
</head>
<body{bodyAttributesAsString}>";

            return content;
        }

        public static string EndResponsiveTemplate()
        {
            return "</body></html>";
        }

        public static string Header(string domainName, string text)
        {
            string colorVerticalMargin = Mvs(domainName, Color().Primary());
            string horizontalMargin = Mhm(domainName);
            string mainContent = string.Format("<td><font face=\"Arial, Helvetica, sans-serif\" size=\"4\" color=\"#FFFFFF\"><b>{0}</b></font></td>", text);
            string contentLine = string.Format("<tr bgcolor=\"{0}\">{1}{2}{1}</tr>", Color().Primary(), horizontalMargin, mainContent);
            string header = string.Format("{0}{1}{0}", colorVerticalMargin, contentLine);

            return header;
        }

        public static string HeaderBlack(string domainName, string text, bool alignLeft = false)
        {
            string textAlign = alignLeft == false ? "center" : "left";
            string colorVerticalMargin = Mvs(domainName, "#FFFFFF");
            string horizontalMargin = Mhm(domainName);
            string mainContent = string.Format("<td><font face=\"Arial, Helvetica, sans-serif\" size=\"5\" color=\"#000000\">{0}</b></font></td>", text);
            string contentLine = string.Format("<tr style= \"text-align: {3}; font-weight: bold;\" bgcolor=\"{0}\">{1}{2}{1}</tr>", "#FFFFFF", horizontalMargin, mainContent, textAlign);
            string header = string.Format("{0}{1}{0}", colorVerticalMargin, contentLine);

            return header;
        }

        public static string PStart(string domainName, string color = null)
        {
            if (color == null)
                color = Color().Text();

            string horizontalMargin = Mhm(domainName);
            string paragraph = string.Format("<tr>{0}<td><font face=\"Arial, Helvetica, sans-serif\" size=\"2\" color=\"{1}\">",
                horizontalMargin,
                color);

            return paragraph;
        }

        public static string PEnd(string domainName)
        {
            string horizontalMargin = Mhm(domainName);
            string ending = string.Format("</font></td>{0}</tr>", horizontalMargin);

            return ending;
        }

        public static string UlStart(string domainName)
        {
            string horizontalMargin = Mhm(domainName);
            string paragraph = string.Format("<tr>{0}<td><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">", horizontalMargin);

            return paragraph;
        }

        public static string UlEnd(string domainName)
        {
            string horizontalMargin = Mhm(domainName);
            string ending = string.Format("</table></td>{0}</tr>", horizontalMargin);

            return ending;
        }

        public static string Li(string domainName, string text, string color = null)
        {
            if (color == null)
                color = Color().Text();

            string leftBulletSpace = Spacer(domainName, "10");
            string rightBulletSpace = Spacer(domainName, "10");
            string item =
                string.Format(
                    "<tr><td>{0}</td><td valign=\"top\">&bull;</td><td>{1}</td><td><font face=\"Arial, Helvetica, sans-serif\" size=\"2\" color=\"{2}\">{3}</font></td></tr>",
                    leftBulletSpace,
                    rightBulletSpace,
                    color,
                    text);

            return item;
        }

        public static string Logo(string domainName, string logoName = "share/noddle-logo.png", string logoHeight = "65", string logoWidth = "200")
        {
            string image = Image(domainName, logoName, width: logoWidth, height: logoHeight, alt: "Noddle");
            string link = string.Format("<a href=\"{0}\" title=\"Go to Noddle\">{1}</a>", domainName, image);
            string logo = PStart(domainName, Color().Primary()) + link + PEnd(domainName);

            return logo;
        }

        public static string LogoWithTracking(
            EmailModel emailModel,
            string path,
            string width,
            string height,
            string linkDescriptor,
            string returnUrl)
        {
            string image = Image(emailModel.RootUrl, path, width: width, height: height, alt: "Noddle");
            string link = OpenTrackingLink(emailModel, linkDescriptor, returnUrl) + image +
                          CloseLink();
            string logo = PStart(emailModel.RootUrl, Color().Primary()) + link + PEnd(emailModel.RootUrl);

            return logo;
        }

        public static string Spacer(string domainName, string width = null, string height = null)
        {
            return Image(domainName, "share/spacer.gif", width, height);
        }

        public static string Image(string domainName, string fileName, string width = null, string height = null, string alt = null)
        {
            string path = GetImageUrl(domainName, fileName);
            string widthExpression = "";
            if (width.IsNotNullNorEmpty())
            {
                int result;
                Fail.IfTrue(int.TryParse(width, out result) && result > 600, "Image cannot be wider than 600px, because it will break the layout (max 600px).");

                widthExpression = string.Format("width=\"{0}\"", width);
            }
            string heightExpression = height.IsNotNullNorEmpty() ? string.Format("height=\"{0}\"", height) : "";
            string altExpression = alt.IsNotNullNorEmpty() ? string.Format("alt=\"{0}\"", alt) : "";
            string image = string.Format("<img src=\"{0}\" {1} {2} {3} border=\"0\" style=\"display: block;\" />",
                path,
                widthExpression,
                heightExpression,
                altExpression);

            return image;
        }

        public static string GetImageUrl(string domainName, string fileName)
        {
            string path = string.Format("{0}{1}{2}", domainName, CONTENT_IMAGES_EMAIL_PATH, fileName);
            return path;
        }

        public static string Mhm(string domainName)
        {
            string mhm = string.Format("<td width=\"20\">{0}</td>", Spacer(domainName, width: "25"));

            return mhm;
        }

        public static string Mvs(string domainName, string bgColor = null)
        {
            return EmptyLine(domainName, "5", bgColor);
        }

        public static string Mvm(string domainName, string bgColor = null)
        {
            return EmptyLine(domainName, "20", bgColor);
        }

        public static string EmptyLine(string domainName, string height, string bgColor = null)
        {
            string bgColorExpression = bgColor.IsNotNullNorEmpty() ? string.Format("bgcolor=\"{0}\"", bgColor) : "";
            string mvm = string.Format("<tr {0}><td height=\"{1}\" colspan=\"3\">{2}</td></tr>",
                bgColorExpression,
                height,
                Spacer(domainName, height: height));

            return mvm;
        }

        public static string PreventUrlFromAutoLinking(string url)
        {
            var protocolSeparatorPattern = @":\/\/"; //find ":\\" and add a img before this separator
            var domainPattern = @"(\.[A-Za-z]{2,6})"; //find all ".domain" and add a img before all domains
            if (!Regex.IsMatch(url, protocolSeparatorPattern) || !Regex.IsMatch(url, domainPattern))
                return url;

            // Adds empty image to url which prevent email client from auto-linking it.
            // http://stackoverflow.com/questions/8395004/is-there-a-way-to-disable-email-engines-from-automatically-hyperlinking-a-url
            var emptyImage = "<img src=\"\" width=\"0\" height=\"0\">";
            url = Regex.Replace(url, protocolSeparatorPattern, match => string.Format("{0}{1}", emptyImage, match.Value));
            url = Regex.Replace(url, domainPattern, match => string.Format("{0}{1}", emptyImage, match.Value));

            return url;
        }

        /// <summary>
        /// Should be used only with HtmlResponsiveHead method
        /// </summary>
        public static string ResponsiveEmailStyles()
        {
            string styles =
        @"<style type=""text/css"">
        #outlook a {padding:0;}
        body{width:100% !important; -webkit-text-size-adjust:100%; -ms-text-size-adjust:100%; margin:0; padding:0;} 
        .ExternalClass {width:100%;}
        .ExternalClass, .ExternalClass p, .ExternalClass span, .ExternalClass font, .ExternalClass td, .ExternalClass div {line-height: 100%;}
        #backgroundTable {margin:0; padding:0; width:100% !important; line-height: 100% !important;background:#751b7b;}

        img {outline:none; text-decoration:none; -ms-interpolation-mode: bicubic;} 
        a img {border:none;} 
        .image_fix {display:block;}
        p {margin: 1em 0;}
        h1, h2, h3, h4, h5, h6 {color: black !important;}
        h1 a, h2 a, h3 a, h4 a, h5 a, h6 a {color: blue !important;}
        h1 a:active, h2 a:active,  h3 a:active, h4 a:active, h5 a:active, h6 a:active {
        color: red !important;
        }
        h1 a:visited, h2 a:visited,  h3 a:visited, h4 a:visited, h5 a:visited, h6 a:visited {
        color: purple !important;
        }
        table td {border-collapse: collapse;}
        table { border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt; }
        a {color: #0c92b8;}

        @@media only screen and (max-device-width: 480px) {
            a[href^=""tel""], a[href^=""sms""] {
                        text-decoration: none;
                        color: black !important;
                        pointer-events: none;
                        cursor: default;
                    }

            .mobile_link a[href^=""tel""], .mobile_link a[href^=""sms""] {
                        text-decoration: initial;
                        color: orange !important;
                        pointer-events: auto;
                        cursor: default;
                    }

        }
        @@media only screen and (min-device-width: 768px) and (max-device-width: 1024px) {
            a[href^=""tel""], a[href^=""sms""] {
                        text-decoration: none;
                        color: black !important;
                        pointer-events: none;
                        cursor: default;
                    }

            .mobile_link a[href^=""tel""], .mobile_link a[href^=""sms""] {
                        text-decoration: initial;
                        color: orange !important;
                        pointer-events: auto;
                        cursor: default;
                    }
        }</style>";

            return styles;
        }

        public static string OpenNumberSafeLink(string url)
        {
            //CCNODLE-2081 && CCNODLE-2220
            //Some email clients try in the brutal way find phone numbers in email content.
            //E.g. Sony Xperia email app treats all numbers with two or more digits as phone number.
            //Mentioned app auto links them by adding "<a href>" tag around this number.
            //Unfortunately, this is done very clumsily and it adds this tags to the first match which often are not the real cause.
            //For example if we have in email template pair: 'click here' or paste link: url, then app adds tags into url from 'click here' instead on the link.
            //Additional attribute 'alt' should prevent auto linking in href, therefore it have to be in the first place.
            //Alt attribute have to had the same value as href attribute! In other way, this hack is not working.
            //Sign: ' has to wrap and hide all things which will be created by email app (which usually uses ").

            var link = string.Format("<a alt='{0}' href='{0}' style='text-decoration: none;'>", url);
            return link;
        }

        public static string OpenTrackingLink(EmailModel emailModel, string linkDescriptor, string returnUrl)
        {
            //var settings = ServiceLocator.GetInstance<IGoogleAnalyticsAllowedEmailsAppSettings>();
            //var trackingPath = String.Empty;

            //if (settings.AllowedEmailTypes.Contains(emailModel.GetTemplateName()))
            //    trackingPath = CreateTrackingPathWithUtm(emailModel, linkDescriptor, returnUrl);
            //else
            //    trackingPath = CreateTrackingPath(emailModel, linkDescriptor, returnUrl);

            //var safeLink = OpenNumberSafeLink(trackingPath);

            return "www.google.co.uk";
        }

        public static string CloseLink()
        {
            return "</a>";
        }

        public static string InsertOpeningTrackImage(EmailModel emailModel)
        {
            string image = string.Format(
                "<img src=\"{0}/email-tracking/open?EmailType={1}&EmailVersion={2}&UserEmail={3}\"" +
                " height=\"1\" width=\"1\" style=\"display: none;\" />",
                emailModel.RootUrl,
                emailModel.GetTemplateName(),
                emailModel.EmailVersion,
                HttpUtility.UrlEncode(emailModel.To));

            return image;
        }

        public static string FollowUs()
        {
            string facebookLink = string.Format("<a href=\"{0}\">Facebook</a>", "http://facebook.com/noddleuk");
            string twitterLink = string.Format("<a href=\"{0}\">Twitter</a>", "http://twitter.com/useyournoddle");

            return string.Format("Follow us on {0} and {1}.", facebookLink, twitterLink);
        }

        private static string CreateTrackingPath(EmailModel emailModel, string linkDescriptor, string returnUrl)
        {
            var link = string.Format(
                "{0}/email-tracking/click?EmailType={1}&EmailVersion={2}&LinkDescriptor={3}&ReturnUrl={4}",
                emailModel.RootUrl,
                emailModel.GetTemplateName(),
                emailModel.EmailVersion,
                linkDescriptor,
                HttpUtility.UrlEncode(returnUrl));

            return link;
        }

        private static string CreateTrackingPathWithUtm(EmailModel emailModel, string linkDescriptor, string returnUrl)
        {
            var link = string.Format(
                "{0}/email-tracking/click?EmailType={1}&EmailVersion={2}&LinkDescriptor={3}&utm_medium={1}&utm_campaign={3}&ReturnUrl={4}",
                emailModel.RootUrl,
                emailModel.GetTemplateName(),
                emailModel.EmailVersion,
                linkDescriptor,
                HttpUtility.UrlEncode(returnUrl));

            return link;
        }

        public static string TrustPilot(string domainName)
        {
            string trustPilot = string.Format(@"
            <tr>
                <td height=""60"" align=""center"">
                    <a href=""https://uk.trustpilot.com/review/noddle.co.uk?utm_medium=Trustbox&amp;utm_source=EmailNewsletter1""><img border=""0"" height=""18"" style=""max-height: 18px;"" alt=""Human score"" src=""http://emailsignature.trustpilot.com/newsletter/en-GB/1/4f4500320000640005131045/text1.png"" /></a>
                    &nbsp;&nbsp; <a href=""https://uk.trustpilot.com/review/noddle.co.uk?utm_medium=Trustbox&amp;utm_source=EmailNewsletter1""><img border=""0"" height=""20"" style=""max-height: 20px;"" alt=""Trustpilot Stars"" src=""http://emailsignature.trustpilot.com/newsletter/en-GB/1/4f4500320000640005131045/stars.png"" /></a>
                    &nbsp; &nbsp;
                    <a href=""https://uk.trustpilot.com/review/noddle.co.uk?utm_medium=Trustbox&amp;utm_source=EmailNewsletter1""><img border=""0"" height=""18"" style=""max-height: 18px;"" alt=""number of reviews"" src=""http://emailsignature.trustpilot.com/newsletter/en-GB/1/4f4500320000640005131045/text2.png"" /></a>
                    &nbsp; &nbsp;
                    <a href=""https://uk.trustpilot.com/review/noddle.co.uk?utm_medium=Trustbox&amp;utm_source=EmailNewsletter1""><img border=""0"" width=""100"" height=""16"" alt=""Trustpilot logo"" style=""display: inline-block;"" src=""{0}/content/Desktop/images/email/share/logo-trustpilot.png""/></a>
                </td>
            </tr>", domainName);
            return trustPilot;
        }

        private static string BodyAttributesToString(Dictionary<string, string> bodyAttributes)
        {
            if (bodyAttributes == null || !bodyAttributes.Any())
                return "";

            return bodyAttributes
                .Aggregate("", (allAttributesAsString, attribute) => $@"{allAttributesAsString} {attribute.Key}=""{attribute.Value}""");
        }

        public static string HeaderCreditReport(EmailModel model)
        {
            //if (model.BrandedAccount == BrandedAccount.HomeAndLegacy)
            //{
            //    return $@"<table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""border-collapse: collapse;"">
            //        <tr>
            //            <td style=""text-align: center; padding: 5px;"">
            //                { EmailHelper.OpenTrackingLink(model, "sign_in_first_link", "/account/sign-in") }
            //                <font size=""2"">Log in to website</font>
            //                {EmailHelper.CloseLink()}
            //            </td>
            //        </tr>
            //        <tr>
            //            <td style=""text-align: center;  padding: 10px 10px 1px 10px;"">
            //                <center>
            //                    {EmailHelper.Image(model.RootUrl, "CreditReportNotification/logo-tab.png", null, null, "Noddle logo")}
            //                </center>
            //            </td>
            //        </tr>
            //        <tr>
            //            <td style=""text-align: center; padding: 1px 10px 1px 10px;"">
            //                <center>
            //                    <font color=""#ffffff"" face=""Arial, Helvetica, sans-serif"" size=""2""><strong>in partnership with</strong></font>
            //                </center>
            //            </td>
            //        </tr>
            //        <tr>
            //            <td style=""text-align: center; padding: 10px;"">
            //                <center>
            //                    {EmailHelper.Image(model.RootUrl, "CreditReportNotification/home-and-legacy-logo.png", null, null, "Home & Legacy logo")}
            //                </center>
            //            </td>
            //        </tr>
            //        <tr>
            //            <td style=""text-align: center; padding: 1px;"">
            //                <font color=""#ffffff"" face=""Arial, Helvetica, sans-serif"" size=""4""><strong>You have a new credit report available</strong></font>
            //            </td>
            //        </tr>
            //        <tr>
            //            <td style=""text-align: center; padding: 2px;padding-top: 6px;"">
            //                <center>
            //                    {EmailHelper.OpenTrackingLink(model, "sign_in_second_link", "/account/sign-in")}
            //                        {EmailHelper.Image(model.RootUrl, "CreditReportNotification/button-log-in.png", null, null, "Log in to see your new report")}
            //                    {EmailHelper.CloseLink()}
            //                </center>
            //            </td>
            //        </tr>
            //    </table>";

            //}
            //else
            //{
                return $@"<table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""border-collapse: collapse;"">
                    <tr>
                        <td style=""text-align: center; padding: 5px;"">
                             { EmailHelper.OpenTrackingLink(model, "sign_in_first_link", "/account/sign-in") }
                            Log in to website
                            { EmailHelper.CloseLink() }
                        </td>
                    </tr>
                    <tr>
                        <td style=""text-align: center; padding: 10px;"">
                            <center>
                                {EmailHelper.Image(model.RootUrl, "CreditReportNotification/logo-tab.png", null, null, "Noddle logo")}
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td style=""text-align: center; padding: 10px;"">
                            <font color=""#ffffff"" face=""Arial, Helvetica, sans-serif"" size=""4""><strong>You have a new credit report available</strong></font>
                        </td>
                    </tr>
                    <tr>
                        <td style=""text-align: center; padding: 10px;"">
                            <center>
                                {EmailHelper.OpenTrackingLink(model, "sign_in_second_link", "/account/sign-in")}
                                    {EmailHelper.Image(model.RootUrl, "CreditReportNotification/button-log-in.png", null, null, "Log in to see your new report")}
                                {EmailHelper.CloseLink()}
                            </center>
                        </td>
                    </tr>
                </table>";
            //}
        }

        //public static string HeaderBranding(EmailModel model)
        //{
        //    if (model.BrandedAccount.IsAnyHomeAndLegacyBrandedAccount())
        //    {
        //        return $@"{PStart(model.RootUrl)}
        //                    <font face=""Arial, Helvetica, sans-serif"" size=""2""><strong>in partnership with</strong></font>
        //                {PEnd(model.RootUrl)}
        //                {PStart(model.RootUrl)}
        //                    {EmailHelper.Image(model.RootUrl, "CreditReportNotification/home-and-legacy-logo.png", null, null, "Home & Legacy logo")}
        //                {PEnd(model.RootUrl)}";
        //    }
        //    return String.Empty;
        //}
    }
}