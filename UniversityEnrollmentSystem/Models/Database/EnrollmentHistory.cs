using System;
using System.Collections.Generic;

namespace UniversityEnrollmentSystem.Models.Database;

public partial class EnrollmentHistory
{
    public int HistoryId { get; set; }

    public int? EnrollmentId { get; set; }

    public int? StudentId { get; set; }

    public int? CourseOfferingsId { get; set; }

    public decimal? FinalGrade { get; set; }

    public string? ActionType { get; set; }

    public DateTime? ActionDate { get; set; }
}
