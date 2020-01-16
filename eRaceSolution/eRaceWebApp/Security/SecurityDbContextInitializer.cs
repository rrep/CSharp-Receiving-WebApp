using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#region Additional Namespaces
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Data.Entity;
using eRaceSystem.BLL.Common;
using eRaceWebApp.Models;
using eRaceWebApp;
using eRaceSystem.Data.Entities;
using eRaceSystem.Data.POCOs;
#endregion

namespace WebApp.Security
{
    public class SecurityDbContextInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            #region Seed the roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //Startup Roles
            var startupRoles = ConfigurationManager.AppSettings["startupRoles"].Split(';');
            foreach (var role in startupRoles)
                roleManager.Create(new IdentityRole { Name = role });

            //Roles list from positions table
            EmployeeController sysmgr = new EmployeeController();
            List<string> employeeroles = sysmgr.Employees_GetTitles();
            foreach (var role in employeeroles)
                roleManager.Create(new IdentityRole { Name = role });
            #endregion

            #region Seed the users

            //admin user seeding
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            string adminUser = ConfigurationManager.AppSettings["adminUserName"];
            string adminRole = ConfigurationManager.AppSettings["adminRole"];
            string adminEmail = ConfigurationManager.AppSettings["adminEmail"];
            string adminPassword = ConfigurationManager.AppSettings["adminPassword"];
            var result = userManager.Create(new ApplicationUser
            {
                UserName = adminUser,
                Email = adminEmail
            }, adminPassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(adminUser).Id, adminRole);

            List<EmployeeRoles> employeeNames = sysmgr.EmployeeRoles_List();
            foreach (var item in employeeNames)
            {
                var count = 0;

                string employeeRole = item.Role;
                string employeeUser = "";
                string employeePassword = ConfigurationManager.AppSettings["employeePassword"];
                do
                {
                    employeeUser =  item.FirstName[0] + item.LastName;
                    if (count > 0)
                    {
                        employeeUser += count;
                    }
                        count++;

                } while (!(userManager.FindByName(employeeUser) is null));

            var userNameResults = userManager.Create(new ApplicationUser
            {
                EmployeeId = item.EmployeeID,
                UserName = employeeUser,
                Email = employeeUser + "@ERace.somewhere.ca"

            }, employeePassword);
            if (userNameResults.Succeeded)
                userManager.AddToRole(userManager.FindByName(employeeUser).Id, employeeRole);
            }

            #endregion

            // ... etc. ...

            base.Seed(context);
        }
    }
}