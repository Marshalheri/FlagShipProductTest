using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace FlagshipProductTest.Shared.Services
{
    public static class Util
    {
        static JsonSerializerSettings settings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore };
        public static string SerializeAsJson<T>(T item)
        {
            return JsonConvert.SerializeObject(item);
        }
        public static T DeserializeFromJson<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input, settings);
        }
        public static string GetEnumDescription(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }
        public static T Clone<T>(this T request)
        {
            var serializedRequest = SerializeAsJson(request);
            return DeserializeFromJson<T>(serializedRequest);
        }
        public static T Sanitize<T>(this T request, string[] propertiesToMask)
        {
            var type = request.GetType();
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType.Name.ToLower() == "string")
                {
                    if (propertiesToMask.Contains(property.Name))
                    {
                        property.SetValue(request, "*******");
                    }
                }
                else if (propertiesToMask.Contains(property.Name))
                {
                    property.SetValue(request, null);
                }
            }
            return request;
        }
        public static bool IsBase64String(string input)
        {
            Span<byte> buffer = new Span<byte>(new byte[input.Length]);
            return Convert.TryFromBase64String(input, buffer, out int bytesParsed);
        }
        public static string GetUsername(this IIdentity identity)
        {
            return identity.GetClaimValue("Username");
        }
        private static string GetClaimValue(this IIdentity identity, string claimType)
        {
            var claimIdentity = (ClaimsIdentity)identity;
            return claimIdentity.Claims.GetClaimValue(claimType);
        }
        private static string GetClaimValue(this IEnumerable<Claim> claims, string claimType)
        {
            var claimsList = new List<Claim>(claims);
            var claim = claimsList.Find(c => c.Type == claimType);
            return claim?.Value;
        }
        public static string AdminAccess()
        {
            return "SuperAdmin@123";
        }
    }
}
