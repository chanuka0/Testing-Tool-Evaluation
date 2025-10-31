using NUnit.Framework;
using System;
using AuthenticationSystem;

namespace AuthenticationSystem.Tests
{
    [TestFixture]
    public class UserAuthenticationTests
    {
        private User _user;

        [SetUp]
        public void Setup()
        {
            _user = new User("john_doe", "john@example.com");
        }

        //registation test

        [Test]
        public void Register_WithValidCredentials_ShouldSucceed()
        {
            var user = new User("alice_smith", "alice@example.com");
            bool result = user.Register("SecurePass123");
            Assert.That(result, Is.True, "Registration should succeed with valid credentials");
        }

        [Test]
        public void Register_WithValidPassword_ShouldSucceed()
        {
            bool result = _user.Register("SecurePass123");
            Assert.That(result, Is.True, "Registration should succeed with valid password");
        }

        [Test]
        public void Register_WithEmptyPassword_ShouldThrowException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _user.Register(""));
            Assert.That(ex.Message, Does.Contain("empty"), "Should mention empty password");
        }

        [Test]
        public void Register_WithNullPassword_ShouldThrowException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _user.Register(null));
            Assert.That(ex.Message, Does.Contain("empty"), "Should mention empty password");
        }

        [Test]
        public void Register_WithWhitespacePassword_ShouldThrowException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _user.Register("   "));
            Assert.That(ex.Message, Does.Contain("empty"), "Should mention empty password");
        }

        //password validation (Parameterized Tests)
        [TestCase("short")]
        [TestCase("hfgpgjehvsf123")]
        [TestCase("KHQOEERCHTE123")]
        [TestCase("Notnumbers")]
        public void Register_WithInvalidPassword_ShouldThrowException(string invalidPassword)
        {
            var ex = Assert.Throws<ArgumentException>(() => _user.Register(invalidPassword));
            Assert.That(ex.Message, Does.Contain("Password must be"));
        }

        // password validaiton
        [TestCase("StrongPass123", true)]
        [TestCase("ValidPassword456", true)]
        [TestCase("MySecurePass789", true)]
        [TestCase("weak", false)]
        [TestCase("NoDigits", false)]
        [TestCase("onlydigits123", false)]
        [TestCase("1234567", false)]
        public void IsValidPassword_WithVariousPasswords_ReturnsCorrectResult(string password, bool expected)
        {
            bool result = User.IsValidPassword(password);
            string validText = expected ? "valid" : "invalid";
            Assert.That(result, Is.EqualTo(expected), $"Password '{password}' should be {validText}");
        }

        // email validaiton (Parameterized Tests)
        [TestCase("user@example.com", true)]
        [TestCase("john.doe@company.co.uk", true)]
        [TestCase("test+tag@domain.com", true)]
        [TestCase("first.last@subdomain.example.org", true)]
        [TestCase("simple@localhost.com", true)]
        [TestCase("invalidemail.com", false)]
        [TestCase("missing@domain", false)]
        [TestCase("@nodomain.com", false)]
        [TestCase("space in@email.com", false)]
        [TestCase("double@@email.com", false)]
        public void IsValidEmail_WithVariousFormats_ReturnsCorrectResult(string email, bool expected)
        {
            bool result = User.IsValidEmail(email);
            Assert.That(result, Is.EqualTo(expected), $"Email '{email}' validation should return {expected}");
        }

        
        //login test
        [Test]
        public void Login_WithCorrectPassword_ShouldReturnTrue()
        {
            string password = "SecurePass123";
            _user.Register(password);
            bool result = _user.Login(password);
            Assert.That(result, Is.True, "Login should succeed with correct password");
        }

        [Test]
        public void Login_WithIncorrectPassword_ShouldReturnFalse()
        {
            _user.Register("SecurePass123");
            bool result = _user.Login("WrongPassword123");
            Assert.That(result, Is.False, "Login should fail with incorrect password");
        }

        [Test]
        public void Login_WithEmptyPassword_ShouldThrowException()
        {
            _user.Register("SecurePass123");
            Assert.Throws<ArgumentException>(() => _user.Login(""));
        }

        [Test]
        public void Login_WithNullPassword_ShouldThrowException()
        {
            _user.Register("SecurePass123");
            Assert.Throws<ArgumentException>(() => _user.Login(null));
        }

        [Test]
        public void Login_BeforeRegistration_ShouldThrowException()
        {
            var unregisteredUser = new User("new_user", "new@example.com");
            var ex = Assert.Throws<InvalidOperationException>(() => unregisteredUser.Login("AnyPass123"));
            Assert.That(ex.Message, Does.Contain("not registered"));
        }

//multiple login test
        [Test]
        public void Login_MultipleWrongAttempts_EachReturnsFalse()
        {
            string correctPassword = "SecurePass123";
            _user.Register(correctPassword);
            Assert.That(_user.Login("WrongAttempt1"), Is.False, "First attempt should fail");
            Assert.That(_user.Login("WrongAttempt2"), Is.False, "Second attempt should fail");
            Assert.That(_user.Login("WrongAttempt3"), Is.False, "Third attempt should fail");
        }

        [Test]
        public void Login_CorrectPasswordAfterWrongAttempts_ShouldSucceed()
        {
            string correctPassword = "SecurePass123";
            _user.Register(correctPassword);
            _user.Login("Wrong1");
            _user.Login("Wrong2");
            bool result = _user.Login(correctPassword);
            Assert.That(result, Is.True, "Should succeed with correct password after failed attempts");
        }

        //integration test
        [Test]
        public void FullAuthenticationFlow_RegisterAndLogin_ShouldSucceed()
        {
            string password = "ValidPass123";
            bool registerResult = _user.Register(password);
            bool loginResult = _user.Login(password);
            Assert.That(registerResult, Is.True, "Registration should succeed");
            Assert.That(loginResult, Is.True, "Login should succeed after registration");
        }

        [Test]
        public void FullAuthenticationFlow_RegisterWithMultipleUsers_EachIndependent()
        {
            var user1 = new User("alice", "alice@test.com");
            var user2 = new User("bob", "bob@test.com");
            user1.Register("AlicePass123");
            user2.Register("BobPassword456");

            Assert.That(user1.Login("AlicePass123"), Is.True, "Alice should login with her password");
            Assert.That(user1.Login("BobPassword456"), Is.False, "Alice should not login with Bob's password");
            Assert.That(user2.Login("BobPassword456"), Is.True, "Bob should login with his password");
            Assert.That(user2.Login("AlicePass123"), Is.False, "Bob should not login with Alice's password");
        }

        [Test]
        public void FullAuthenticationFlow_RegistrationWithVariousValidPasswords_AllSucceed()
        {
            var testCases = new[]
            {
                new { user = new User("user1", "user1@test.com"), password = "SecurePass123" },
                new { user = new User("user2", "user2@test.com"), password = "ValidPassword456" },
                new { user = new User("user3", "user3@test.com"), password = "MyPass789ABC" }
            };

            foreach (var testCase in testCases)
            {
                bool registerResult = testCase.user.Register(testCase.password);
                Assert.That(registerResult, Is.True, $"Registration should succeed for {testCase.user.Username}");
                bool loginResult = testCase.user.Login(testCase.password);
                Assert.That(loginResult, Is.True, $"Login should succeed for {testCase.user.Username}");
            }
        }

        //usser datils test
        [Test]
        public void User_Properties_CanBeSet()
        {
            var user = new User("john", "john@test.com");
            var username = user.Username;
            var email = user.Email;
            Assert.That(username, Is.EqualTo("john"), "Username should be set correctly");
            Assert.That(email, Is.EqualTo("john@test.com"), "Email should be set correctly");
        }

        [Test]
        public void User_MultipleRegistrations_SecondShouldOverwrite()
        {
            string password1 = "FirstPassword123";
            string password2 = "SecondPassword456";
            _user.Register(password1);
            _user.Register(password2);

            Assert.That(_user.Login(password1), Is.False, "Should not login with old password");
            Assert.That(_user.Login(password2), Is.True, "Should login with new password");
        }

        //edge cases
        [Test]
        public void Register_WithCaseSensitivePassword_BothCasesRequired()
        {
            var user = new User("testuser", "test@example.com");
            Assert.Throws<ArgumentException>(() => user.Register("alllowercase123"));
            Assert.Throws<ArgumentException>(() => user.Register("ALLUPPERCASE123"));
        }

        [Test]
        public void Login_PasswordIsCaseSensitive()
        {
            string password = "SecurePass123";
            _user.Register(password);
            Assert.That(_user.Login("securePass123"), Is.False, "Password should be case-sensitive");
            Assert.That(_user.Login("SECUREPASS123"), Is.False, "Password should be case-sensitive");
            Assert.That(_user.Login(password), Is.True, "Exact case should work");
        }

        [Test]
        public void Register_WithLongPassword_ShouldSucceed()
        {
            var user = new User("longuser", "long@test.com");
            string longPassword = "ThisIsAVeryLongPasswordWith123Numbers";
            bool result = user.Register(longPassword);
            Assert.That(result, Is.True, "Should accept very long passwords");
            Assert.That(user.Login(longPassword), Is.True, "Should be able to login with long password");
        }

        //error msg echeck
        [Test]
        public void Register_InvalidPassword_ErrorMessageIsDescriptive()
        {
            var user = new User("testuser", "test@test.com");
            var ex = Assert.Throws<ArgumentException>(() => user.Register("weakpass"));
            Assert.That(ex.Message, Does.Contain("8 characters"), "Should mention minimum length");
            Assert.That(ex.Message, Does.Contain("uppercase"), "Should mention uppercase requirement");
            Assert.That(ex.Message, Does.Contain("lowercase"), "Should mention lowercase requirement");
            Assert.That(ex.Message, Does.Contain("number"), "Should mention number requirement");
        }

        [Test]
        public void Register_InvalidEmail_ErrorMessageIsDescriptive()
        {
            var user = new User("testuser", "notanemail");
            var ex = Assert.Throws<ArgumentException>(() => user.Register("ValidPass123"));
            Assert.That(ex.Message, Does.Contain("email"), "Error should mention email");
        }

        [Test]
        public void Login_BeforeRegistration_ErrorMessageIsClear()
        {
            var unregisteredUser = new User("newuser", "new@test.com");
            var ex = Assert.Throws<InvalidOperationException>(() => unregisteredUser.Login("AnyPass123"));
            Assert.That(ex.Message, Does.Contain("not registered"));
        }
    }
}