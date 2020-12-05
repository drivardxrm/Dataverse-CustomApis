using Driv.CustomApis.API;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Driv.CustomApis.Tests.API
{
    public class GetEnvironmentVariableTests
    {
        [TestClass]
        public class UnitTest1
        {
            [TestMethod]
            public void WhenKeyNotExists()
            {
                var fakedContext = new XrmFakedContext();


                #region data
                var environmentvariabledef = new EnvironmentVariableDefinition()
                {
                    Id = Guid.NewGuid(),
                    SchemaName = "MyKey"
                    
                };

                fakedContext.Initialize(new List<Entity>() {
                    environmentvariabledef
                });

                #endregion






                var inputparameters = new ParameterCollection
                {
                    { "Key", "MyNonExistingKey" }
                };


                var outputparameters = new ParameterCollection
                {
                    { "Exists", null },
                    { "ValueString", null }
                };



                var context = new XrmFakedPluginExecutionContext
                {
                    MessageName = "driv_GetEnvironmentVariable",
                    InputParameters = inputparameters,
                    OutputParameters = outputparameters,
                    Stage = 30
                };

                
                fakedContext.ExecutePluginWith<GetEnvironmentVariable>(context);


                
                Assert.IsFalse((bool)context.OutputParameters["Exists"]);
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
                    DefaultValue = "MyKeyValue"

                };

                fakedContext.Initialize(new List<Entity>() {
                    environmentvariabledef
                });

                #endregion






                var inputparameters = new ParameterCollection
                {
                    { "Key", "MyKey" }
                };


                var outputparameters = new ParameterCollection
                {
                    { "Exists", null },
                    { "ValueString", null }
                };



                var context = new XrmFakedPluginExecutionContext
                {
                    MessageName = "driv_GetEnvironmentVariable",
                    InputParameters = inputparameters,
                    OutputParameters = outputparameters,
                    Stage = 30
                };


                fakedContext.ExecutePluginWith<GetEnvironmentVariable>(context);



                Assert.IsTrue((bool)context.OutputParameters["Exists"]);
                Assert.AreEqual("MyKeyValue",(string)context.OutputParameters["ValueString"]);
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
                    DefaultValue = "MyKeyValue"

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

                #endregion






                var inputparameters = new ParameterCollection
                {
                    { "Key", "MyKey" }
                };


                var outputparameters = new ParameterCollection
                {
                    { "Exists", null },
                    { "ValueString", null }
                };



                var context = new XrmFakedPluginExecutionContext
                {
                    MessageName = "driv_GetEnvironmentVariable",
                    InputParameters = inputparameters,
                    OutputParameters = outputparameters,
                    Stage = 30
                };


                fakedContext.ExecutePluginWith<GetEnvironmentVariable>(context);



                Assert.IsTrue((bool)context.OutputParameters["Exists"]);
                Assert.AreEqual("MyOverridenValue", (string)context.OutputParameters["ValueString"]);
            }
        }
    }
}
