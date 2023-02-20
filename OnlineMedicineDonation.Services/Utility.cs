
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnlineMedicineDonation.Services
{
    public class Utility
    {
        public string GetClientIPAddress(HttpContext context)
        {
            string ip = string.Empty;
            if (!string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-For"]))
            {
                ip = context.Request.Headers["X-Forwarded-For"];
            }
            else
            {
                ip = context.Request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
            }
            return ip;
        }
       
        public bool ChkPdf(string filename)
        {
            try
            {
                byte[] buffer = null;
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(filename).Length;
                //buffer = br.ReadBytes((int)numBytes);
                buffer = br.ReadBytes(5);
                var enc = new System.Text.ASCIIEncoding();
                var header = enc.GetString(buffer);
                bool rtrn = false;
                if (buffer[0] == 0x25 && buffer[1] == 0x50
                    && buffer[2] == 0x44 && buffer[3] == 0x46)
                {
                    rtrn = header.StartsWith("%PDF-");
                }
                fs.Close();
                br.Close();
                return rtrn;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ChkImage(string filename)
        {
            try
            {
                bool rtrn = false;
                System.Drawing.Image img = System.Drawing.Image.FromFile(filename);
                if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                {
                    rtrn = true;
                }
                if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                {
                    rtrn = true;
                }
                if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                {
                    rtrn = true;
                }
                if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
                {
                    rtrn = true;
                }

                img.Dispose();
                return rtrn;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool sendsms(string msg, string phone_no, string template_id)
        {
            string str_response = string.Empty;
            try
            {
                string sms = "";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sms);
                ServicePointManager.ServerCertificateValidationCallback = new
                RemoteCertificateValidationCallback
                (
                   delegate { return true; }
                );
                WebResponse response = req.GetResponse();
                StreamReader responseStream = new StreamReader(response.GetResponseStream());
                str_response = responseStream.ReadToEnd();
                responseStream.Close();
                string[] str_tmp = str_response.Trim().Split(' ');
                if (str_tmp[1].Trim() == "Accepted")
                    return true;
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool sendsms(string msg, string mobile)
        {
            try
            {
                string _resultUC = msg.Replace("&", "and");
                string sms = "";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sms);
                ServicePointManager.ServerCertificateValidationCallback = new
                RemoteCertificateValidationCallback
                (
                   delegate { return true; }
                );
                WebResponse response = req.GetResponse();
                StreamReader responseStream = new StreamReader(response.GetResponseStream());
                string str_response = responseStream.ReadToEnd();
                responseStream.Close();
                string[] str_tmp = str_response.Trim().Split(' ');
                if (str_tmp[1].Trim() == "Accepted")
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool email(string display_name, string email_from, string usr_id, string pwd, bool enablessl, int port, string smpt_host, string email_to, string subject, string body, List<string> list_attachment)
        {
            MailMessage mail = new MailMessage();
            try
            {
                if (subject.Trim().Length == 0 && body.Trim().Length == 0)
                {
                    return false;
                }
                string[] email_arr = email_to.Split(',');
                for (int i = 0; i < email_arr.Length; i++)
                {
                    Regex rgx_email = new Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$");
                    if (!rgx_email.IsMatch(email_arr[i]))
                    {
                        return false;
                    }
                }
                SmtpClient smtpClient = new SmtpClient(smpt_host, port);
                smtpClient.Credentials = new NetworkCredential(usr_id, pwd);
                smtpClient.EnableSsl = enablessl;
                //string FromAddress = email_from; // Sender EmailID

                mail.From = new MailAddress(email_from, display_name);
                mail.To.Add(email_to);
                //mail.Bcc.Add(email_from);
                mail.Body = body;
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                foreach (var item in list_attachment)
                {
                    mail.Attachments.Add(new Attachment(item));
                }

                smtpClient.Send(mail);
                mail.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                mail.Attachments.Dispose();
                mail.Dispose();
                return false;
            }
        }

        public async Task<bool> emailAsync(string display_name, string email_from, string usr_id, string pwd, bool enablessl, int port, string smpt_host, string email_to, string subject, string body, List<string> list_attachment)
        {
            MailMessage mail = new MailMessage();
            try
            {
                if (subject.Trim().Length == 0 && body.Trim().Length == 0)
                {
                    return false;
                }
                string[] email_arr = email_to.Split(',');
                for (int i = 0; i < email_arr.Length; i++)
                {
                    Regex rgx_email = new Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$");
                    if (!rgx_email.IsMatch(email_arr[i]))
                    {
                        return false;
                    }
                }
                SmtpClient smtpClient = new SmtpClient(smpt_host, port);
                smtpClient.Credentials = new NetworkCredential(usr_id, pwd);
                smtpClient.EnableSsl = enablessl;
                //string FromAddress = email_from; // Sender EmailID

                mail.From = new MailAddress(email_from, display_name);
                mail.To.Add(email_to);
                //mail.Bcc.Add(email_from);
                mail.Body = body;
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                foreach (var item in list_attachment)
                {
                    mail.Attachments.Add(new Attachment(item));
                }
                await Task.Run(() =>
                {
                    smtpClient.Send(mail);
                    mail.Dispose();
                });
                return true;
            }
            catch (Exception ex)
            {
                await Task.Run(() =>
                {
                    mail.Attachments.Dispose();
                    mail.Dispose();
                });
                return false;
            }
        }


        public bool email(string email_to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            try
            {
                if (subject.Trim().Length == 0 && body.Trim().Length == 0)
                {
                    return false;
                }
                string[] email_arr = email_to.Split(',');
                for (int i = 0; i < email_arr.Length; i++)
                {
                    Regex rgx_email = new Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$");
                    if (!rgx_email.IsMatch(email_arr[i]))
                    {
                        return false;
                    }
                }
                SmtpClient smtpClient = new SmtpClient("relay.nic.in", 25);
                //smtpClient.Credentials = new NetworkCredential("epresi.auth", "s*93$yAq");
                smtpClient.EnableSsl = false;
                //string FromAddress = email_from; // Sender EmailID

                mail.From = new MailAddress("support-rb@nic.in", "RBNDC");
                mail.To.Add(email_to);
                //mail.Bcc.Add(email_from);
                mail.Body = body;
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                smtpClient.Send(mail);
                mail.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                mail.Attachments.Dispose();
                mail.Dispose();
                return false;
            }
        }


        public string Decrypt(string cipherText)
        {

            if (cipherText.Contains("abhi1"))
            {
                cipherText = cipherText.Replace("abhi1", "+");
            }
            if (cipherText.Contains("abhi2"))
            {
                cipherText = cipherText.Replace("abhi2", "?");
            }
            if (cipherText.Contains("abhi3"))
            {
                cipherText = cipherText.Replace("abhi3", "&");
            }
            if (cipherText.Contains("abhi4"))
            {
                cipherText = cipherText.Replace("abhi4", "/");
            }
            if (cipherText.Contains("abhi5"))
            {
                cipherText = cipherText.Replace("abhi5", ":");
            }

            cipherText = cipherText.Replace(' ', '+');
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            string reg_no = clearText;
            if (reg_no.Contains("+"))
            {
                reg_no = reg_no.Replace("+", "abhi1");
            }

            if (reg_no.Contains("?"))
            {
                reg_no = reg_no.Replace("?", "abhi2");
            }

            if (reg_no.Contains("&"))
            {
                reg_no = reg_no.Replace("&", "abhi3");
            }

            if (reg_no.Contains("/"))
            {
                reg_no = reg_no.Replace("/", "abhi4");
            }

            if (reg_no.Contains(":"))
            {
                reg_no = reg_no.Replace(":", "abhi5");
            }

            return reg_no;
        }

        public string getddmmyy(string mmddyy)
        {
            try
            {
                string[] temp = mmddyy.Split('/');
                string tempm = temp[0];
                string tempd = temp[1];
                string tempy = temp[2];
                string newd = tempd + "/" + tempm + "/" + tempy;
                return newd;
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        public string getyyyymmdd(string ddmmyy)
        {
            try
            {
                string[] temp = ddmmyy.Split('/');
                string tempd = temp[0];
                string tempm = temp[1];
                string tempy = temp[2];
                string newd = tempy + "-" + tempm + "-" + tempd;
                return newd;
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        public string sha256(string strdata)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(strdata));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string UserMenu(string[] roles_list_user, string PageName)
        {
            PageName = PageName.ToLower();
            string str = string.Empty;
            try
            {

                str = @"<nav class='mt-2'>
                    <ul class='nav nav-pills nav-sidebar flex-column' data-widget='treeview' role='menu' data-accordion='false'>";

                str += @"<li class='nav-item'>";
                if (PageName == "index")
                {
                    str += @"<a href='../dashboard/index' class='nav-link active' >";
                }
                else
                {
                    str += @"<a href='../dashboard/index' class='nav-link'>";
                }
                str += @"<i class='nav-icon fas fa-tachometer-alt'></i>
                                <p>Dashboard</p>
                            </a>
                        </li>";


                if (allow_roles(new string[] { "HelpDesk", "TeamLead", "Developer", "ProjectManager", "Director" }, roles_list_user))
                {
                    str += @"<li class='nav-item'>";
                    if (PageName == "newrequest")
                    {
                        str += @"<a href='../request/newrequest' class='nav-link active'>";
                    }
                    else
                    {
                        str += @"<a href='../request/newrequest' class='nav-link'>";
                    }
                    str += @"<i class='nav-icon fas fa-circle-plus'></i>
                                <p>Raise Ticket</p>
                            </a>
                        </li>";
                }

                if (allow_roles(new string[] { "HelpDesk" }, roles_list_user))
                {
                    if (allow_roles(new string[] { "TeamLead", "Developer", "ProjectManager", "Director" }, roles_list_user))
                    {
                        str += @"<li class='nav-item'>";
                        if (PageName == "mytask")
                        {
                            str += @"<a href='../request/mytask' class='nav-link active'>";
                        }
                        else
                        {
                            str += @"<a href='../request/mytask' class='nav-link'>";
                        }
                        str += @"<i class='nav-icon fas fa-tasks'></i>
                                <p>My Task</p>
                            </a>
                        </li>";
                    }
                }



                #region Master
                int cnt = 0;
                string str_inner = string.Empty;
                cnt = 0;
                if (PageName == "makemaster" || PageName == "modelmaster" || PageName == "enginecapacitymaster" || PageName == "enquirymaster" || PageName == "bodytypemaster" || PageName == "fuelmaster" || PageName == "transmissionmaster" || PageName == "ratingmaster" || PageName == "colourmaster" || PageName == "homeimage" || PageName == "categorymaster")
                {
                    str_inner += @"<li class='nav-item menu-is-opening menu-open'>
                                    <a href='#' class='nav-link active'>
                                        <i class='nav-icon fas fa-navicon'> </i>
                                        <p>Masters<i class='fas fa-angle-right right'></i></p>
                                    </a>
                                    <ul class='nav nav-treeview'>";
                }
                else
                {
                    str_inner += @"<li class='nav-item'>
                                        <a href='#' class='nav-link'>
                                            <i class='nav-icon fas fa-navicon'> </i>
                                            <p>Masters<i class='fas fa-angle-right right'></i></p>
                                         </a>
                           <ul class='nav nav-treeview' style='display:none;'>";
                }

                //if (allow_roles(new string[] { "Admin" }, roles_list_user))
                //{
                //    if (PageName == "bodytypemaster")
                //    {
                //        str_inner += "<li class='nav-item'><a href='../master/bodytypemaster' class='nav-link active' ><p>Body Type</p></a></li>";
                //    }
                //    else
                //    {
                //        str_inner += "<li class='nav-item'><a href='../master/bodytypemaster' class='nav-link'><p>Body Type</p></a></li>";
                //    }
                //}

                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {
                    if (PageName == "homeimage")
                    {
                        str_inner += "<li class='nav-item'><a href='../master/homeimage' class='nav-link active' ><p>Home Image</p></a></li>";
                    }
                    else
                    {
                        str_inner += "<li class='nav-item'><a href='../master/homeimage' class='nav-link'><p>Home Image</p></a></li>";
                    }
                }

                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {
                    if (PageName == "colourmaster")
                    {
                        str_inner += "<li class='nav-item'><a href='../master/colourmaster' class='nav-link active' ><p>Vehicle Color</p></a></li>";
                    }
                    else
                    {
                        str_inner += "<li class='nav-item'><a href='../master/colourmaster' class='nav-link'><p>Vehicle Color</p></a></li>";
                    }
                }
                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {
                    if (PageName == "categorymaster")
                    {
                        str_inner += "<li class='nav-item'><a href='../master/categorymaster' class='nav-link active' ><p>Vehicle Category</p></a></li>";
                    }
                    else
                    {
                        str_inner += "<li class='nav-item'><a href='../master/categorymaster' class='nav-link'><p>Vehicle Category</p></a></li>";
                    }
                    cnt++;
                }
                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {
                    if (PageName == "makemaster")
                    {
                        str_inner += "<li class='nav-item'><a href='../master/makemaster' class='nav-link active' ><p>Vehicle Make</p></a></li>";
                    }
                    else
                    {
                        str_inner += "<li class='nav-item'><a href='../master/makemaster' class='nav-link'><p>Vehicle Make</p></a></li>";
                    }
                    cnt++;
                }

                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {
                    if (PageName == "modelmaster")
                    {
                        str_inner += "<li class='nav-item'><a href='../master/modelmaster' class='nav-link active' ><p>Vehicle Model</p></a></li>";
                    }
                    else
                    {
                        str_inner += "<li class='nav-item'><a href='../master/modelmaster' class='nav-link'><p>Vehicle Model</p></a></li>";
                    }
                }

                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {
                    if (PageName == "fuelmaster")
                    {
                        str_inner += "<li class='nav-item'><a href='../master/fuelmaster' class='nav-link active' ><p>Fuel Type</p></a></li>";
                    }
                    else
                    {
                        str_inner += "<li class='nav-item'><a href='../master/fuelmaster' class='nav-link'><p>Fuel Type</p></a></li>";
                    }
                }

                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {
                    if (PageName == "transmissionmaster")
                    {
                        str_inner += "<li class='nav-item'><a href='../master/transmissionmaster' class='nav-link active' ><p>Transmission Type</p></a></li>";
                    }
                    else
                    {
                        str_inner += "<li class='nav-item'><a href='../master/transmissionmaster' class='nav-link'><p>Transmission Type</p></a></li>";
                    }
                }

                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {
                    if (PageName == "ratingmaster")
                    {
                        str_inner += "<li class='nav-item'><a href='../master/ratingmaster' class='nav-link active' ><p>Rating</p></a></li>";
                    }
                    else
                    {
                        str_inner += "<li class='nav-item'><a href='../master/ratingmaster' class='nav-link'><p>Rating</p></a></li>";
                    }
                }

                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {
                    if (PageName == "enginecapacitymaster")
                    {
                        str_inner += "<li class='nav-item'><a href='../master/enginecapacitymaster' class='nav-link active' ><p>Engine Capacity</p></a></li>";
                    }
                    else
                    {
                        str_inner += "<li class='nav-item'><a href='../master/enginecapacitymaster' class='nav-link'><p>Engine Capacity</p></a></li>";
                    }
                }

                //if (allow_roles(new string[] { "Admin" }, roles_list_user))
                //{
                //    if (PageName == "enquirymaster")
                //    {
                //        str_inner += "<li class='nav-item'><a href='../master/enquirymaster' class='nav-link active' ><p>Enquiry</p></a></li>";
                //    }
                //    else
                //    {
                //        str_inner += "<li class='nav-item'><a href='../master/enquirymaster' class='nav-link'><p>Enquiry</p></a></li>";
                //    }
                //}


                str_inner += "	</ul></li>";
                if (cnt > 0)
                {
                    str += str_inner;
                }
                #endregion

                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {
                    str += @"<li class='nav-item'>";
                    if (PageName == "contentmanagement")
                    {
                        str += @"<a href='../master/contentmanagement' class='nav-link active'>";
                    }
                    else
                    {
                        str += @"<a href='../master/contentmanagement' class='nav-link'>";
                    }
                    str += @"<i class='nav-icon fas fa-file-text'></i>
                                <p>Content Management</p>
                            </a>
                        </li>";
                }
                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {

                    str += @"<li class='nav-item'>";
                    if (PageName == "vehicleapproval")
                    {
                        str += @"<a href='../request/vehicleapproval' class='nav-link active'>";
                    }
                    else
                    {
                        str += @"<a href='../request/vehicleapproval' class='nav-link'>";
                    }
                    str += @"<i class='nav-icon fas fa-car'></i>
                                <p>Vehicle Approval</p>
                            </a>
                        </li>";
                }
                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {

                    str += @"<li class='nav-item'>";
                    if (PageName == "reviewapproval")
                    {
                        str += @"<a href='../request/reviewapproval' class='nav-link active'>";
                    }
                    else
                    {
                        str += @"<a href='../request/reviewapproval' class='nav-link'>";
                    }
                    str += @"<i class='nav-icon fas fa-star'></i>
                                <p>Review Approval</p>
                            </a>
                        </li>";
                }
                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {

                    str += @"<li class='nav-item'>";
                    if (PageName == "tradesellerapproval")
                    {
                        str += @"<a href='../request/tradesellerapproval' class='nav-link active'>";
                    }
                    else
                    {
                        str += @"<a href='../request/tradesellerapproval' class='nav-link'>";
                    }
                    str += @"<i class='nav-icon fas fa-users'></i>
                                <p>Trade Seller Approval</p>
                            </a>
                        </li>";
                }
                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {

                    str += @"<li class='nav-item'>";
                    if (PageName == "reviewlist")
                    {
                        str += @"<a href='../request/reviewlist' class='nav-link active'>";
                    }
                    else
                    {
                        str += @"<a href='../request/reviewlist' class='nav-link'>";
                    }
                    str += @"<i class='nav-icon fas fa-star'></i>
                                <p>Review List</p>
                            </a>
                        </li>";
                }
                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {

                    str += @"<li class='nav-item'>";
                    if (PageName == "carslist")
                    {
                        str += @"<a href='../request/carslist' class='nav-link active'>";
                    }
                    else
                    {
                        str += @"<a href='../request/carslist' class='nav-link'>";
                    }
                    str += @"<i class='nav-icon fas fa-car'></i>
                                <p>Car List</p>
                            </a>
                        </li>";
                }
                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {
                    str += @"<li class='nav-item'>";
                    if (PageName == "userslist")
                    {
                        str += @"<a href='../master/userslist' class='nav-link active'>";
                    }
                    else
                    {
                        str += @"<a href='../master/userslist' class='nav-link'>";
                    }
                    str += @"<i class='nav-icon fas fa-users'></i>
                                <p>Customer List</p>
                            </a>
                        </li>";
                }

                if (allow_roles(new string[] { "Admin" }, roles_list_user))
                {

                    str += @"<li class='nav-item'>";
                    if (PageName == "enquirylist")
                    {
                        str += @"<a href='../request/enquirylist' class='nav-link active'>";
                    }
                    else
                    {
                        str += @"<a href='../request/enquirylist' class='nav-link'>";
                    }
                    str += @"<i class='nav-icon fa fa-question-circle'></i>
                                <p>Enquiry List</p>
                            </a>
                        </li>";
                }


                //if (allow_roles(new string[] { "Admin" }, roles_list_user))
                //{
                //    str += @"<li class='nav-item'>";
                //    if (PageName == "emailconfig")
                //    {
                //        str += @"<a href='../Admin/EmailConfig' class='nav-link active'>";
                //    }
                //    else
                //    {
                //        str += @"<a href='../Admin/EmailConfig' class='nav-link'>";
                //    }
                //    str += @"<i class='nav-icon fas fa-cog'></i>
                //                <p>Email Configuration</p>
                //            </a>
                //        </li>";
                //}

                if (allow_roles(new string[] { "TeamLead", "Developer", "ProjectManager", "Director", "HelpDesk" }, roles_list_user))
                {
                    str += @"<li class='nav-item'>";
                    if (PageName == "searchticket")
                    {
                        str += @"<a href='../request/searchticket' class='nav-link active'>";
                    }
                    else
                    {
                        str += @"<a href='../request/searchticket' class='nav-link'>";
                    }
                    str += @"<i class='nav-icon fas fa-search'></i>
                                <p>Search Ticket</p>
                            </a>
                        </li>";
                }

                //if (allow_roles(new string[] { "TeamLead", "Developer", "ProjectManager", "Director" }, roles_list_user))
                //{
                //   str += @"<li class='nav-item'>";
                //    if (PageName == "memberticketsearch")
                //    {
                //        str += @"<a href='../request/MemberTicketSearch' class='nav-link active'>";
                //    }
                //    else
                //    {
                //        str += @"<a href='../request/MemberTicketSearch' class='nav-link'>";
                //    }
                //    str += @"<i class='nav-icon fas fa-search'></i>
                //                <p>Search Ticket</p>
                //            </a>
                //        </li>";
                //}

                if (allow_roles(new string[] { "admin" }, roles_list_user))
                {
                    str += @"<li class='nav-item'>
                                <a href = '../admin/ChangePwd' ";

                    if (PageName == "changepwd")
                    {
                        str += "class='nav-link active'>";
                    }
                    else
                    {
                        str += "class='nav-link'>";
                    }
                    str += @"<i class='nav-icon fas fa-user-secret'></i>
                            <p>Change Password</p>
                        </a>
                    </li>";
                }

                str += @"</ul></nav>";
                return str;
            }
            catch (Exception ex)
            {
                Log.instance.WriteLog("userMenu" + ex.Message);
                return ("user not found");
            }
        }


        public bool allow_roles(string[] roles_list_accepted, string[] roles_list_user)
        {
            bool rtrn_value = false;
            foreach (var item in roles_list_user)
            {
                rtrn_value = roles_list_accepted.Contains(item);
                if (rtrn_value == true)
                {
                    return rtrn_value;
                }
            }
            return rtrn_value;
        }

       
    }


    public class Log
    {
        private static int cnt = 0;
        public static Log instance = null;
        public static Log Getinstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Log();
                }
                return instance;
            }
        }

        private Log()
        {
            cnt++;
        }


        public void WriteLog(string log_msg)
        {
            var Path = Directory.GetCurrentDirectory();
            StreamWriter objSw = null;
            string sFolderName = Path + "\\log\\";
            //string sFolderName = Path + "\\wwwroot\\writereaddata\\";
            //string sFolderName = "C:\\websites\\carsale.projectstatus.in\\log\\";
            if (!Directory.Exists(sFolderName))
                Directory.CreateDirectory(sFolderName);
            string sFilePath = sFolderName + "Transaction.log";
            objSw = new StreamWriter(sFilePath, true);
            objSw.WriteLine(DateTime.Now.ToString() + " " + log_msg);
            objSw.Flush();
            objSw.Dispose();
        }
    }
    public class CatpchaImage
    {
        public static string SESSION_CAPTCHA = "CAPTCHA";
        const int default_width = 140;
        const int default_height = 40;

        protected Bitmap result = null;

        public int Width;
        public int Height;

        public CatpchaImage()
        {
            InitBitmap(default_width, default_height);
            rnd = new Random();
        }

        public CatpchaImage(int width, int height)
        {
            InitBitmap(width, height);
        }

        protected void InitBitmap(int width, int height)
        {
            result = new Bitmap(width, height);
            Width = width;
            Height = height;
            rnd = new Random();
        }

        public PointF Noise(PointF p, double eps)
        {
            p.X = Convert.ToSingle(rnd.NextDouble() * eps * 2 - eps) + p.X;
            p.Y = Convert.ToSingle(rnd.NextDouble() * eps * 2 - eps) + p.Y;
            return p;
        }

        public PointF Wave(PointF p, double amp, double size)
        {
            p.Y = Convert.ToSingle(Math.Sin(p.X / size) * amp) + p.Y;
            p.X = Convert.ToSingle(Math.Sin(p.X / size) * amp) + p.X;
            return p;
        }

        public GraphicsPath RandomWarp(GraphicsPath path)
        {
            // Add line //
            int PsCount = 10;
            PointF[] curvePs = new PointF[PsCount * 2];
            for (int u = 0; u < PsCount; u++)
            {
                curvePs[u].X = u * (Width / PsCount);
                curvePs[u].Y = Height / 2;
            }
            for (int u = PsCount; u < (PsCount * 2); u++)
            {
                curvePs[u].X = (u - PsCount) * (Width / PsCount);
                curvePs[u].Y = Height / 2 + 2;
            }


            double eps = Height * 0.05;

            double amp = rnd.NextDouble() * (double)(Height / 3);
            double size = rnd.NextDouble() * (double)(Width / 4) + Width / 8;

            double offset = (double)(Height / 3);


            PointF[] pn = new PointF[path.PointCount];
            byte[] pt = new byte[path.PointCount];

            GraphicsPath np2 = new GraphicsPath();

            GraphicsPathIterator iter = new GraphicsPathIterator(path);
            for (int i = 0; i < iter.SubpathCount; i++)
            {
                GraphicsPath sp = new GraphicsPath();
                bool closed;
                iter.NextSubpath(sp, out closed);

                Matrix m = new Matrix();
                m.RotateAt(Convert.ToSingle(rnd.NextDouble() * 30 - 15), sp.PathPoints[0]);

                m.Translate(-1 * i, 0);//uncomment

                sp.Transform(m);

                np2.AddPath(sp, true);
            }




            for (int i = 0; i < np2.PointCount; i++)
            {
                //pn[i] = Noise( path.PathPoints[i] , eps);
                pn[i] = Wave(np2.PathPoints[i], amp, size);
                pt[i] = np2.PathTypes[i];
            }

            GraphicsPath newpath = new GraphicsPath(pn, pt);

            return newpath;

        }

        Random rnd;

        public string DrawNumbers(int len)
        {
            string str = "";
            string possible = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789abcdfghjkmnpqrstvwxyz";
            char ch = 'A';
            for (int i = 0; i < len; i++)
            {
                ch = possible[rnd.Next(0, possible.Length - 1)];
                str = str + ch.ToString();
            }

            DrawText(str);
            return str;
        }

        public void DrawText(string aText)
        {
            Graphics g = Graphics.FromImage(result);
            int startsize = Height;
            Font f = new Font("Verdana", startsize, FontStyle.Bold, GraphicsUnit.Pixel);

            do
            {
                f = new Font("Verdana", startsize, GraphicsUnit.Pixel);
                startsize--;
            } while ((g.MeasureString(aText, f).Width >= Width) || (g.MeasureString(aText, f).Height >= Height));
            SizeF sf = g.MeasureString(aText, f);
            int width = Convert.ToInt32(sf.Width);
            int height = Convert.ToInt32(sf.Height);

            int x = Convert.ToInt32(Math.Abs((double)width - (double)Width) * rnd.NextDouble());
            int y = Convert.ToInt32(Math.Abs((double)height - (double)Height) * rnd.NextDouble());

            //////// Paths ///
            GraphicsPath path = new GraphicsPath(FillMode.Alternate);
            FontFamily family = new FontFamily("Verdana");
            int fontStyle = (int)(FontStyle.Regular);
            float emSize = f.Size;
            Point origin = new Point(x, y);
            StringFormat format = StringFormat.GenericDefault;
            path.AddString(aText, family, fontStyle, emSize, origin, format);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            g.FillRectangle(new System.Drawing.Drawing2D.LinearGradientBrush(rect, Color.BurlyWood, Color.BurlyWood, 0f), rect);
            g.SmoothingMode = SmoothingMode.HighQuality;


            Color noiseCol = Color.FromArgb(23, 3, 118);
            Pen p = new Pen(noiseCol);


            /* generate random dots in background */
            for (int i = 0; i < (Width * Height) / 3; i++)
            {
                g.FillEllipse(Brushes.DarkGreen, rnd.Next(0, Width), rnd.Next(0, Height), 1, 1);
            }
            /*noise ends here*/

            /* generate random lines in background */
            for (int i = 0; i < (Width * Height) / 150; i++)
            {
                g.DrawLine(p, rnd.Next(0, Width), rnd.Next(0, Height), rnd.Next(0, Width), rnd.Next(0, Height));
            }

            Color textColor = Color.FromArgb(255, 255, 255);
            g.FillPath(new SolidBrush(textColor), path);

            g.Dispose();
        }

        public Bitmap Result
        {
            get
            {
                return result;
            }
        }
    }
}
