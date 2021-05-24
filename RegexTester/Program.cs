using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexTester
{
    class Program
    {
        static void Main(string[] args)
        {

            new RegexSplitTests().TestMatchDatePattern();
            //new RegexSplitTests().TestMatchPatterns();
            return;

            string p;
            do
            {
                Console.WriteLine("Type in org name to test");

                p = Console.ReadLine();
                new OrganisationName().Test(p);
            } while (p != "exit");
        }
    }

    public class RegexPattern
    {

        public string Description { get; set; }
    }

    public class OrganisationName
    {

        public const string VALID_REGEX = "^[ -~]{0,50}$";
        private readonly IAddressResolver _addressResolver;
        private readonly ITestAddressResolver _testAddressResolver;

        public OrganisationName() : this(new AddressResolver(), new TestAddressResolver())
        {

        }
        public OrganisationName(IAddressResolver addressResolver, ITestAddressResolver testAddressResolver)
        {
            _addressResolver = addressResolver;
            _testAddressResolver = testAddressResolver;
        }

        public void Test(string value)
        {

            Console.WriteLine("lenth is " + value.Length);
            var isValid = Regex.IsMatch(value, VALID_REGEX); //  StringValidator.Validate(value, VALID_REGEX, nameof(value));
            Console.WriteLine(isValid ? "Is valid" : "Not valid");
        }

        public void TestAddressResolver()
        {
            var postcode = "M25 0GZ";
            var addresses1 = _testAddressResolver.CanHandlePostcode(postcode)
                ? _testAddressResolver.GetAllAddressesByPostcode(postcode)
                : _addressResolver.GetAllAddressesByPostcode(postcode);
            var settingA = true;
            var settingB = true;

            // 
            //var addresses = (settingA && settingB && _testAddressResolver.CanHandlePostcode(postcode)
            //	? _testAddressResolver
            //	: _addressResolver).GetAllAddressesByPostcode(postcode);

        }

    }

    public interface ITestAddressResolver
    {
        bool CanHandlePostcode(string postcode);
        IEnumerable<Address> GetAllAddressesByPostcode(string postcode);
    }
    public interface IAddressResolver
    {
        IEnumerable<Address> GetAllAddressesByPostcode(string postcode);
    }

    public class TestAddressResolver : ITestAddressResolver
    {
        public bool CanHandlePostcode(string postcode)
        {
            return postcode.StartsWith("M25");
        }
        public IEnumerable<Address> GetAllAddressesByPostcode(string postcode)
        {
            return new List<Address>
            {
                new Address() {Description = "1 TestAddress"},
                new Address() {Description = "2 TestAddress"},
                new Address() {Description = "3 TestAddress"}
            };
        }
    }

    public class Address
    {
        public string Description { get; set; }
    }

    public class AddressResolver : IAddressResolver
    {
        public IEnumerable<Address> GetAllAddressesByPostcode(string postcode)
        {
            return new List<Address>
            {
                new Address() {Description = "1 Address"},
                new Address() {Description = "2 Address"},
                new Address() {Description = "3 Address"}
            };
        }
    }

    public class StringValidator
    {
        public static void Validate(string value, string pattern, string paramName)
        {
            if (!Regex.IsMatch(value, pattern))
                throw new ArgumentOutOfRangeException(paramName);
        }
    }
}
