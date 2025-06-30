using Application.VideoGames.Commands;
using FluentValidation.TestHelper;
using Xunit;

namespace Tests.Validation;

public class VideoGameCommandValidatorTests
{
    [Fact]
    public void AddCommand_Should_Have_Error_When_Title_Empty()
    {
        var validator = new AddVideoGameCommand.Validator();
        var command = new AddVideoGameCommand
        {
            Title = "",
            Genre = "Action",
            ReleaseDate = new DateTime(2023, 1, 1)
        };

        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void AddCommand_Should_Have_Error_When_Genre_Empty()
    {
        var validator = new AddVideoGameCommand.Validator();
        var command = new AddVideoGameCommand
        {
            Title = "Valid",
            Genre = "",
            ReleaseDate = new DateTime(2023, 1, 1)
        };

        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Genre);
    }

    [Fact]
    public void AddCommand_Should_Have_Error_When_ReleaseDate_Default()
    {
        var validator = new AddVideoGameCommand.Validator();
        var command = new AddVideoGameCommand
        {
            Title = "Valid",
            Genre = "Action",
            ReleaseDate = DateTime.MinValue
        };

        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ReleaseDate);
    }

    [Fact]
    public void DeleteCommand_Should_Have_Error_When_Id_Invalid()
    {
        var validator = new DeleteVideoGameCommand.Validator();
        var command = new DeleteVideoGameCommand { VideoGameId = 0 };

        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.VideoGameId);
    }

    [Fact]
    public void UpdateCommand_Should_Have_Error_When_Id_Invalid()
    {
        var validator = new UpdateVideoGameCommand.Validator();
        var command = new UpdateVideoGameCommand { VideoGameId = -1 };

        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.VideoGameId);
    }
}
