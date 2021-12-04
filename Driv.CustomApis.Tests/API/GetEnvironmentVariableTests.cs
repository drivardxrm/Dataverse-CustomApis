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
    public class GetEnvironnementVariableTest : FakeXrmEasyTestBase
    {
        [TestMethod]
        public void WhenKeyNotExists()
        {

            #region ARRANGE

            var environmentvariabledef = new EnvironmentVariableDefinition()
            {
                Id = Guid.NewGuid(),
                SchemaName = "MyKey",
                ValueSchema = "StringValue",
                Type = environmentvariabledefinition_type.String

            };

            _context.Initialize(new List<Entity>() {
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


            _context.ExecutePluginWith<GetEnvironmentVariable>(context);
            #endregion


            #region ASSERT
            Assert.IsFalse((bool)context.OutputParameters["Exists"]);
            #endregion
        }


        [TestMethod]
        public void WhenKeyExistsNoOverride()
        {
            #region ARRANGE
            var environmentvariabledef = new EnvironmentVariableDefinition()
            {
                Id = Guid.NewGuid(),
                SchemaName = "MyKey",
                DefaultValue = "MyKeyValue",
                Type = environmentvariabledefinition_type.String

            };

            _context.Initialize(new List<Entity>() {
                environmentvariabledef
            });
            _context.InitializeMetadata(Assembly.GetAssembly(typeof(EnvironmentVariableDefinition)));
            


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
            #endregion

            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_GetEnvironmentVariable",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<GetEnvironmentVariable>(context);
            #endregion


            #region ASSERT
            Assert.IsTrue((bool)context.OutputParameters["Exists"]);
            Assert.AreEqual("MyKeyValue",(string)context.OutputParameters["ValueString"]);
            Assert.AreEqual(new OptionSetValue((int)environmentvariabledefinition_type.String), (OptionSetValue)context.OutputParameters["Type"]);
            #endregion
        }

        [TestMethod]
        public void WhenKeyExistsAndOverriden()
        {
            #region ARRANGE

            var environmentvariabledef = new EnvironmentVariableDefinition()
            {
                Id = Guid.NewGuid(),
                SchemaName = "MyKey",
                DefaultValue = "MyKeyValue",
                Type = environmentvariabledefinition_type.String

            };
            var environmentvariblevalue = new EnvironmentVariableValue()
            {
                Id = Guid.NewGuid(),
                EnvironmentVariableDefinitionId = environmentvariabledef.ToEntityReference(),
                Value = "MyOverridenValue"

            };

            _context.Initialize(new List<Entity>() {
                environmentvariabledef,
                environmentvariblevalue
            });
            _context.InitializeMetadata(Assembly.GetAssembly(typeof(EnvironmentVariableDefinition)));


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
            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_GetEnvironmentVariable",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<GetEnvironmentVariable>(context);
            #endregion


            #region ASSERT
            Assert.IsTrue((bool)context.OutputParameters["Exists"]);
            Assert.AreEqual("MyOverridenValue", (string)context.OutputParameters["ValueString"]);
            Assert.AreEqual(new OptionSetValue((int)environmentvariabledefinition_type.String), (OptionSetValue)context.OutputParameters["Type"]);
            #endregion 
        }

        [TestMethod]
        public void WhenKeyExistsAndTypeIsBool()
        {

            #region ARRANGE
            var environmentvariabledef = new EnvironmentVariableDefinition()
            {
                Id = Guid.NewGuid(),
                SchemaName = "MyKey",
                DefaultValue = "no", //false
                Type = environmentvariabledefinition_type.Boolean

            };
            var environmentvariblevalue = new EnvironmentVariableValue()
            {
                Id = Guid.NewGuid(),
                EnvironmentVariableDefinitionId = environmentvariabledef.ToEntityReference(),
                Value = "yes" //overriden to true

            };

            _context.Initialize(new List<Entity>() {
                environmentvariabledef,
                environmentvariblevalue
            });
            _context.InitializeMetadata(Assembly.GetAssembly(typeof(EnvironmentVariableDefinition)));

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

            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_GetEnvironmentVariable",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<GetEnvironmentVariable>(context);
            #endregion


            #region ASSERT
            Assert.IsTrue((bool)context.OutputParameters["Exists"]);
            Assert.AreEqual("yes", (string)context.OutputParameters["ValueString"]);
            Assert.AreEqual(true, (bool)context.OutputParameters["ValueBool"]);
            Assert.AreEqual(new OptionSetValue((int)environmentvariabledefinition_type.Boolean), (OptionSetValue)context.OutputParameters["Type"]);
            #endregion 
        }

        [TestMethod]
        public void WhenKeyExistsAndTypeIsNumber()
        {

            #region ARRANGE
            
    
            var environmentvariabledef = new EnvironmentVariableDefinition()
            {
                Id = Guid.NewGuid(),
                SchemaName = "MyKey",
                DefaultValue = "10.5", 
                Type = environmentvariabledefinition_type.Number

            };
            var environmentvariblevalue = new EnvironmentVariableValue()
            {
                Id = Guid.NewGuid(),
                EnvironmentVariableDefinitionId = environmentvariabledef.ToEntityReference(),
                Value = "100.5" //overriden value

            };

            _context.Initialize(new List<Entity>() {
                environmentvariabledef,
                environmentvariblevalue
            });
            _context.InitializeMetadata(Assembly.GetAssembly(typeof(EnvironmentVariableDefinition)));



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
            #endregion

            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_GetEnvironmentVariable",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<GetEnvironmentVariable>(context);
            #endregion

            #region ASSERT
            Assert.IsTrue((bool)context.OutputParameters["Exists"]);
            Assert.AreEqual("100.5", (string)context.OutputParameters["ValueString"]);
            Assert.AreEqual(new decimal(100.5), (decimal)context.OutputParameters["ValueDecimal"]);
            Assert.AreEqual(new OptionSetValue((int)environmentvariabledefinition_type.Number), (OptionSetValue)context.OutputParameters["Type"]);
            #endregion
        }

        [TestMethod]
        public void WhenKeyExistsAndTypeIsJson()
        {

            #region ARRANGE
            
            var environmentvariabledef = new EnvironmentVariableDefinition()
            {
                Id = Guid.NewGuid(),
                SchemaName = "MyKey",
                DefaultValue = "{'test':'test'}",
                Type = environmentvariabledefinition_type.JSON

            };
            var environmentvariblevalue = new EnvironmentVariableValue()
            {
                Id = Guid.NewGuid(),
                EnvironmentVariableDefinitionId = environmentvariabledef.ToEntityReference(),
                Value = "{'test2':'test2'}" //overriden value

            };

            _context.Initialize(new List<Entity>() {
                environmentvariabledef,
                environmentvariblevalue
            });
            _context.InitializeMetadata(Assembly.GetAssembly(typeof(EnvironmentVariableDefinition)));



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
            #endregion

            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_GetEnvironmentVariable",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<GetEnvironmentVariable>(context);
            #endregion

            #region ASSERT
            Assert.IsTrue((bool)context.OutputParameters["Exists"]);
            Assert.AreEqual("{'test2':'test2'}", (string)context.OutputParameters["ValueString"]);
            Assert.AreEqual(new OptionSetValue((int)environmentvariabledefinition_type.JSON), (OptionSetValue)context.OutputParameters["Type"]);
            #endregion
        }
    }
    
}
