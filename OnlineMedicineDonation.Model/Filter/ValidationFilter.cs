using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnlineMedicineDonation.Model.Filter
{
    class ValidateEmail : ValidationAttribute
    {
        private readonly string FieldName;
        public ValidateEmail(string FieldName)
        {
            this.FieldName = FieldName;
        }
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                Regex rgx_email = new Regex(@"^\+?[A-Za-z0-9](([-+.]|[_]+)?[A-Za-z0-9]+)*@([A-Za-z0-9]+(\.|\-))+[A-Za-z]{2,6}$");
                if (!rgx_email.IsMatch(value.ToString()))
                {
                    ErrorMessage = "Invalid characters in " + FieldName;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

        }
    }

    class ValidateIntId : ValidationAttribute
    {
        private readonly string FieldName;
        public ValidateIntId(string FieldName)
        {
            this.FieldName = FieldName;
        }
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                Regex rgx = new Regex(@"^[0-9]{" + value.ToString().Length + "}");
                if (!rgx.IsMatch(value.ToString()))
                {
                    ErrorMessage = "Please Select " + FieldName;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }

    class ValidateMasterName : ValidationAttribute
    {
        private readonly string FieldName;
        public ValidateMasterName(string FieldName)
        {
            this.FieldName = FieldName;
        }
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (value.ToString().Length > 50)
                {
                    ErrorMessage = "Invalid length for " + FieldName;
                    return false;
                }
                Regex rgx = new Regex(@"[a-zA-Z0-9 ]{" + value.ToString().Length + "}");
                if (!rgx.IsMatch(value.ToString()))
                {
                    ErrorMessage = "Invalid characters in " + FieldName;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }

    class ValidateAllowAll : ValidationAttribute
    {
        private readonly string FieldName;
        private readonly int? Length;
        public ValidateAllowAll(string FieldName)
        {
            this.FieldName = FieldName;
        }
        public ValidateAllowAll(string FieldName, int Length)
        {
            this.FieldName = FieldName;
            this.Length = Length;
        }
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (Length != null)
                {
                    if (value.ToString().Length >= Length)
                    {
                        ErrorMessage = "Invalid length for " + FieldName;
                        return false;
                    }
                }
                Regex rgx = new Regex(@"^[^<>\[\]]{" + value.ToString().Length + "}");
                if (!rgx.IsMatch(value.ToString()))
                {
                    ErrorMessage = "Invalid characters in " + FieldName;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }

    class ValidateUserName : ValidationAttribute
    {
        private readonly string FieldName;
        public ValidateUserName(string FieldName)
        {
            this.FieldName = FieldName;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (value.ToString().Length <= 3)
                {
                    ErrorMessage = "Minimum length for " + FieldName + " should be greater 3 characters";
                    return false;
                }
                else if (value.ToString().Length >= 20)
                {
                    ErrorMessage = "Maximum length for " + FieldName + " should be less 20 characters";
                    return false;
                }
                Regex rgx = new Regex(@"^[a-zA-Z0-9@#$%^&*]{" + value.ToString().Length + "}");
                if (!rgx.IsMatch(value.ToString()))
                {
                    ErrorMessage = "Invalid characters in " + FieldName;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }

    class ValidatePassword : ValidationAttribute
    {
        private readonly string FieldName;
        public ValidatePassword(string FieldName)
        {
            this.FieldName = FieldName;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (value.ToString().Length <= 6)
                {
                    ErrorMessage = "Minimum length for " + FieldName + " should be greater 3 characters";
                    return false;
                }
                else if (value.ToString().Length >= 100)
                {
                    ErrorMessage = "Maximum length for " + FieldName + " should be less 20 characters";
                    return false;
                }
                Regex rgx = new Regex(@"^[a-zA-Z0-9 @#$%^&*]{" + value.ToString().Length + "}");
                if (!rgx.IsMatch(value.ToString()))
                {
                    ErrorMessage = FieldName;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }

    class ValidateMatchPassword : ValidationAttribute
    {
        private readonly string FieldName;
        public ValidateMatchPassword(string FieldName)
        {
            this.FieldName = FieldName;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                Regex rgx = new Regex(@"^[a-zA-Z]{" + value.ToString().Length + "}");
                if (!rgx.IsMatch(value.ToString()))
                {
                    ErrorMessage = FieldName;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
