using Driv.CustomApis.API;
using FakeXrmEasy;
using FakeXrmEasy.Abstractions.Plugins;
using FakeXrmEasy.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;


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
