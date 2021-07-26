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
    public class RegexReplaceTest : FakeXrmEasyTestBase
    {
        [TestMethod]
        public void NoMatchTest()
        {

            #region ARRANGE
            //var fakedContext = new XrmFakedContext();


            
            

            var inputparameters = new ParameterCollection
            {
                { "InputString", "myteststring"},
                { "Regex", @"\d"}, //any number
                { "ReplaceString", "X"}
            };


            var outputparameters = new ParameterCollection
            {
                { "OutputString", null }
               
            };
            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_RegexReplace",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<RegexReplace>(context);
            #endregion


            #region ASSERT
            Assert.AreEqual("myteststring", (string)context.OutputParameters["OutputString"]);
            #endregion
        }

        [TestMethod]
        public void MatchTest()
        {

            #region ARRANGE
            //var fakedContext = new XrmFakedContext();





            var inputparameters = new ParameterCollection
            {
                { "InputString", "123mytest1string123"},
                { "Regex", @"\d"}, //any number
                { "ReplaceString", "X"}
            };


            var outputparameters = new ParameterCollection
            {
                { "OutputString", null }

            };
            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_RegexReplace",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<RegexReplace>(context);
            #endregion


            #region ASSERT
            Assert.AreEqual("XXXmytestXstringXXX", (string)context.OutputParameters["OutputString"]);
            #endregion
        }




    }
    
}
