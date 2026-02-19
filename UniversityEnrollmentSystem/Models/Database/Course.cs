using System;
using System.Collections.Generic;

namespace UniversityEnrollmentSystem.Models.Database;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseCode { get; set; } = null!;

    public string Title1 { get; set; } = null!;

    public int Credits { get; set; }

    public virtual ICollection<CourseOffering> CourseOfferings { get; set; } = new List<CourseOffering>();
}