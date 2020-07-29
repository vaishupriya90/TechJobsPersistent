using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechJobsPersistent.Models;

namespace TechJobsPersistent.ViewModels
{
    public class AddJobViewModel
    {
        public int EmployerId { get; set; }

        public string JobName { get; set; }

        public List<int> SkillId { get; set; }
        public Skill Skill { get; set; }

        public List<Skill> Skills { get; set; }

        public List<SelectListItem> Employers { get; set; }


        public AddJobViewModel(List<Employer> employers,List<Skill>skills)
        {
            Skills = new List<Skill>(skills);
            Employers = new List<SelectListItem>();

            foreach (var employer in employers)
            {
                Employers.Add(new SelectListItem
                {
                    Value = employer.Id.ToString(),
                    Text = employer.Name
                });
            }
        }
        public AddJobViewModel()
        {
        }
    }
}
