using System.Text;

namespace SimpleEcommerce.Api.Exceptions
{
    public class BusinessLogicException : Exception
    {
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public BusinessLogicException(string error)  : this(new  List<string> { error})
        {
           
        }

        public BusinessLogicException(IEnumerable<string> errors) : base(string.Join("/n", errors))
        {
            Errors = errors;
        }

   }
}
