# Testing-Tool-Evaluation
Testing Tool Evaluation - N unit testing tool
## Overview
This project demonstrates **NUnit testing framework** using a practical User Authentication system with registration, login validation, email validation, and password strength checking.

## Key NUnit Features Demonstrated

[SetUp] Attribute — Initialize test data before each test  
[TestCase] Parameterized Tests — Run same test with different inputs  
[TestFixture] — Test class organization  
Exception Testing — Verify correct exceptions are thrown  
Data-Driven Testing — Multiple scenarios in one test  

## Project Structure

Solution Explorer:
┌─ User_Authentication (Solution)
└─├─ AuthenticationDemo (Main Project)
   │  ├─ User.cs
   │  ├─ Program.cs
   │  └─ AuthenticationDemo.csproj
   │
   └─ AuthenticationDemo.Tests (Test Project)
      ├─ UserAuthenticationTests.cs
      └─ AuthenticationDemo.Tests.csproj


## How to Run

** I use visual studio for run this **

## Keyboard Shortcuts for Visual Studio

| Shortcut               | What It Does            |
|------------------------|-------------------------|
| **Ctrl + B**           | Build project           |
| **Ctrl + Shift + B**   | Clean and build         |
| **Ctrl + E, T**        | Open Test Explorer      |
| **F5**                 | Run (with debugging)    |
| **Ctrl + F5**          | Run (without debugging) |
| **Ctrl + `**           | Open Terminal           |
| **Ctrl + ,**           | Open Settings           |
| **Ctrl + K, Ctrl + D** | Format code             |

### Prerequisites
- .NET 8.0 SDK or higher installed
- NUnit and test adapters will be installed automatically

### Setup & Execution

1. **Navigate to project directory:**
     bash
   cd User_Authentication
   

2. **Restore NuGet packages:**(if need)
     bash
   dotnet restore
   

3. **Run all tests:**
      bash
   dotnet test
   

4. **Run tests with verbose output:**
     bash
   dotnet test --verbosity detailed
   

5. **Run specific test class:**
     bash
   dotnet test --filter "ClassName=UserAuthenticationTests"
   


## What We Have (Unit Testing):

  User.cs                         
  ├─ Register()              
  ├─ Login()                 
  ├─ IsValidPassword()       
  └─ IsValidEmail()          
