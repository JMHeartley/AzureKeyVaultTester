# AzureKeyVaultTester
A console application for testing Azure Key Vault secret retrieval, the code is based on the [quickstart][1] guide by Microsoft. 
By running this program, you can test your Azure Key Vault credential access is set up properly without the overhead of having to run your entire program during troubleshooting.

## How to Use
1. Download the console application
2. Set up [Key Vault credentials][2] one way or another
3. Run application to test key retrieval

[1]: https://learn.microsoft.com/en-us/azure/key-vault/secrets/quick-create-net?tabs=azure-cli
[2]: https://learn.microsoft.com/en-us/dotnet/api/azure.identity.defaultazurecredential?view=azure-dotnet
