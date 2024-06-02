using NUnit.Framework;

[TestFixture]
public class LoginTests
{
    private LogInManager logInManager;

    [SetUp]
    public void SetUp()
    {
        // Створюємо екземпляр LogInManager перед кожним тестом
        logInManager = new LogInManager();
    }

    [Test]
    public void IsLoginDataCorrect_EmptyLogin_ReturnsFalse()
    {
        // Arrange
        string login = "";
        string password = "password";
        string pcIdent = "pcIdentifier";

        // Act
        bool result = logInManager.IsLogInDataCorrect(login, password, pcIdent);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void IsLoginDataCorrect_EmptyPassword_ReturnsFalse()
    {
        // Arrange
        string login = "username";
        string password = "";
        string pcIdent = "pcIdentifier";

        // Act
        bool result = logInManager.IsLogInDataCorrect(login, password, pcIdent);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void IsLoginDataCorrect_InvalidPasswordFormat_ReturnsFalse()
    {
        // Arrange
        string login = "username";
        string password = "pass"; // Invalid password format
        string pcIdent = "pcIdentifier";

        // Act
        bool result = logInManager.IsLogInDataCorrect(login, password, pcIdent);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void IsLoginDataCorrect_NullPcIdent_ReturnsFalse()
    {
        // Arrange
        string login = "username";
        string password = "password";
        string pcIdent = null;

        // Act
        bool result = logInManager.IsLogInDataCorrect(login, password, pcIdent);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void IsLoginDataCorrect_ValidData_ReturnsTrue()
    {
        // Arrange
        string login = "username";
        string password = "password";
        string pcIdent = "pcIdentifier";

        // Act
        bool result = logInManager.IsLogInDataCorrect(login, password, pcIdent);

        // Assert
        Assert.IsTrue(result);
    }
}
