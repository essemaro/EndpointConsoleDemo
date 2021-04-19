using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace EndpointConsoleDemo
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
                
        public class User
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("username")]
            public string Username { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; }

            [JsonPropertyName("phone")]
            public string Phone { get; set; }

            [JsonPropertyName("website")]
            public string Website { get; set; }

            [JsonPropertyName("company.name")]
            public string CompanyName { get; set; }
        }

        static async Task Main(string[] args)
        {
            
            //Retrieves and displays a list of users
            Console.WriteLine("Current Users: ");
            Console.WriteLine();
            var users = await GetUsers();

            foreach (var user in users)
            {
                Console.WriteLine($"User ID : {user.Id}");
                Console.WriteLine($"Name: {user.Name}");
                Console.WriteLine($"Username: {user.Username}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Phone #: {user.Phone}");
                Console.WriteLine($"Website: {user.Website}");
                Console.WriteLine($"Company: {user.CompanyName}");
                Console.WriteLine();
            }

            //Gather user input
            GetUserInput();
            
        }      

        private static async Task<List<User>> GetUsers()
        {
            var streamTask = client.GetStreamAsync("https://my-json-server.typicode.com/essemaro/EndpointConsoleDemo/users");
            var users = await JsonSerializer.DeserializeAsync<List<User>>(await streamTask);

            return users;
        }

        private static void GetUserInput()
        {
            Console.WriteLine("Do you want to ADD, UPDATE, or DELETE a user? Type EXIT or CTRL+C to quit application. ");
            string result = Console.ReadLine();
            result = result.ToUpper();

            //Check input
            if (result == "ADD")
            {
                AddUser().Wait();
                ViewUsers().Wait();
                GetUserInput();
            }
            else if (result == "UPDATE")
            {
                //Add UPDATE FUNCTION
                ViewUsers().Wait();
                GetUserInput();
            }
            else if (result == "DELETE")
            {
                //Add DELETE FUNCTION
                ViewUsers().Wait();
                GetUserInput();
            }
            else if (result == "EXIT")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid response entered. Press a button to continue...");
                Console.WriteLine();
                GetUserInput();
            }
        }

        //Add user to database
        private static async Task AddUser()
        {
            try
            {
                User newUser = new User();

                //User id generation placeholder
                var num = new Random();
                newUser.Id = num.Next(10, 100);

                //Get user information
                //Name
                Console.WriteLine("Enter user's name: ");
                string result = Console.ReadLine();
                //Check for empty value
                if (!UserInputValidation(result))
                {
                    throw new InvalidOperationException("User's name cannot be empty.");
                }
                newUser.Name = result;
                result = ""; //Clearing result to mitigate info redundancy if nothing is entered
                
                //Username
                Console.WriteLine("Enter user's username: ");
                result = Console.ReadLine();
                if (!UserInputValidation(result))
                {
                    throw new InvalidOperationException("User's username cannot be empty.");
                }
                newUser.Username = result;
                result = "";

                //Email
                Console.WriteLine("Enter user's email: ");
                result = Console.ReadLine();
                if (!UserInputValidation(result))
                {
                    throw new InvalidOperationException("User's email cannot be empty.");
                }
                newUser.Email = result;
                result = "";

                //Phone number
                Console.WriteLine("Enter user's phone number: ");
                result = Console.ReadLine();
                newUser.Phone = result;
                result = "";

                //Website 
                Console.WriteLine("Enter user's website: ");
                result = Console.ReadLine();
                newUser.Website = result;
                result = "";

                //Company Name
                Console.WriteLine("Enter user's company name: ");
                result = Console.ReadLine();
                newUser.CompanyName = result;

                //Serialize object to json and convert to HttpContent type
                var content = JsonSerializer.Serialize(newUser);
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                await client.PostAsync("https://my-json-server.typicode.com/essemaro/EndpointConsoleDemo", byteContent);
            }
            catch (Exception ex)
            {

                Console.WriteLine("There was an issue adding a user");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        //Input Validation
        private static bool UserInputValidation(string result)
        {
            if (String.IsNullOrEmpty(result))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //View users after data manipulation
        private static async Task ViewUsers()
        {
            //Retrieves and displays a list of users
            Console.WriteLine("Current Users: ");
            Console.WriteLine();
            var users = await GetUsers();

            foreach (var user in users)
            {
                Console.WriteLine(user.Id);
                Console.WriteLine(user.Name);
                Console.WriteLine(user.Username);
                Console.WriteLine(user.Email);
                Console.WriteLine(user.Phone);
                Console.WriteLine(user.Website);
                Console.WriteLine(user.CompanyName);
                Console.WriteLine();
            }

            GetUserInput();
        }
    }
}
