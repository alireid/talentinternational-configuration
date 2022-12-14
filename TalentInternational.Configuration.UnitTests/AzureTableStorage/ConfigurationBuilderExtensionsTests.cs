using System;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using TalentInternational.Configuration.AzureTableStorage;
using TalentInternational.Testing;

namespace TalentInternational.Configuration.UnitTests.AzureTableStorage
{
    [TestFixture]
    public class ConfigurationBuilderExtensionsTests : FluentTest<ConfigurationBuilderExtensionsTestsFixture>
    {
        [Test]
        public void AddAzureTableStorage_WhenSetupOptionsIsInvoked_ThenOptionsContainDefaults()
        {
            Test(f => f.StoreCallbackOptions(), f => f.AddAzureTableStorageWithOptions(),
                f => f.AssertOptionsContainDefaults());
        }

        [Test]
        public void AddAzureTableStorage_WhenDefaultEnvironmentNameOptionsAreUsedAndDefaultEnvironmentVariableIsPresent_ThenAddCalledWithConfigurationSourceWithEnvironmentNameFromDefaultEnvironmentVariable()
        {
            Test(f => f.ArrangeDefaultEnvironmentNameVariable(), f => f.AddAzureTableStorageWithOptions(),
                f => f.VerifyAddCalledWithConfigurationSourceWithCorrectEnvironmentName());
        }

        [Test]
        public void AddAzureTableStorage_WhenDefaultEnvironmentNameOptionsAreUsedAndNoDefaultEnvironmentVariableIsPresent_ThenAddCalledWithConfigurationSourceWithDefaultEnvironmentName()
        {
            Test(f => f.ArrangeNoDefaultEnvironmentNameVariable(), f => f.AddAzureTableStorageWithOptions(),
                    f => f.VerifyAddCalledWithConfigurationSourceWithCorrectEnvironmentName());
        }

        [Test]
        public void AddAzureTableStorage_WhenDefaultConnectionStringOptionsAreUsedAndDefaultEnvironmentVariableIsPresent_ThenAddCalledWithConfigurationSourceWithConnectionStringFromDefaultEnvironmentVariable()
        {
            Test(f => f.ArrangeDefaultConnectionStringVariable(), f => f.AddAzureTableStorageWithOptions(),
                f => f.VerifyAddCalledWithConfigurationSourceWithCorrectConnectionString());
        }

        [Test]
        public void AddAzureTableStorage_WhenDefaultConnectionStringOptionsAreUsedAndNoDefaultEnvironmentVariableIsPresentAndEnvironmentIsLocal_ThenAddCalledWithConfigurationSourceWithDefaultConnectionString()
        {
            Test(f => f.ArrangeNoDefaultConnectionStringVariable().ArrangeEnvironmentIsLocal(), f => f.AddAzureTableStorageWithOptions(),
                f => f.VerifyAddCalledWithConfigurationSourceWithCorrectConnectionString());
        }

        [Test]
        public void AddAzureTableStorage_WhenDefaultConnectionStringOptionsAreUsedAndNoDefaultEnvironmentVariableIsPresentAndEnvironmentIsNotLocal_ThenExceptionIsThrown()
        {
            TestException(f => f.ArrangeNoDefaultConnectionStringVariable().ArrangeEnvironmentIsNotLocal(), f => f.AddAzureTableStorageWithOptions(),
                (f, r) => r.Should().Throw<Exception>());
        }

        [Test]
        public void AddAzureTableStorage_WhenOptionsAreSuppliedWithEnvironmentNameEnvironmentVariableNameAndNoEnvironmentName_ThenAddCalledWithConfigurationSourceWithEnvironmentNameFromGivenEnvironmentVariable()
        {
            Test(f => f.ArrangeOptionsAreSuppliedWithEnvironmentNameEnvironmentVariableNameAndNoEnvironmentName(),
                f => f.AddAzureTableStorageWithOptions(),
                f => f.VerifyAddCalledWithConfigurationSourceWithCorrectEnvironmentName());
        }

        [Test]
        public void AddAzureTableStorage_WhenOptionsAreSuppliedWithEnvironmentNameAndNoEnvironmentNameEnvironmentVariableName_ThenAddCalledWithConfigurationSourceWithSuppliedEnvironmentName()
        {
            Test(f => f.ArrangeOptionsAreSuppliedWithEnvironmentNameAndNoEnvironmentNameEnvironmentVariableName(),
                f => f.AddAzureTableStorageWithOptions(),
                f => f.VerifyAddCalledWithConfigurationSourceWithCorrectEnvironmentName());
        }

        [Test]
        public void AddAzureTableStorage_WhenOptionsAreSuppliedWithEnvironmentNameAndEnvironmentNameEnvironmentVariableName_ThenAddCalledWithConfigurationSourceWithSuppliedEnvironmentName()
        {
            Test(f => f.ArrangeOptionsAreSuppliedWithEnvironmentNameAndEnvironmentNameEnvironmentVariableName(),
                f => f.AddAzureTableStorageWithOptions(),
                f => f.VerifyAddCalledWithConfigurationSourceWithCorrectEnvironmentName());
        }
        
        [Test]
        public void AddAzureTableStorage_WhenOptionsAreSuppliedWithStorageConnectionStringEnvironmentVariableNameAndNoStorageConnectionString_ThenAddCalledWithConfigurationSourceWithStorageConnectionStringFromGivenEnvironmentVariable()
        {
            Test(f => f.ArrangeOptionsAreSuppliedWithStorageConnectionStringEnvironmentVariableNameAndNoStorageConnectionString(),
                f => f.AddAzureTableStorageWithOptions(),
                f => f.VerifyAddCalledWithConfigurationSourceWithCorrectConnectionString());
        }
        
        [Test]
        public void AddAzureTableStorage_WhenOptionsAreSuppliedWithStorageConnectionStringAndNoStorageConnectionStringEnvironmentVariableName_ThenAddCalledWithConfigurationSourceWithSuppliedStorageConnectionString()
        {
            Test(f => f.ArrangeOptionsAreSuppliedWithStorageConnectionStringAndNoStorageConnectionStringEnvironmentVariableName(),
                f => f.AddAzureTableStorageWithOptions(),
                f => f.VerifyAddCalledWithConfigurationSourceWithCorrectConnectionString());
        }

        [Test]
        public void AddAzureTableStorage_WhenOptionsAreSuppliedWithStorageConnectionStringAndStorageConnectionStringEnvironmentVariableName_ThenAddCalledWithConfigurationSourceWithSuppliedStorageConnectionString()
        {
            Test(f => f.ArrangeOptionsAreSuppliedWithStorageConnectionStringAndStorageConnectionStringEnvironmentVariableName(),
                f => f.AddAzureTableStorageWithOptions(),
                f => f.VerifyAddCalledWithConfigurationSourceWithCorrectConnectionString());
        }
    }

    public class ConfigurationBuilderExtensionsTestsFixture
    {
        public Mock<IConfigurationBuilder> ConfigurationBuilder { get; set; }
        public Action<StorageOptions> SetupOptions { get; set; }
        public StorageOptions StoredCallbackOptions { get; set; }
        public string[] ConfigurationKeys { get; set; }
        public string ExpectedEnvironmentName { get; set; }
        public const string DirectlySuppliedEnvironmentName = nameof(DirectlySuppliedEnvironmentName);
        public const string EnvironmentNameFromVariable = nameof(EnvironmentNameFromVariable);
        public const string OptionSuppliedEnvironmentNameEnvironmentVariableName = nameof(OptionSuppliedEnvironmentNameEnvironmentVariableName);
        public const string EnvironmentNameFromDefaultEnvironmentVariable = nameof(EnvironmentNameFromDefaultEnvironmentVariable);
        public const string DefaultEnvironmentNameEnvironmentVariableName = "APPSETTING_EnvironmentName";
        
        public string ExpectedConnectionString { get; set; }
        public const string DirectlySuppliedConnectionString = nameof(DirectlySuppliedConnectionString);
        public const string ConnectionStringFromVariable = nameof(ConnectionStringFromVariable);
        public const string OptionSuppliedConnectionStringEnvironmentVariableName = nameof(OptionSuppliedConnectionStringEnvironmentVariableName);
        public const string ConnectionStringFromDefaultEnvironmentVariable = nameof(ConnectionStringFromDefaultEnvironmentVariable);
        public const string DefaultConnectionStringEnvironmentVariableName = "APPSETTING_ConfigurationStorageConnectionString";
        
        public ConfigurationBuilderExtensionsTestsFixture()
        {
            ConfigurationBuilder = new Mock<IConfigurationBuilder>();
            ConfigurationKeys = new[] {"Key"};
            SetupOptions = so => so.ConfigurationKeys = ConfigurationKeys;

            // clear defaults, which are likely to be set on the developer's machine
            Environment.SetEnvironmentVariable("APPSETTING_EnvironmentName", EnvironmentNameFromVariable);
            Environment.SetEnvironmentVariable("APPSETTING_ConfigurationStorageConnectionString", ConnectionStringFromVariable);
        }

        public ConfigurationBuilderExtensionsTestsFixture StoreCallbackOptions()
        {
            SetupOptions = so =>
            {
                StoredCallbackOptions = so.Clone();
                so.ConfigurationKeys = ConfigurationKeys;
            };
            return this;
        }

        public ConfigurationBuilderExtensionsTestsFixture ArrangeDefaultEnvironmentNameVariable()
        {
            ExpectedEnvironmentName = EnvironmentNameFromDefaultEnvironmentVariable;
            Environment.SetEnvironmentVariable(DefaultEnvironmentNameEnvironmentVariableName, ExpectedEnvironmentName);
            return this;
        }

        public ConfigurationBuilderExtensionsTestsFixture ArrangeNoDefaultEnvironmentNameVariable()
        {
            ExpectedEnvironmentName = "LOCAL";
            Environment.SetEnvironmentVariable(DefaultEnvironmentNameEnvironmentVariableName, null);
            return this;
        }

        public ConfigurationBuilderExtensionsTestsFixture ArrangeEnvironmentIsLocal()
        {
            SetupOptions = so =>
            {
                so.ConfigurationKeys = ConfigurationKeys;
                so.EnvironmentName = "LOCAL";
            };
            return this;
        }

        public ConfigurationBuilderExtensionsTestsFixture ArrangeEnvironmentIsNotLocal()
        {
            SetupOptions = so =>
            {
                so.ConfigurationKeys = ConfigurationKeys;
                so.EnvironmentName = "NOTLOCAL";
            };
            return this;
        }

        public ConfigurationBuilderExtensionsTestsFixture ArrangeDefaultConnectionStringVariable()
        {
            ExpectedConnectionString = ConnectionStringFromDefaultEnvironmentVariable;
            Environment.SetEnvironmentVariable(DefaultConnectionStringEnvironmentVariableName, ExpectedConnectionString);
            return this;
        }

        public ConfigurationBuilderExtensionsTestsFixture ArrangeNoDefaultConnectionStringVariable()
        {
            ExpectedConnectionString = "UseDevelopmentStorage=true";
            Environment.SetEnvironmentVariable(DefaultConnectionStringEnvironmentVariableName, null);
            return this;
        }

        public ConfigurationBuilderExtensionsTestsFixture ArrangeOptionsAreSuppliedWithEnvironmentNameEnvironmentVariableNameAndNoEnvironmentName()
        {
            SetupOptions = so =>
            {
                so.ConfigurationKeys = ConfigurationKeys;
                so.EnvironmentNameEnvironmentVariableName = OptionSuppliedEnvironmentNameEnvironmentVariableName;
                so.EnvironmentName = null;
            };

            ExpectedEnvironmentName = "EnvironmentNameFromOptionSuppliedVariableName";
            Environment.SetEnvironmentVariable(OptionSuppliedEnvironmentNameEnvironmentVariableName, ExpectedEnvironmentName);
            
            return this;
        }

        public ConfigurationBuilderExtensionsTestsFixture ArrangeOptionsAreSuppliedWithEnvironmentNameAndNoEnvironmentNameEnvironmentVariableName()
        {
            SetupOptions = so =>
            {
                so.ConfigurationKeys = ConfigurationKeys;
                so.EnvironmentNameEnvironmentVariableName = null;
                so.EnvironmentName = ExpectedEnvironmentName = DirectlySuppliedEnvironmentName;
            };

            return this;
        }

        public ConfigurationBuilderExtensionsTestsFixture ArrangeOptionsAreSuppliedWithEnvironmentNameAndEnvironmentNameEnvironmentVariableName()
        {
            SetupOptions = so =>
            {
                so.ConfigurationKeys = ConfigurationKeys;
                so.EnvironmentNameEnvironmentVariableName = OptionSuppliedEnvironmentNameEnvironmentVariableName;
                so.EnvironmentName = ExpectedEnvironmentName = DirectlySuppliedEnvironmentName;
            };

            ExpectedEnvironmentName = "EnvironmentNameFromOptionSuppliedVariableName";
            Environment.SetEnvironmentVariable(OptionSuppliedEnvironmentNameEnvironmentVariableName, ExpectedEnvironmentName);
            
            return this;
        }
        
        public ConfigurationBuilderExtensionsTestsFixture ArrangeOptionsAreSuppliedWithStorageConnectionStringEnvironmentVariableNameAndNoStorageConnectionString()
        {
            const string optionSuppliedStorageConnectionStringEnvironmentVariableName
                = nameof(optionSuppliedStorageConnectionStringEnvironmentVariableName);
            
            SetupOptions = so =>
            {
                so.ConfigurationKeys = ConfigurationKeys;
                so.StorageConnectionStringEnvironmentVariableName = optionSuppliedStorageConnectionStringEnvironmentVariableName;
                so.StorageConnectionString = null;
            };

            ExpectedConnectionString = "ConnectionStringFromOptionSuppliedVariableName";
            Environment.SetEnvironmentVariable(optionSuppliedStorageConnectionStringEnvironmentVariableName, ExpectedConnectionString);
            
            return this;
        }
        
        public ConfigurationBuilderExtensionsTestsFixture ArrangeOptionsAreSuppliedWithStorageConnectionStringAndNoStorageConnectionStringEnvironmentVariableName()
        {
            SetupOptions = so =>
            {
                so.ConfigurationKeys = ConfigurationKeys;
                so.StorageConnectionStringEnvironmentVariableName = null;
                so.StorageConnectionString = ExpectedConnectionString = DirectlySuppliedConnectionString;
            };

            return this;
        }

        public ConfigurationBuilderExtensionsTestsFixture ArrangeOptionsAreSuppliedWithStorageConnectionStringAndStorageConnectionStringEnvironmentVariableName()
        {
            SetupOptions = so =>
            {
                so.ConfigurationKeys = ConfigurationKeys;
                so.StorageConnectionStringEnvironmentVariableName = OptionSuppliedConnectionStringEnvironmentVariableName;
                so.StorageConnectionString = ExpectedConnectionString = DirectlySuppliedConnectionString;
            };

            ExpectedEnvironmentName = "EnvironmentNameFromOptionSuppliedVariableName";
            Environment.SetEnvironmentVariable(OptionSuppliedEnvironmentNameEnvironmentVariableName, ExpectedEnvironmentName);
            
            return this;
        }

        public void AddAzureTableStorageWithOptions()
        {
            ConfigurationBuilder.Object.AddAzureTableStorage(SetupOptions);
        }

        public void AssertOptionsContainDefaults()
        {
            StoredCallbackOptions.Should().BeEquivalentTo(new StorageOptions
            {
                EnvironmentNameEnvironmentVariableName = "APPSETTING_EnvironmentName",
                StorageConnectionStringEnvironmentVariableName = "APPSETTING_ConfigurationStorageConnectionString",
                PreFixConfigurationKeys = true
            });
        }
        
        public void VerifyAddCalledWithConfigurationSourceWithCorrectEnvironmentName()
        {
            ConfigurationBuilder.Verify(cb => cb.Add(It.Is<AzureTableStorageConfigurationSource>( 
                s => s.EnvironmentName == ExpectedEnvironmentName)));
        }
        
        public void VerifyAddCalledWithConfigurationSourceWithCorrectConnectionString()
        {
            ConfigurationBuilder.Verify(cb => cb.Add(It.Is<AzureTableStorageConfigurationSource>( 
                s => s.ConnectionString == ExpectedConnectionString)));
        }
    }
}