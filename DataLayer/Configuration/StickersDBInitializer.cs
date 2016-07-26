using System;
using System.Collections.Generic;
using System.Data.Entity;
using DataLayer.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace DataLayer.Configuration
{
    public class StickersDbInitializer : CreateDatabaseIfNotExists<StickersContext>
    {
        protected override void Seed(StickersContext context)
        {
            var userManager = new UserManager<User>(new UserStore<User>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var roleAdmin = new IdentityRole
            {
                Name = "Admin"
            };
            var roleUser = new IdentityRole
            {
                Name = "User"
            };
            roleManager.Create(roleAdmin);
            roleManager.Create(roleUser);
          
            Department department = new Department()
            {
                Id = Guid.NewGuid(),
                Name = "AltexSoft",
                Description = "Our department"
            };
            context.Departments.Add(department);

            
            var user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "evgeniy@gmail.com",
                PasswordHash = "AOsGhNNhuNR1eXeFrmobV+dyTStHr5c5KpND+N5WHVGiiOMYdo+ljrz6ZDXJVJ7OeQ==",
                SecurityStamp = "8e2ce8dc-6552-40c4-abe6-8f0dbbc9e2c4",
                Firstname = "Evgeniy",
                LastName = "Glushko",
                Avatar = "391bear.jpg",
                IsActivate = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                Email = "evgeniy@gmail.com",
                DepartmentId = department.Id,
                Culture = "en"
            };
            var result = userManager.Create(user);
            if (result.Succeeded)
            {
                userManager.AddToRole(user.Id, roleAdmin.Name);
            }
           
            context.Users.Add(new User
            {
                Id = "1",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Al",
                LastName = "Pacino",
                Avatar = "Al_Pacino.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "al.pacino@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true
            });


            context.Users.Add(new User
            {
                Id = "2",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Angelina",
                LastName = "Jolie",
                Avatar = "AngelinaJolie.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "ang.jolie@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "3",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Cameron",
                LastName = "Diaz",
                Avatar = "CameronDiaz.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "cameron.diaz@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true
            });
            context.Users.Add(new User
            {
                Id = "4",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Denzel",
                LastName = "Washington",
                Avatar = "DenzelWashington.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "den_washinton@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "5",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Emma",
                LastName = "Stone",
                Avatar = "emma-stone.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "emma.stone@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "6",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Gary",
                LastName = "Oldman",
                Avatar = "GaryOldman.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "gary.oldman@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "7",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Halle",
                LastName = "Berry",
                Avatar = "HalleBerry.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "halle.berry@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "8",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Jennifer",
                LastName = "Aniston",
                Avatar = "JenniferAniston.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "jen.aniston@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "9",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Jessica",
                LastName = "Alba",
                Avatar = "JessicaAlba.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "jess.alba@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "10",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Johnny",
                LastName = "Depp",
                Avatar = "JohnnyDepp.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "johnny.depp@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "11",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Leonardo",
                LastName = "Dicaprio",
                Avatar = "LeonardoDicaprio.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "dicaprio@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "12",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Marion",
                LastName = "Cotillard",
                Avatar = "MarionCotillard.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "cotillard@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "13",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Megan",
                LastName = "Fox",
                Avatar = "MeganFox.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "megan.fox@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "14",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Morgan",
                LastName = "Freeman",
                Avatar = "MorganFreeman.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "freeman@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "15",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Natalie",
                LastName = "Partman",
                Avatar = "Natalie-Portman.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "partman@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "16",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Nicole",
                LastName = "Kidman",
                Avatar = "NicoleKidman.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "nicole.kidman@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "17",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Rebert",
                LastName = "De-niro",
                Avatar = "robert-de-niro.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "de_niro@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });
            context.Users.Add(new User
            {
                Id = "18",
                UserName = "Admin",
                PasswordHash = "JaS3MecSfP8f23L0DfTeuBV+AvtCpVcC8ybqb9XVjME=",
                Firstname = "Sandra",
                LastName = "Bullock",
                Avatar = "SandraBullock.jpg",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                Email = "sandra.bullock@gmail.com",
                DepartmentId = department.Id,
                IsActivate = true,
                EmailConfirmed = true
            });


            context.Positions.Add(new Position
            {
                Id = Guid.NewGuid(),
                Name = "Project Manager",
                Description = "Группа пользователей с привилегиями менеджера."
            });

            context.Positions.Add(new Position
            {
                Id = Guid.NewGuid(),
                Name = "Product Owner",
                Description = "Пользователь с привилегиями Product Owner"
            });

            context.Positions.Add(new Position
            {
                Id = Guid.NewGuid(),
                Name = "Developer",
                Description = "Группа пользователей с привилегиями Product Developer"
            });
            
            context.Positions.Add(new Position
            {
                Id = Guid.NewGuid(),
                Name = "Quality Assurance",
                Description = "Группа пользователей с привилегиями тестировщика"
            });

            //context.Positions.Add(new Position
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Scrum Master",
            //    Description = "Группа пользователей с привилегиями Scrum Master"
            //});

            StateProject archiveStateProject = new StateProject
            {
                Id = Guid.NewGuid(),
                Name = "Archive",
                Description = "Description about Archive state"

            };

            StateProject openStateProject = new StateProject
            {
                Id = Guid.NewGuid(),
                Name = "Open",
                Description = "Description about Open state"
            };

            //context.StateProjects.Add(archiveStateProject);
            //context.StateProjects.Add(openStateProject);

            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "M1T1",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });

            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "M1T2",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });

            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "M1T3",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });

            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "M1T4",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });
            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "M1T5",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });
            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "M1T6",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });
            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "M1T7",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });
          
            
            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "M2T2",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });
            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "M2T3",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });
            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "M2T4",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });
            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "M2T5",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });
          
            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "J1T1",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });
            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "J1T2",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });
            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "J1T3",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });
            context.Teams.Add(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "J1T4",
                Description = "Description",
                DateCreated = DateTime.Now,
                DepartmentId = department.Id
            });
            
            context.Projects.Add(new Project
            {
                Id = Guid.NewGuid(),
                Name = "LogViewer",
                Description = "This is LogViewer description",
                DateCreate = DateTime.Now,
                Code = "LG",
                StateProject = archiveStateProject,
                DepartmentId = department.Id
            });

            context.Projects.Add(new Project
            {
                Id = Guid.NewGuid(),
                Name = "BugTracker",
                Description = "This is BugTracker description",
                StateProject = openStateProject,
                DateCreate = DateTime.Now,
                Code = "BT",
                DepartmentId = department.Id
            });

            context.IssuePriorities.Add(new IssuePriority()
            {
                Id = Guid.NewGuid(),
                Name = "Minor",
                Description = "Description",
            });
            context.IssuePriorities.Add(new IssuePriority()
            {
                Id = Guid.NewGuid(),
                Name = "Major",
                Description = "Description",
            });
            context.IssuePriorities.Add(new IssuePriority()
            {
                Id = Guid.NewGuid(),
                Name = "Normal",
                Description = "Description",
            });
            context.IssuePriorities.Add(new IssuePriority()
            {
                Id = Guid.NewGuid(),
                Name = "Critical",
                Description = "Description",
            });

            context.IssueTypes.Add(new IssueType()
            {
                Id = Guid.NewGuid(),
                Name = "Epic",
                Description = "Description",
            });
            context.IssueTypes.Add(new IssueType()
            {
                Id = Guid.NewGuid(),
                Name = "Story",
                Description = "Description",
            });
            context.IssueTypes.Add(new IssueType()
            {
                Id = Guid.NewGuid(),
                Name = "Task",
                Description = "Description",
            });
            context.IssueTypes.Add(new IssueType()
            {
                Id = Guid.NewGuid(),
                Name = "Bug",
                Description = "Description",
            });

            context.IssueStates.Add(new IssueState()
            {
                Id = Guid.NewGuid(),
                Name = "Open",
                Description = "Description",
            });
            context.IssueStates.Add(new IssueState()
            {
                Id = Guid.NewGuid(),
                Name = "Reopened",
                Description = "Description",
            });
            context.IssueStates.Add(new IssueState()
            {
                Id = Guid.NewGuid(),
                Name = "In Progress",
                Description = "Description",
            });
            context.IssueStates.Add(new IssueState()
            {
                Id = Guid.NewGuid(),
                Name = "Fixed",
                Description = "Description",
            });
            context.IssueStates.Add(new IssueState()
            {
                Id = Guid.NewGuid(),
                Name = "Verified",
                Description = "Description",
            });
            Project project = new Project
            {
                Id = Guid.NewGuid(),
                Name = "ScrumTracker",
                Description = "This is BugTracker description",
                StateProject = openStateProject,
                DateCreate = DateTime.Now,
                Code = "BT"
            };
            context.Projects.Add(project);
            Issue issue = new Issue() { Id = Guid.NewGuid(), Name = "Issue", DateCreate = DateTime.Now, ProjectId = project.Id };
            Issue issue1 = new Issue() { Id = Guid.NewGuid(), Name = "Issue1", DateCreate = DateTime.Now, ParentIssueId = issue.Id, ProjectId = project.Id };
            Issue issue2 = new Issue() { Id = Guid.NewGuid(), Name = "Issue2", DateCreate = DateTime.Now, ParentIssueId = issue.Id, ProjectId = project.Id };
            context.Issues.Add(issue);
            context.Issues.Add(issue1);
            context.Issues.Add(issue2);

            context.TimeTrackingTypes.Add(new TimeTrackingType()
            {
                Id = Guid.NewGuid(),
                Name = "Development",
                Description = "Description",
            });

            context.TimeTrackingTypes.Add(new TimeTrackingType()
            {
                Id = Guid.NewGuid(),
                Name = "Documentation",
                Description = "Description",
            });

            context.TimeTrackingTypes.Add(new TimeTrackingType()
            {
                Id = Guid.NewGuid(),
                Name = "Testing",
                Description = "Description",
            });
            base.Seed(context);
        }
    }
}
