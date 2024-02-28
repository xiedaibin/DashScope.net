using DashScope;
using DashScope.Models;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddUserSecrets(typeof(Program).Assembly)
                        .Build();

var client = new DashScopeClient(configuration["DashScope:ApiKey"]!);

while (true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("User:");
    var userInput = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(userInput))
    {
        continue;
    }
    if (userInput == "exit")
    {
        break;
    }
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("Bot:");
    //var result = await client.GenerationAsync(new CompletionRequest()
    //{
    //    Input =
    //    {
    //         Messages = new List<Message>()
    //         {
    //              new Message()
    //              {
    //                   Role = "user",
    //                   Content = userInput
    //              }
    //         }
    //    }
    //});

    //if (result.IsTextResponse)
    //{
    //    Console.WriteLine(result.Output.Text);
    //}
    //else
    //{
    //    Console.WriteLine(result.Output.Choices![0].Message.Content);
    //}

    var results = client.GenerationStreamAsync(new CompletionRequest()
    {
        Input =
        {
             Messages = new List<Message>()
             {
                  new Message()
                  {
                       Role = "user",
                       Content = userInput
                  }
             }
        }
    });

    await foreach (var result in results)
    {
        if (result.IsTextResponse)
        {
            Console.WriteLine(result.Output.Text);
        }
        else
        {
            Console.WriteLine(result.Output.Choices![0].Message.Content);
        }
    }
}