﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.Models
{
    public class WorkoutPlan : BaseEntity
    {
        public IEnumerable<Exercise> Exercises { get; set; }
        public IList<int>? DoneWorkoutsIds { get; set; }
        public string Name { get; set; }
    }
}
