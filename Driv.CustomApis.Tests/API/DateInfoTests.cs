using Driv.CustomApis.API;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;


namespace Driv.CustomApis.Tests.API
{
    [TestClass]
    public class DateInfoTests : FakeXrmEasyTestBase
    {
        [TestMethod]
        public void Feb28OfLeapYearIsNotLastDayOfMonth()
        {

            #region ARRANGE

            
          
            var inputparameters = new ParameterCollection
            {
                { "Date", new DateTime(2020,2,28)}
            };


            var outputparameters = new ParameterCollection
            {
                { "Year", null },
                { "Month", null },
                { "Day", null },
                { "DayOfYear", null },
                { "DayOfWeek", null },
                { "IsLeapYear", null },
                { "IsLastDayOfMonth", null },

            };
            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_DateInfo",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<DateInfo>(context);
            #endregion


            #region ASSERT
            Assert.IsTrue((bool)context.OutputParameters["IsLeapYear"]);
            Assert.IsFalse((bool)context.OutputParameters["IsLastDayOfMonth"]);
            #endregion
        }

        [TestMethod]
        public void Feb29OfLeapYearIsLastDayOfMonth()
        {

            #region ARRANGE



            var inputparameters = new ParameterCollection
            {
                { "Date", new DateTime(2020,2,29)}
            };


            var outputparameters = new ParameterCollection
            {
                { "Year", null },
                { "Month", null },
                { "Day", null },
                { "DayOfYear", null },
                { "DayOfWeek", null },
                { "IsLeapYear", null },
                { "IsLastDayOfMonth", null },

            };
            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_DateInfo",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<DateInfo>(context);
            #endregion


            #region ASSERT
            Assert.IsTrue((bool)context.OutputParameters["IsLeapYear"]);
            Assert.IsTrue((bool)context.OutputParameters["IsLastDayOfMonth"]);
            #endregion
        }

        [TestMethod]
        public void Feb28OfNonLeapYearIsLastDayOfMonth()
        {

            #region ARRANGE



            var inputparameters = new ParameterCollection
            {
                { "Date", new DateTime(2019,2,28)}
            };


            var outputparameters = new ParameterCollection
            {
                { "Year", null },
                { "Month", null },
                { "Day", null },
                { "DayOfYear", null },
                { "DayOfWeek", null },
                { "IsLeapYear", null },
                { "IsLastDayOfMonth", null },

            };
            #endregion


            #region ACT
            var context = new XrmFakedPluginExecutionContext
            {
                MessageName = "driv_DateInfo",
                InputParameters = inputparameters,
                OutputParameters = outputparameters,
                Stage = 30
            };


            _context.ExecutePluginWith<DateInfo>(context);
            #endregion


            #region ASSERT
            Assert.IsFalse((bool)context.OutputParameters["IsLeapYear"]);
            Assert.IsTrue((bool)context.OutputParameters["IsLastDayOfMonth"]);
            #endregion
        }




    }
    
}
