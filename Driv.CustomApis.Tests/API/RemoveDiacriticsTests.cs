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
    public class RemoveDiacriticsTests : FakeXrmEasyTestBase
    {
        [TestMethod]
        public void AllKindsOfDiaCritics()
        {

            #region ARRANGE
            //var fakedContext = new XrmFakedContext();


            
            

            var inputparameters = new ParameterCollection
            {
                { "InputString", "åçčéëèēêñŏ"}
            };


            var outputparameters = new ParameterCollection
            {
                { "OutputString", null }
               
            };
            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_RemoveDiacritics",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<RemoveDiacritics>(context);
            #endregion


            #region ASSERT
            Assert.AreEqual("acceeeeeno",(string)context.OutputParameters["OutputString"]);
            #endregion
        }


        


    }
    
}
