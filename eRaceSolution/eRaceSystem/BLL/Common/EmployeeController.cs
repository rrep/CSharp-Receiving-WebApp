using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eRaceSystem.DAL;
using eRaceSystem.Data.Entities;
using System.ComponentModel;
using eRaceSystem.Data.POCOs;
using eRaceSystem.Data.DTOs;

#endregion

namespace eRaceSystem.BLL.Common
{
    [DataObject]
    public class EmployeeController
    {
        //employee get method for displaying name on page
        public Employee Employee_Get(int employeeid)
        {
            using (var context = new ERaceContext())
            {
                return context.Employees.Find(employeeid);
            }
        }

        public List<Employee> Employees_GetFullName()
        {
            using (var context = new ERaceContext()) 
            {
                var data = from x in context.Employees
                           select new Employee
                           {
                               FirstName = x.FirstName,
                               LastName = x.LastName
                           };

                return data.ToList();
            }

        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectionList> Employee_ListNames()
        {
            using (var context = new ERaceContext())
            {
                var employeelist = from x in context.Employees
                                   orderby x.LastName, x.FirstName
                                   select new SelectionList
                                   {
                                       DisplayText = x.LastName + ", " + x.FirstName,
                                       IDValueField = x.EmployeeID
                                   };
                return employeelist.ToList();
            }
        }

        public List<string> Employees_GetTitles()
        {
            using (var context = new ERaceContext())
            {
                var data2 = (from x in context.Positions
                             select x.Description).Distinct();

                return data2.ToList();
            }
        }

        public List<Employee> Employee_List()
        {
            using (var context = new ERaceContext())
            {
                return context.Employees.ToList();
            }
        }

        public List<EmployeeRoles> EmployeeRoles_List()
        {
            using (var context = new ERaceContext())
            {
                var results = from x in context.Employees
                              select new EmployeeRoles
                              {
                                  EmployeeID = x.EmployeeID,
                                  FirstName = x.FirstName,
                                  LastName = x.LastName,
                                  Role = x.Position.Description
                              };

               return results.ToList();
            }
        }

        
    }

}
