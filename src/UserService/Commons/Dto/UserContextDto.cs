namespace UserService.Commons.Dto;

public record UserContextDto(string Email, Guid Subject, string UserName,bool IsAuthenticated);