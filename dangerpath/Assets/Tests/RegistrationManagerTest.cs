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

    /*[Test]
    public IEnumerator CheckAccountExist_AccountExists_ReturnsErrorMessage()
    {
        Debug.Log("111");
        string jsonData = "{\"username\":\"existing_user\",\"password\":\"existingpassword\",\"email\":\"existing@example.com\"}";
        string expectedErrorMessage = "This Email or Username already in use!";

        Debug.Log("1");

        // Act
        registrationManager.CheckAccountExist(jsonData);
      
        Debug.Log("2");
        // Assert
        Assert.AreEqual(expectedErrorMessage, registrationManager.GetLastErrorMessage());
        
    }

    [Test]
    public void CheckAccountExist_AccountDoesNotExist_ReturnsNoError()
    {
        string jsonData = "{\"username\":\"new_user\",\"password\":\"password123\",\"email\":\"new@example.com\"}";
        string expectedErrorMessage = "Verify your email address!";

         //Act
        registrationManager.CheckAccountExist(jsonData);

         //Assert
        Assert.AreEqual(expectedErrorMessage, registrationManager.GetLastErrorMessage());
    }*/
}
