
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driv.CustomApis.Tests
{
    public class FakeXrmEasyTestBase
    {
        protected readonly IOrganizationService _service;
        protected readonly XrmFakedContext _context;

        public FakeXrmEasyTestBase()
        {
            _context = new XrmFakedContext();
            _service = _context.GetOrganizationService();
        }


    }
}
