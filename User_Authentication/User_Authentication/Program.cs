using AuthenticationSystem;
using System;

//entry point
var user = AuthenticationFlow();

if (user != null)
{
    LoginFlow(user);
}

Console.WriteLine("\nThank you! Goodbye!");

// registation part

User AuthenticationFlow()
{
    Console.WriteLine("========================================");
    Console.WriteLine("   Welcome to Authentication System     ");
    Console.WriteLine("========================================\n");

    try
    {
        // Get username
        Console.Write("Enter Username: ");
        string? username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine(" Username cannot be empty!");
            return null;
        }

        // Get email
        Console.Write(" Enter Email: ");
        string? email = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine(" Email cannot be empty!");
            return null;
        }

        // Get password
        Console.Write("Enter Password (min 8 chars, uppercase, lowercase, digit): ");
        string? password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine(" Password cannot be empty!");
            return null;
        }

        // Create user and register
        var user = new User(username, email);

        try
        {
            user.Register(password);
            Console.WriteLine("\n Registration Successful! \n");
            return user;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine("\n Registration Failed: Message.... \n");
            return null;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($" Error: {ex.Message}");
        return null;
    }
}


//login part
void LoginFlow(User registeredUser)
{
    Console.WriteLine("========================================");
    Console.WriteLine("        Login to Your Account           ");
    Console.WriteLine("========================================\n");

    int attempts = 0;
    bool loginSuccess = false;

    while (attempts < 3 && !loginSuccess)
    {
        try
        {
            Console.Write("Enter Password to Login: ");
            string? password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password cannot be empty!");
                attempts++;
                continue;
            }

            // Try to login
            if (registeredUser.Login(password))
            {
                Console.WriteLine("\n Login Successful!\n");
                Console.WriteLine("Welcome back, {registeredUser.Username}!");
                loginSuccess = true;
            }
            else
            {
                attempts++;
                int remaining = 3 - attempts;
                if (remaining > 0)
                {
                    Console.WriteLine(" Invalid password. \n");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(" Error: Message... ");
            attempts++;
        }
    }

    if (!loginSuccess)
    {
        Console.WriteLine("Too many failed attempts. Access denied.");
    }
}