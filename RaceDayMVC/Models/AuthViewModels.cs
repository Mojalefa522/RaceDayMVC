namespace RaceDayMVC.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }

    public class EventSummaryViewModel
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
        public int TotalEnrolments { get; set; }
    }

    public class CreateEventViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class EventListViewModel
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
    }

    public class EventDetailViewModel
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
        public string? ImageUrl { get; set; }
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }

    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Distance { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentEnrolments { get; set; }
        public bool IsEnrolled { get; set; }
    }

    public class CreateCategoryViewModel
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Distance { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int MaxCapacity { get; set; }
    }

    public class MyEnrolmentViewModel
    {
        public int EnrolmentId { get; set; }
        public string EventName { get; set; }
        public string CategoryName { get; set; }
        public DateTime EnrolmentDate { get; set; }
        public string Status { get; set; }
        public string BibNumber { get; set; }
    }

    public class MyResultViewModel
    {
        public string EventName { get; set; }
        public string CategoryName { get; set; }
        public string FinishTime { get; set; }
        public int? Position { get; set; }
        public string Status { get; set; }
    }

    public class EventEnrolmentViewModel
    {
        public int EnrolmentId { get; set; }
        public string ParticipantName { get; set; }
        public string CategoryName { get; set; }
        public string Status { get; set; }
        public string BibNumber { get; set; }
        public DateTime EnrolmentDate { get; set; }
    }

    public class ResultEntryViewModel
    {
        public int EventId { get; set; }
        public List<EnrolmentResultViewModel> Enrolments { get; set; } = new List<EnrolmentResultViewModel>();
    }

    public class EnrolmentResultViewModel
    {
        public int EnrolmentId { get; set; }
        public string ParticipantName { get; set; }
        public string CategoryName { get; set; }
        public string BibNumber { get; set; }
        public string? FinishTime { get; set; }
        public int? Position { get; set; }
        public string Status { get; set; }
    }

    public class SubmitResultViewModel
    {
        public int EventId { get; set; }
        public int EnrolmentId { get; set; }
        public string FinishTime { get; set; }
        public int Position { get; set; }
    }

    public class UpdateOrganiserProfileDto
    {
        public string? CompanyName { get; set; }
        public string? Website { get; set; }
        public string? ContactPhone { get; set; }
    }

    public class UpdateParticipantProfileDto
    {
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? EmergencyContact { get; set; }
        public string? MedicalNotes { get; set; }
    }
}