using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

[TestFixture]
public class RegistrationManagerTest
{
    private RegistrationManager registrationManager;

    [SetUp]
    public void SetUp()
    {
        // Створюємо екземпляр RegistrationManager перед кожним тестом
        registrationManager = new RegistrationManager();
    }

    [Test]
    public void RegistrationDataValidation_AllFieldsFilledCorrectly_ReturnsTrue()
    {
        string validUsername = "validUsername";
        string validPassword = "validPassword123";
        string validEmail = "test@example.com";

        // Act
        bool result = registrationManager.IsRegisterDataCorrect(validUsername, validPassword, validEmail);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void RegistrationDataValidation_EmptyUsername_ReturnsFalse()
    {
        string invalidUsername = "";
        string validPassword = "validPassword123";
        string validEmail = "test@example.com";

        // Act
        bool result = registrationManager.IsRegisterDataCorrect(invalidUsername, validPassword, validEmail);


        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void RegistrationDataValidation_InvalidEmailFormat_ReturnsFalse()
    {
        string validUsername = "validUsername";
        string validPassword = "validPassword123";
        string invalidEmail = "invalidemail";
        // Act
        bool result = registrationManager.IsRegisterDataCorrect(validUsername, validPassword, invalidEmail);
        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void RegistrationDataValidation_ShortPassword_ReturnsFalse()
    {
        string validUsername = "validUsername";
        string shortPassword = "short";
        string validEmail = "test@example.com";

        // Act
        bool result = registrationManager.IsRegisterDataCorrect(validUsername, shortPassword, validEmail);

        // Assert
        Assert.IsFalse(result);
    }
}
