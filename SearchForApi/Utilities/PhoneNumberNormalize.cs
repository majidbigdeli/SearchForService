using System;
using SearchForApi.Models.Exceptions;

namespace SearchForApi.Utilities
{
    public static class PhoneNumberNormalize
    {
        public static string NormalizePhoneNumber(this string phoneNumber)
        {
            var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
            var phoneNumberItems = phoneNumberUtil.Parse(phoneNumber, "IR");

            var nationalNumber = phoneNumberItems.NationalNumber.ToString();

            var normalizedPhoneNumber = $"{phoneNumberItems.CountryCode}{nationalNumber}";

            if (nationalNumber.Length != 10 ||
                !nationalNumber.StartsWith("9") ||
                phoneNumberItems.CountryCode != 98)
                throw new ValidationException();

            return normalizedPhoneNumber;
        }

        public static string NationalizePhoneNumber(this string phoneNumber)
        {
            var normalizedPhoneNumber = $"+{NormalizePhoneNumber(phoneNumber)}";
            return normalizedPhoneNumber;
        }

        public static string MaskPhoneNumber(this string phoneNumber)
        {
            var normalizedPhoneNumber = $"+{NormalizePhoneNumber(phoneNumber)}";
            var maskedPhoneNumber = "0" + normalizedPhoneNumber.Substring(3, 3) + "***" + normalizedPhoneNumber.Substring(9, 4);

            return maskedPhoneNumber;
        }
    }
}

