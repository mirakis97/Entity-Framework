using P01_StudentSystem.Data.Models.Enumerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Homework
    {
        [Key]
        public int HomeworkId { get; set; }
        [Column(TypeName = "varchar(2048)")]
        public string Content  { get; set; }
        public ContentType ContentType { get; set; }

        public DateTime SubmissionTime { get; set; }

        public int StudentId { get; set; }
        public Student Students { get; set; }

        public int CourseId { get; set; }
        public Course Courses { get; set; }
    }
}
