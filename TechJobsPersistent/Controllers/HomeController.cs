﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        private JobDbContext context;

        public HomeController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        [HttpGet("/Add")]
        public IActionResult AddJob()
        {
            List<Employer> employers = context.Employers.ToList();
            List<Skill> skills = context.Skills.ToList();

            AddJobViewModel addJobViewModel = new AddJobViewModel(employers,skills );
            return View(addJobViewModel);
        }

        [HttpPost]
        public IActionResult ProcessAddJobForm(AddJobViewModel addJobViewModel,string[] selectedSkills)
        {
            if (ModelState.IsValid)
            {
                Job job = new Job()
                {
                    Name = addJobViewModel.JobName,
                    EmployerId = addJobViewModel.EmployerId,
                    
                };
                job.JobSkills = new List<JobSkill>();
                //looping selected skills
                foreach(string selectSkill in selectedSkills)
                {
                    JobSkill jobSkill = new JobSkill
                    {
                        JobId = job.Id,
                        SkillId = int.Parse(selectSkill),
                    };
                    //context.JobSkills.Add(jobSkill);
                    job.JobSkills.Add(jobSkill);
                    context.Jobs.Add(job);
                    context.JobSkills.Add(jobSkill);



                }


                context.SaveChanges();

                return Redirect("/Home");
            }

            return View("Add",addJobViewModel);
        }

        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }
    }
}
