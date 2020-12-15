using Driv.CustomApis.API;
using FakeXrmEasy;
using FakeXrmEasy.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Driv.CustomApis.Tests.API
{
    [TestClass]
    public class GetEnvironnementVariableTest
    {
        [TestMethod]
        public void WhenKeyNotExists()
        {

            #region ARRANGE
            var fakedContext = new XrmFakedContext();


            
            var environmentvariabledef = new EnvironmentVariableDefinition()
            {
                Id = Guid.NewGuid(),
                SchemaName = "MyKey",
                ValueSchema = "StringValue",
                Type = EnvironmentVariableDefinition_Type.String

            };

            fakedContext.Initialize(new List<Entity>() {
                environmentvariabledef
            });

            var inputparameters = new ParameterCollection
            {
                { "Key", "MyNonExistingKey" }
            };


            var outputparameters = new ParameterCollection
            {
                { "Exists", null },
                { "ValueString", null },
                { "ValueBool", null },
                { "ValueDecimal", null },
                { "Type", null },
                { "TypeName", null }
            };
            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_GetEnvironmentVariable",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            fakedContext.ExecutePluginWith<GetEnvironmentVariable>(context);
            #endregion


            #region ASSERT
            Assert.IsFalse((bool)context.OutputParameters["Exists"]);
            #endregion
        }


        [TestMethod]
        public void WhenKeyExistsNoOverride()
        {
            var fakedContext = new XrmFakedContext();

            #region data
            var environmentvariabledef = new EnvironmentVariableDefinition()
            {
                Id = Guid.NewGuid(),
                SchemaName = "MyKey",
                DefaultValue = "MyKeyValue",
                Type = EnvironmentVariableDefinition_Type.String

            };

            fakedContext.Initialize(new List<Entity>() {
                environmentvariabledef
            });
            fakedContext.InitializeMetadata(Assembly.GetAssembly(typeof(EnvironmentVariableDefinition)));
            #endregion


            var inputparameters = new ParameterCollection
            {
                { "Key", "MyKey" }
            };


            var outputparameters = new ParameterCollection
            {
                { "Exists", null },
                { "ValueString", null },
                { "ValueBool", null },
                { "ValueDecimal", null },
                { "Type", null },
                { "TypeName", null }
            };


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_GetEnvironmentVariable",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            fakedContext.ExecutePluginWith<GetEnvironmentVariable>(context);
            #endregion



            Assert.IsTrue((bool)context.OutputParameters["Exists"]);
            Assert.AreEqual("MyKeyValue",(string)context.OutputParameters["ValueString"]);
            Assert.AreEqual(new OptionSetValue((int)EnvironmentVariableDefinition_Type.String), (OptionSetValue)context.OutputParameters["Type"]);
        }

        [TestMethod]
        public void WhenKeyExistsAndOverriden()
        {
            var fakedContext = new XrmFakedContext();


            #region data
            var environmentvariabledef = new EnvironmentVariableDefinition()
            {
                Id = Guid.NewGuid(),
                SchemaName = "MyKey",
                DefaultValue = "MyKeyValue",
                Type = EnvironmentVariableDefinition_Type.String

            };
            var environmentvariblevalue = new EnvironmentVariableValue()
            {
                Id = Guid.NewGuid(),
                EnvironmentVariableDefinitionId = environmentvariabledef.ToEntityReference(),
                Value = "MyOverridenValue"

            };

            fakedContext.Initialize(new List<Entity>() {
                environmentvariabledef,
                environmentvariblevalue
            });
            fakedContext.InitializeMetadata(Assembly.GetAssembly(typeof(EnvironmentVariableDefinition)));

            #endregion






            var inputparameters = new ParameterCollection
            {
                { "Key", "MyKey" }
            };


            var outputparameters = new ParameterCollection
            {
                { "Exists", null },
                { "ValueString", null },
                { "ValueBool", null },
                { "ValueDecimal", null },
                { "Type", null },
                { "TypeName", null }
            };



            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_GetEnvironmentVariable",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            fakedContext.ExecutePluginWith<GetEnvironmentVariable>(context);
            #endregion



            Assert.IsTrue((bool)context.OutputParameters["Exists"]);
            Assert.AreEqual("MyOverridenValue", (string)context.OutputParameters["ValueString"]);
            Assert.AreEqual(new OptionSetValue((int)EnvironmentVariableDefinition_Type.String), (OptionSetValue)context.OutputParameters["Type"]);
        }

        [TestMethod]
        public void WhenKeyExistsAndTypeIsBool()
        {
            var fakedContext = new XrmFakedContext();


            #region data
            var environmentvariabledef = new EnvironmentVariableDefinition()
            {
                Id = Guid.NewGuid(),
                SchemaName = "MyKey",
                DefaultValue = "no", //false
                Type = EnvironmentVariableDefinition_Type.Boolean

            };
            var environmentvariblevalue = new EnvironmentVariableValue()
            {
                Id = Guid.NewGuid(),
                EnvironmentVariableDefinitionId = environmentvariabledef.ToEntityReference(),
                Value = "yes" //overriden to true

            };

            fakedContext.Initialize(new List<Entity>() {
                environmentvariabledef,
                environmentvariblevalue
            });
            fakedContext.InitializeMetadata(Assembly.GetAssembly(typeof(EnvironmentVariableDefinition)));

            #endregion






            var inputparameters = new ParameterCollection
            {
                { "Key", "MyKey" }
            };


            var outputparameters = new ParameterCollection
            {
                { "Exists", null },
                { "ValueString", null },
                { "ValueBool", null },
                { "ValueDecimal", null },
                { "Type", null },
                { "TypeName", null }
            };



            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_GetEnvironmentVariable",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            fakedContext.ExecutePluginWith<GetEnvironmentVariable>(context);
            #endregion



            Assert.IsTrue((bool)context.OutputParameters["Exists"]);
            Assert.AreEqual("yes", (string)context.OutputParameters["ValueString"]);
            Assert.AreEqual(true, (bool)context.OutputParameters["ValueBool"]);
            Assert.AreEqual(new OptionSetValue((int)EnvironmentVariableDefinition_Type.Boolean), (OptionSetValue)context.OutputParameters["Type"]);
        }

        [TestMethod]
        public void WhenKeyExistsAndTypeIsNumber()
        {
            var fakedContext = new XrmFakedContext();


            #region data
            var environmentvariabledef = new EnvironmentVariableDefinition()
            {
                Id = Guid.NewGuid(),
                SchemaName = "MyKey",
                DefaultValue = "10.5", 
                Type = EnvironmentVariableDefinition_Type.Number

            };
            var environmentvariblevalue = new EnvironmentVariableValue()
            {
                Id = Guid.NewGuid(),
                EnvironmentVariableDefinitionId = environmentvariabledef.ToEntityReference(),
                Value = "100.5" //overriden value

            };

            fakedContext.Initialize(new List<Entity>() {
                environmentvariabledef,
                environmentvariblevalue
            });
            fakedContext.InitializeMetadata(Assembly.GetAssembly(typeof(EnvironmentVariableDefinition)));

            #endregion






            var inputparameters = new ParameterCollection
            {
                { "Key", "MyKey" }
            };


            var outputparameters = new ParameterCollection
            {
                { "Exists", null },
                { "ValueString", null },
                { "ValueBool", null },
                { "ValueDecimal", null },
                { "Type", null },
                { "TypeName", null }
            };


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_GetEnvironmentVariable",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            fakedContext.ExecutePluginWith<GetEnvironmentVariable>(context);
            #endregion


            Assert.IsTrue((bool)context.OutputParameters["Exists"]);
            Assert.AreEqual("100.5", (string)context.OutputParameters["ValueString"]);
            Assert.AreEqual(new decimal(100.5), (decimal)context.OutputParameters["ValueDecimal"]);
            Assert.AreEqual(new OptionSetValue((int)EnvironmentVariableDefinition_Type.Number), (OptionSetValue)context.OutputParameters["Type"]);
        }
    }
    
}
