﻿namespace GymTracker.Models
{
    public class SpecificExercise
    {
        public int Id
        {
            get; set;
        }
        public Exercise Exercise { get; set; }
        public double Repetitions { get; set; }
        public byte Sets { get; set; }
        public double Weight { get; set; }
    }
}
