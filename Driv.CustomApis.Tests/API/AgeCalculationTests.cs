using Driv.CustomApis.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using FakeXrmEasy.Plugins;
using FakeXrmEasy.Abstractions.Plugins;

namespace Driv.CustomApis.Tests.API
{
    [TestClass]
    public class AgeCalculationTests : FakeXrmEasyTestBase
    {
        [TestMethod]
        public void AgeAtBirthdayIsZero()
        {

            #region ARRANGE
            //var fakedContext = new XrmFakedContext();


            
            

            var inputparameters = new ParameterCollection
            {
                { "Birthday", new DateTime(2020,2,29)},
                { "AgeAtDate", new DateTime(2020,2,29)},
            };


            var outputparameters = new ParameterCollection
            {
                { "Age", null }
               
            };
            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_AgeCalculation",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<AgeCalculation>(context);
            #endregion


            #region ASSERT
            Assert.AreEqual(0,(int)context.OutputParameters["Age"]);
            #endregion
        }


        public void AgeOneDayBeforeFirstBirthdayIsZero()
        {

            #region ARRANGE
            //var fakedContext = new XrmFakedContext();





            var inputparameters = new ParameterCollection
            {
                { "Birthday", new DateTime(2020,2,29)},
                { "AgeAtDate", new DateTime(2021,2,28)},
            };


            var outputparameters = new ParameterCollection
            {
                { "Age", null }

            };
            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_AgeCalculation",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<AgeCalculation>(context);
            #endregion


            #region ASSERT
            Assert.AreEqual(0, (int)context.OutputParameters["Age"]);
            #endregion
        }


        public void AgeOneDayAfterFirstBirthdayIsOne()
        {

            #region ARRANGE
            //var fakedContext = new XrmFakedContext();





            var inputparameters = new ParameterCollection
            {
                { "Birthday", new DateTime(2020,2,29)},
                { "AgeAtDate", new DateTime(2021,3,1)},
            };


            var outputparameters = new ParameterCollection
            {
                { "Age", null }

            };
            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_AgeCalculation",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<AgeCalculation>(context);
            #endregion


            #region ASSERT
            Assert.AreEqual(1, (int)context.OutputParameters["Age"]);
            #endregion
        }

        public void AgeOnFirstBirthdayIsOne()
        {

            #region ARRANGE
            //var fakedContext = new XrmFakedContext();





            var inputparameters = new ParameterCollection
            {
                { "Birthday", new DateTime(2020,1,1)},
                { "AgeAtDate", new DateTime(2021,1,1)},
            };


            var outputparameters = new ParameterCollection
            {
                { "Age", null }

            };
            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_AgeCalculation",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<AgeCalculation>(context);
            #endregion


            #region ASSERT
            Assert.AreEqual(1, (int)context.OutputParameters["Age"]);
            #endregion
        }


    }
    
}
