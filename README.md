# WhatsAppApisSender

This project is a .NET 8 application designed to send messages via the WhatsApp Business API, utilizing a SQL database for data persistence.

## Prerequisites

Before you begin, ensure you have the following installed:

* **[.NET 8 SDK](https://dotnet.microsoft.com/en-us/download):** Required for building and running the application.
* **[Visual Studio 2022](https://visualstudio.microsoft.com/):** Recommended for development.
* **[MsSql for Windows](https://go.microsoft.com/fwlink/p/?linkid=2215158&clcid=0x409&culture=en-us&country=us):** For local database setup.

    * If using the default MsSql settings, you can use the following connection string, replacing `1234` with your password:

        ```
        Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=True;
        ```

## Configuration

1.  **Create `appsettings.Development.json`:**

    * In the root directory of the `WhatsAppApisSender` project, create a file named `appsettings.Development.json`.
    * Populate the file with the following JSON structure, replacing the placeholder values with your actual configuration:

        ```json
        {
          "ConnectionStrings": {
            "Database": "your connection string"
          },
          "Jwt": {
            "Issuer": "https://localhost:5001",
            "Audience": "https://localhost:5001",
            "Secret": "p7xkOiFSk3B6Wj9zFJXsY3OEgZcQa2gcrJPt1dOG+dc=",
            "ExpirationInMinutes": 60
          },
          "WhatsAppSettings": {
            "BaseUrl": "https://graph.facebook.com/v22.0/620484351142644/messages"
          },
          "Logging": {
            "LogLevel": {
              "Default": "Information",
              "Microsoft.AspNetCore": "Warning"
            }
          }
        }
        ```

    * **`ConnectionStrings:Database`:** Your MsSql connection string.
    * **`Jwt:Issuer` and `Jwt:Audience`:** The issuer and audience for JWT authentication.
    * **`Jwt:Secret`:** A strong, secure secret key for JWT signing.
    * **`Jwt:ExpirationInMinutes`:** The JWT expiration time in minutes.
    * **`WhatsAppSettings:BaseUrl`:** The base URL for the WhatsApp Business API.

## Running the Application

1.  **Navigate to the Project Directory:**

    * Open your command prompt or terminal.
    * Change the directory to the `WhatsAppApisSender` project folder.

        ```bash
        cd ./WhatsAppApisSender
        ```

2.  **Install Entity Framework Core Tools (if not already installed):**

    * Install the global .NET Entity Framework Core tools:

        ```bash
        dotnet tool install --global dotnet-ef
        ```

3.  **Apply Database Migrations:**

    * Create the initial database migration:

        ```bash
        dotnet ef migrations add InitialCreate
        ```

    * Update the database to apply the migrations:

        ```bash
        dotnet ef database update
        ```

4.  **Run the Application:**

    * Open the `WhatsAppApisSender` project in Visual Studio 2022.
    * Run the application from Visual Studio.

**Important Notes:**

* Ensure your MsSql server is running and accessible.
* Verify that the connection string in `appsettings.Development.json` is correct.
* If you modified the default MsSql port or configuration, update the connection string accordingly.