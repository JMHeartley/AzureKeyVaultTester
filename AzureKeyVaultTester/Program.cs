using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;

namespace AzureKeyVaultTester
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Azure Key Vault Tester!");
            Console.WriteLine();

            var useSameKeyVault = false;
            bool wantsAnotherSecret;
            do
            {
                var keyVaultUri = string.Empty;
                if (!useSameKeyVault)
                {
                    keyVaultUri = PromptForInfo("Enter the URI of the Key Vault you're retrieving from");
                }

                var secretName = PromptForInfo("Enter the name of the secret you'd like to retrieve");

                GetSecret(keyVaultUri, secretName);

                wantsAnotherSecret = AskYesOrNo("Would you like to retrieve another secret?");

                if (wantsAnotherSecret)
                {
                    useSameKeyVault = AskYesOrNo("Would you like to use the same key vault?");
                }
            } while (wantsAnotherSecret);
        }

        private static string PromptForInfo(string prompt)
        {
            var answer = string.Empty;
            while (answer == string.Empty)
            {
                Console.Write($"{prompt}: ");
                answer = Console.ReadLine();
            }

            return answer;
        }

        private static bool AskYesOrNo(string question)
        {
            var answer = new ConsoleKeyInfo();
            while (answer.Key != ConsoleKey.Y && answer.Key != ConsoleKey.N)
            {
                Console.Write($"{question} (y or n) ");
                answer = Console.ReadKey();
                Console.WriteLine();
            }

            return answer.Key == ConsoleKey.Y;
        }

        private static void GetSecret(string keyVaultUri, string secretName)
        {
            Console.WriteLine("Retrieving secret...");
            Console.WriteLine();

            try
            {
                var options = new SecretClientOptions
                {
                    Retry = {
                        Delay = TimeSpan.FromSeconds(2),
                        MaxDelay = TimeSpan.FromSeconds(16),
                        MaxRetries = 5,
                        Mode = RetryMode.Exponential
                    }
                };

                var secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential(), options);
                var response = secretClient.GetSecret(secretName);
                var secretValue = response.Value.Value;

                Console.WriteLine("Secret retrieved!");
                Console.WriteLine($"Name: {secretName}");
                Console.WriteLine($"Value: {secretValue}");
            }
            catch (Azure.RequestFailedException e)
            {
                Console.WriteLine("Secret retrieval failed :(");
                Console.WriteLine($"Error: {e.Status} - {e.ErrorCode}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An exception occurred: {e.Message}");
            }
            finally
            {
                Console.WriteLine();
            }
        }
    }
}
