using Driv.CustomApis.API;
using Driv.CustomApis.Models;
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
using XrmVision.Extensions.Extensions;

namespace Driv.CustomApis.Tests.API
{
    [TestClass]
    public class RegexMatchTest : FakeXrmEasyTestBase
    {
        [TestMethod]
        public void NoMatchTest()
        {

            #region ARRANGE
            //var fakedContext = new XrmFakedContext();


            
            

            var inputparameters = new ParameterCollection
            {
                { "InputString", "myteststring"},
                { "Regex", @"\d"} //any number
            };


            var outputparameters = new ParameterCollection
            {
                { "MatchFound", null },
                { "Matches", null }

            };
            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_RegexMatch",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<RegexMatch>(context);
            #endregion


            #region ASSERT
            Assert.IsFalse((bool)context.OutputParameters["MatchFound"]);
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
                { "Regex", @"\d"} //any number
            };


            var outputparameters = new ParameterCollection
            {
                { "MatchFound", null },
                { "Matches", null }

            };
            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_RegexMatch",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<RegexMatch>(context);
            #endregion


            #region ASSERT
            Assert.IsTrue((bool)context.OutputParameters["MatchFound"]);
            var matches = JsonHelper.DeserializeJson<List<RegexMatchModel>>((string)context.OutputParameters["Matches"]);
            Assert.AreEqual(7,matches.Count);
            Assert.AreEqual(0, matches.First().Index);
            Assert.AreEqual("1", matches.First().Value);

            #endregion
        }




    }
    
}
