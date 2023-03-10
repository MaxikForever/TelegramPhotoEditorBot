using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Args;
using Microsoft.VisualBasic.FileIO;
using System.Linq.Expressions;

namespace TelegramBotProject
{
    class Program
    {

        static TelegramBotClient botClient = new TelegramBotClient("6166133846:AAGKCg8QsHwDAM9nc3lWEuI_EmL72k1xFAs");
        private static string destinationFilePathGlobal;
        static string fileName;
        private static CancellationTokenSource cts = new CancellationTokenSource();
        static  string editedPhoto = @"C:\Users\Max\Desktop\Saltman Edited\" + fileName + ".jpg";
        static bool introSended = false; 

        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
     

            botClient.StartReceiving(Update, Error, cancellationToken: cancellationToken);



            Console.ReadLine();
        }


        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {

         
            var message = update.Message;
          

            if (update != null && update.CallbackQuery != null)
            Console.WriteLine("callback data = " + update.CallbackQuery.Data);
       

            if (message != null)
            Console.WriteLine($"INFO :   Chat ID :  {message.Chat.Id}\n  Username : {message.Chat.Username} Name : {message.Chat.FirstName} LastName : {message.From.LastName} \n Message text : {message.Text}");
            string response = "";


            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {

                if (!introSended)
                {
                    // Send an introduction message to the user
                    string introMessage = "Hello! I am a bot that can help you edit your photos with filters. " +
                        "To get started, send me a photo and I'll give you some filter options to choose from. " +
                        "After you choose a filter, I'll send you back the edited photo. " +
                        "Type /help to see this message again.";
                    await botClient.SendTextMessageAsync(message.Chat.Id, introMessage);
                    introSended = true; 
                }


                if (message.Text != null)
                {

                    Console.WriteLine($" ID =  {message.Chat.Id}  Name = {message.Chat.FirstName}  Location =  {message.Chat.Location}");

                 if(message.Text.ToLower().Equals("/help"))
                    {
                        string introMessage = "Hello! I am a bot that can help you edit your photos with filters. " +
                      "To get started, send me a photo and I'll give you some filter options to choose from. " +
                      "After you choose a filter, I'll send you back the edited photo. " +
                      "Type /help to see this message again.";
                        await botClient.SendTextMessageAsync(message.Chat.Id, introMessage);
                    }


                    switch (message.Text.ToLower())
                    {
                        case "hi":
                        case "hello":
                        case "hey":
                            response = "Hello, how can I assist you?";
                            break;
                        case "what's your name?":
                        case "who are you?":
                            response = "My name is Bot, pleased to meet you!";
                            break;
                        case "how are you?":
                        case "how are you doing?":
                            response = "I'm doing well, thanks for asking. How can I help you?";
                            break;
                        case "what can you do?":
                        case "what are your capabilities?":
                            response = "I can perform a variety of tasks, such as answer questions, give recommendations, and provide information. What can I help you with?";
                            break;
                        case "thank you":
                        case "thanks":
                            response = "You're welcome!";
                            break;
                        case "bye":
                        case "goodbye":
                            response = "Goodbye, have a great day!";
                            break;
                        case "what's the weather like?":
                        case "what's the weather forecast?":
                            response = "The weather forecast for today is partly cloudy with a high of 75°F.";
                            break;
                        case "what's the time?":
                        case "what time is it?":
                            response = $"The current time is {DateTime.Now.ToString("h:mm tt")}.";
                            break;
                        case "tell me a joke":
                        case "make me laugh":
                            response = "Why did the chicken cross the road? To get to the other side!";
                            break;
                        case "what's your favorite color?":
                            response = "I don't have a favorite color, I'm a bot!";
                            break;
                        case "what's your favorite food?":
                            response = "I don't eat, I'm a bot!";
                            break;
                        case "how old are you?":
                            response = "I don't age, I'm a bot!";
                            break;
                        case "what's the meaning of life?":
                            response = "That's a difficult question, what do you think the meaning of life is?";
                            break;
                        case "what's your favorite movie?":
                            response = "I don't watch movies, I'm a bot!";
                            break;
                        case "do you have any siblings?":
                            response = "No, I don't have any siblings, I'm a bot!";
                            break;
                        case "where are you from?":
                            response = "I was created by a programmer, so you could say I'm from the internet!";
                            break;
                        case "what's the capital of France?":
                            response = "The capital of France is Paris.";
                            break;
                        case "what's the largest country in the world?":
                            response = "The largest country in the world is Russia.";
                            break;
                        case "what's the population of China?":
                            response = "As of 2021, the population of China is approximately 1.4 billion people.";
                            break;
                        case "what's the tallest mountain in the world?":
                            response = "The tallest mountain in the world is Mount Everest, which is located in the Himalayas.";
                            break;
                        case "what's the largest ocean in the world?":
                            response = "The largest ocean in the world is the Pacific Ocean.";
                            break;
                        default:
                            response = "Send a photo !!!";
                            break;
                    }

                    await botClient.SendTextMessageAsync(message.Chat.Id, response);

                }


                else if (message.Photo != null)
                {
                    Console.WriteLine("A Photo is received !!! ");
                    var photo = message.Photo.Last(); // Get the last (largest) photo sent
                    var fileId = photo.FileId;
                    var fileInfo = await botClient.GetFileAsync(fileId);
                    var filePath = fileInfo.FilePath;

                    fileName = Guid.NewGuid().ToString();
                    Console.WriteLine("The photo name is : {0} ", fileId);

                    Console.WriteLine(" The file path is " + filePath);

                    // saving the downloaded photo 
                    Console.WriteLine("");
                     destinationFilePathGlobal = @"C:\Users\Max\Desktop\BotPhotos\" + fileName + ".jpg";

                    Console.WriteLine("Started already ");

                    // opening the file 
                    try
                    {
                        await using Stream photoStream = System.IO.File.OpenWrite(destinationFilePathGlobal);
                        await botClient.DownloadFileAsync(filePath: filePath, destination: photoStream);
                        photoStream.Close();

                        await botClient.SendTextMessageAsync(message.Chat.Id, "Your Photo Was Received");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception on opening file fase " + e.Message);
                        await botClient.SendTextMessageAsync(message.Chat.Id, "The file name can't be readed");
                    }


                    // Editing
                    try
                    {
                        ChooseAnFilter(message);
                        return;
                    }
                    catch (Exception e)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, " Error : " + e.Message);
                    }

                }



                else if (message.Document != null)
                {

                    Console.WriteLine("An image is received !!! ");
                    fileName = Guid.NewGuid().ToString();
                    Console.WriteLine("\nFile name is " + fileName);
                    var fileId = update.Message.Document.FileId;
                    var fileInfo = await botClient.GetFileAsync(fileId);
                    var filePath = fileInfo.FilePath;


                    // saving the downloaded file 
                     destinationFilePathGlobal = @"C:\Users\Max\Desktop\BotPhotos\" + fileName + ".jpg";

                    Console.WriteLine("Started already ");
                    try
                    {
                        await using Stream fileStream = System.IO.File.OpenWrite(destinationFilePathGlobal);
                        await botClient.DownloadFileAsync(filePath: filePath, destination: fileStream);
                        fileStream.Close();
                    }
                    catch (Exception e)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "There are some issues : " + e.Message);
                    }

                    await botClient.SendTextMessageAsync(message.Chat.Id, "Your Image Was Received");

                    try
                    {
                        ChooseAnFilter(message);
                        return;
                    }
                    catch (Exception e )
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, " Error : " + e.Message);
                    }
                }

            }

            else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {

                Console.WriteLine("Entered the callback query ");
                Console.WriteLine("Destionation file path = " + destinationFilePathGlobal);
                string option = update.CallbackQuery.Data.ToLower();

                switch (option)
                {
                    case "vintage":
                        {
                            Thread photoShopThread = new Thread(async () =>
                            {

                                try
                                {
                                    Process.Start(@"C:\Users\Max\Desktop\DropLets\Vintage.exe", destinationFilePathGlobal)?.WaitForExit();
                                }
                                catch (Exception ex)
                                {
                                    // Handle any exceptions that might be occurring
                                    Console.WriteLine($"An error occurred while running the process: {ex.Message}");
                                }
                            }
                            );

                            photoShopThread.Start();
                            photoShopThread.Join();
                            editedPhoto = @"C:\Users\Max\Desktop\Saltman Edited\" + fileName + ".jpg";
                        }
                        break;
                    case "insta":
                        Thread photoShopThread2 = new Thread(async () =>
                        {

                            try
                            {
                                Process.Start(@"C:\Users\Max\Desktop\DropLets\Insta.exe", destinationFilePathGlobal)?.WaitForExit();
                            }
                            catch (Exception ex)
                            {
                                // Handle any exceptions that might be occurring
                                Console.WriteLine($"An error occurred while running the process: {ex.Message}");
                            }
                        }
                            );

                        photoShopThread2.Start();
                        photoShopThread2.Join();

                        editedPhoto = @"C:\Users\Max\Desktop\Saltman Edited\" + fileName + ".jpg";

                        break;

                    case "height":
                        Thread photoShopThread3 = new Thread(async () =>
                        {

                            try
                            {
                                Process.Start(@"C:\Users\Max\Desktop\DropLets\TelegramHeightResizer.exe", destinationFilePathGlobal)?.WaitForExit();
                            }
                            catch (Exception ex)
                            {
                                // Handle any exceptions that might be occurring
                                Console.WriteLine($"An error occurred while running the process: {ex.Message}");
                            }
                        }
                           );

                        photoShopThread3.Start();
                        photoShopThread3.Join();
                        editedPhoto = @"C:\Users\Max\Downloads\Stickers\Resized Images\" + fileName + ".png";

                        break;

                    case "width":
                        Thread photoShopThread4 = new Thread(async () =>
                        {

                            try
                            {
                                Process.Start(@"C:\Users\Max\Desktop\DropLets\TelegramWidthResizer.exe", destinationFilePathGlobal)?.WaitForExit();
                            }
                            catch (Exception ex)
                            {
                                // Handle any exceptions that might be occurring
                                Console.WriteLine($"An error occurred while running the process: {ex.Message}");
                            }
                        }
                           );

                        photoShopThread4.Start();
                        photoShopThread4.Join();
                        editedPhoto = @"C:\Users\Max\Downloads\Stickers\Resized Images\" + fileName + ".png";

                        break; 


                };

                try
                {

                 
                    Console.WriteLine("Edited photo path on local machine is : " + editedPhoto);
                    await using Stream stream = System.IO.File.OpenRead(editedPhoto);
                    Console.WriteLine("Open read passed ");
                    await botClient.SendPhotoAsync(chatId: update.CallbackQuery.Message.Chat.Id, photo: new InputOnlineFile(content: stream), caption: "Here is your edited Photo!!!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("this is the message " + e.Message);
                    await botClient.SendTextMessageAsync(chatId: update.CallbackQuery.Message.Chat.Id, e.Message);
                }

                Console.WriteLine("Message handling ended succsesfully ");

            }



            return;
        }


    async static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();
    }


    public static async void ChooseAnFilter(Message message)
    {



        InlineKeyboardMarkup inlineKeyboard = new(new[]
{
                // first row
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Vintage", callbackData: "vintage"),
                    InlineKeyboardButton.WithCallbackData(text: "Insta", callbackData: "insta"),
                },
                // second row
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Make 512 Height", callbackData: "height"),
                    InlineKeyboardButton.WithCallbackData(text: "Make 512 Width", callbackData: "width"),
                },
            });

        Message sentMessage = await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Choose a Filter", replyMarkup: inlineKeyboard, cancellationToken: cts.Token);

        Console.WriteLine("options received !!! ");

    }


     }
  }


