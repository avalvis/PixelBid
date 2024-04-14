// AuctionCreatedFaultConsumer.cs file is responsible for consuming the fault message of the AuctionCreated event. 
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        Console.WriteLine("--> Consuming Faulty Case");

        var exception = context.Message.Exceptions.First();

        // If the exception type is System.ArgumentException, the title of the auction will be changed to Tetris
        if (exception.ExceptionType == "System.Exception")
        {
            context.Message.Message.Title = "Tetris";
            await context.Publish(context.Message.Message);
        }
        else
        {
            Console.WriteLine("Unhandled Exception");
        }
    }
}
