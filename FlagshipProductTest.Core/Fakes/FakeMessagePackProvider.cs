using FlagshipProductTest.Shared;
using FlagshipProductTest.Shared.Services;
using System.Collections.Generic;
namespace FlagshipProductTest.Core.Fakes
{
    public class FakeMessagePackProvider : IMessagePackProvider
    {
        readonly IDictionary<int, MessagePack> _packs;
        public MessagePack GetPack()
        {
            return (_packs.TryGetValue(1, out var pack)) ? pack : null;
        }

        public FakeMessagePackProvider()
        {
            _packs = new Dictionary<int, MessagePack>(1)
            {
                { 1, GetMessagePack() },
            };
        }

        private MessagePack GetMessagePack()
        {
            var pack = new MessagePack("We are currently experiencing network issues. Please try again later.")
            {
                Mappings = new Dictionary<string, string>
                {
                    ["FS001"] = "Invalid Request",
                    ["FS002"] = "Username does not match the required format.",
                    ["FS003"] = "This username already exists.",
                    ["FS004"] = "Date of birth is greater than current year.",
                    ["FS005"] = "Is not a valid base 64 string.",
                    ["FS006"] = "Invalid login credentials supplied.",
                    ["FS007"] = "Your profile is locked, kindly reset your password or contact an admin.",
                    ["FS008"] = "Your Profile is inactive, kindly contact an admin.",
                    ["FS009"] = "No contribution found.",
                    ["FS010"] = "No user found.",

                    ["FS999"] = "Opps, something went wrong. This is on us, please try again"
                },
            };

            return pack;
        }
    }
}
