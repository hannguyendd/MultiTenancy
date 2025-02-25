using System.ComponentModel.DataAnnotations;

namespace SharedDatabase.Contracts;

public record SignUpRequest([EmailAddress] string Email, string FullName, string Password);