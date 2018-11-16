using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeout.AddNumbers
{
    public class AddNumbersRequest : IRequest
    {
    }

    public interface IRequest
    {
    }

    public class AddNumbersRequestHandler : IReuqestHandler<AddNumbersRequest>
    {
        public Task<string> HandleAsync(AddNumbersRequest request)
        {
            AddNumbersRequestReceived.Create();

            var result = this.GetType().BaseType.DeclaringMethod.ToString();
            return result;
        }
    }

    public interface IReuqestHandler<in TRequest> where TRequest : IRequest
    {
        Task<string> HandleAsync(TRequest request);

    }

    public interface IEvent
    {

    }

    public interface IEventHandler<in TEvent>
        where TEvent : IEvent
    {
        Task HandleAsync(object sender, TEvent @event);
    }


    public class AddNumbersRequestReceived : IEvent
    {

        public static AddNumbersRequestReceived Create()
        {
            return new AddNumbersRequestReceived();            
        }
    }
}
