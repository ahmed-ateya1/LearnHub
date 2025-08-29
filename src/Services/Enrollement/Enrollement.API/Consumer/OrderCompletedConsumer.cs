using BuildingBlocks.Messaging.Events;
using Enrollement.API.Enrollement.AddEnrollement;
using MassTransit;
using MediatR;

namespace Enrollement.API.Consumer
{
    public class OrderCompletedConsumer(ISender sender) : IConsumer<OrderCompletedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
        {
            var message = context.Message;

            var tasks = message.courseIds.Select(courseId =>
            {
                var enrollementAdd = new AddEnrollementCommand(courseId, message.userId);
                return sender.Send(enrollementAdd, context.CancellationToken);
            });

            await Task.WhenAll(tasks); 
        }
    }

}
