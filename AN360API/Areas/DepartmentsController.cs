using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AN360API.Areas
{
    public class Employee
    {
        public string Name { get; set; }
        public Department Department { get; set; }
    }

    public class Department
    {
        public string Name { get; set; }
        public Employee Manager { get; set; }
    }

    public class DepartmentsController : ApiController
    {
        public Department Get(int id)
        {
            Department sales = new Department() { Name = "Sales" };
            Employee alice = new Employee() { Name = "Alice", Department = sales };
            sales.Manager = alice;
            return sales;
        }
    }
}