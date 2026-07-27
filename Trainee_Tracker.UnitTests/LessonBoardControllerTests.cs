using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Trainee_Tracker.Controllers;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.UnitTests;
//code-Owner: Julia Sandner (total class)
public class LessonBoardControllerTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<ILessonAssignmentRepository> _assignmentRepoMock;
    private readonly Mock<IAssignmentService> _assignmentServiceMock;
    private readonly LessonBoardController _controller;
    
    public LessonBoardControllerTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _assignmentRepoMock = new Mock<ILessonAssignmentRepository>();
        _assignmentServiceMock = new Mock<IAssignmentService>();
        
        _controller = new LessonBoardController(_userRepoMock.Object, _assignmentRepoMock.Object, _assignmentServiceMock.Object);

        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        SetUser(41);
    }

    private void SetUser(int? traineeID)
    {
        var claims = new List<Claim>();
        if (traineeID.HasValue)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, traineeID.Value.ToString()));
        }
        var identity = new ClaimsIdentity(claims, traineeID.HasValue ? "TestAuthType" : null);
        _controller.ControllerContext = new ControllerContext{HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)}};
    }

    /// <summary>
    /// test for a trainee starting an assignment (successful)
    /// </summary>
    [Fact]
    public void Start_ValidAssignment_UpdateStatusAndRedirectToLessonBoard()
    {
        //arrange
        var assignment = new LessonAssignment {Id = 5, TraineeId = 41};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup( s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Started)).Returns(assignment);
        SetUser(41);

        //Act
        var result = _controller.StartAssignment(5);

        //Assert
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Started), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("LessonBoard", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);

    }

    /// <summary>
    /// test for a trainee starting an unknown assignment
    /// </summary>
    [Fact]
    public void Start_UnknownAssignment_ReturnNotFound()
    {
        //Arrange
        _assignmentRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((LessonAssignment?) null);
         //Act
         var result = _controller.StartAssignment(999);

         //Assert
         Assert.IsType<NotFoundResult>(result);
         _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    /// <summary>
    /// test for a trainee starting an assignment without authorization
    /// </summary>
    [Fact]
    public void Start_NoUserClaim_ReturnUnauthorized()
    {
        //Arrange
        var assignment = new LessonAssignment {Id = 5, TraineeId = 41};
        _assignmentRepoMock.Setup(r =>r.GetById(5)).Returns(assignment);
        SetUser(null);

        //Act
        var result = _controller.StartAssignment(5);

        //Assert
        Assert.IsType<UnauthorizedResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    /// <summary>
    /// test for a trainee starting an assignment that is assignent to another trainee
    /// </summary>
    [Fact]
    public void Start_AssignmentNotOwnedByTrainee_ReturnForbid()
    {
        //Arrange
        var assignment = new LessonAssignment {Id = 5, TraineeId = 41};
        _assignmentRepoMock.Setup(r =>r.GetById(5)).Returns(assignment);
        SetUser(80);

        //Act
        var result = _controller.StartAssignment(5);

        //Assert
        Assert.IsType<ForbidResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    /// <summary>
    /// test for a trainee starting an assignment in a invalid state
    /// </summary>
    [Fact]
    public void Start_InvalidTransition_SetErrorAndRedirectToLessonBoard()
    {
        //arrange
        var assignment = new LessonAssignment {Id = 5, TraineeId = 41, Status =LessonAssignmentStatus.Finished};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup( s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Started)).Throws(new InvalidOperationException("Invalid status transition for assignment 5: Finished -> started"));
        SetUser(41);

        //Act
        var result = _controller.StartAssignment(5);

        //Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("LessonBoard", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);
        Assert.NotNull(_controller.TempData["Error"]);

    }

    /// <summary>
    /// test for a trainee finishing an assignment (successful)
    /// </summary>
    [Fact]
    public void Finish_ValidAssignment_UpdateStatusAndRedirectToLessonBoard()
    {
        //arrange
        var assignment = new LessonAssignment {Id = 5, TraineeId = 41};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup( s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Finished)).Returns(assignment);
        SetUser(41);

        //Act
        var result = _controller.FinishAssignment(5);

        //Assert
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Finished), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("LessonBoard", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);

    }

    /// <summary>
    /// test for a trainee finishing an unknown assignment
    /// </summary>
    [Fact]
    public void Finish_UnknownAssignment_ReturnNotFound()
    {
        //Arrange
        _assignmentRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((LessonAssignment?) null);
        
        //Act
        var result = _controller.FinishAssignment(999);

        //Assert
         Assert.IsType<NotFoundResult>(result);
         _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    /// <summary>
    /// test for trainee finishsing an assignment with no authorization
    /// </summary>
    [Fact]
    public void Finish_NoUserClaim_ReturnUnauthorized()
    {
        //Arrange
        var assignment = new LessonAssignment {Id = 5, TraineeId = 41};
        _assignmentRepoMock.Setup(r =>r.GetById(5)).Returns(assignment);
        SetUser(null);

        //Act
        var result = _controller.FinishAssignment(5);

        //Assert
        Assert.IsType<UnauthorizedResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    /// <summary>
    /// test for a trainee finishing an assignment that is assigned to another trainee
    /// </summary>
    [Fact]
    public void Finish_AssignmentNotOwnedByTrainee_ReturnForbid()
    {
        //Arrange
        var assignment = new LessonAssignment {Id = 5, TraineeId = 41};
        _assignmentRepoMock.Setup(r =>r.GetById(5)).Returns(assignment);
        SetUser(80);

        //Act
        var result = _controller.FinishAssignment(5);

        //Assert
        Assert.IsType<ForbidResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    /// <summary>
    /// test for finishing an assignment in an invalid state
    /// </summary>
    [Fact]
    public void Finish_InvalidTransition_SetErrorAndRedirectToLessonBoard()
    {
        //arrange
        var assignment = new LessonAssignment {Id = 5, TraineeId = 41, Status =LessonAssignmentStatus.Accepted};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup( s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Finished)).Throws(new InvalidOperationException("Invalid status transition for assignment 5: Accepted -> Finished"));
        SetUser(41);

        //Act
        var result = _controller.FinishAssignment(5);

        //Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("LessonBoard", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);
        Assert.NotNull(_controller.TempData["Error"]);

    }

    /// <summary>
    /// test for a trainee opening an assignment (successful)
    /// </summary>
    [Fact]
    public void Open_ValidAssignment_UpdateStatusAndRedirectToLessonBoard()
    {
        //arrange
        var assignment = new LessonAssignment {Id = 5, TraineeId = 41};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup( s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Open)).Returns(assignment);
        SetUser(41);

        //Act
        var result = _controller.OpenAssignment(5);

        //Assert
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Open), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("LessonBoard", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);

    }

    /// <summary>
    /// test for a trainee opening an unknown assignment
    /// </summary>
    [Fact]
    public void Open_UnknownAssignment_ReturnNotFound()
    {
        //Arrange
        _assignmentRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((LessonAssignment?) null);
        
        //Act
        var result = _controller.OpenAssignment(999);

        //Assert
         Assert.IsType<NotFoundResult>(result);
         _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    /// <summary>
    /// test for trainee opening an assignment with no authorization
    /// </summary>
    [Fact]
    public void Open_NoUserClaim_ReturnUnauthorized()
    {
        //Arrange
        var assignment = new LessonAssignment {Id = 5, TraineeId = 41};
        _assignmentRepoMock.Setup(r =>r.GetById(5)).Returns(assignment);
        SetUser(null);

        //Act
        var result = _controller.OpenAssignment(5);

        //Assert
        Assert.IsType<UnauthorizedResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    /// <summary>
    /// test for a trainee opening an assignment that is assigned to another trainee
    /// </summary>
    [Fact]
    public void Open_AssignmentNotOwnedByTrainee_ReturnForbid()
    {
        //Arrange
        var assignment = new LessonAssignment {Id = 5, TraineeId = 41};
        _assignmentRepoMock.Setup(r =>r.GetById(5)).Returns(assignment);
        SetUser(80);

        //Act
        var result = _controller.OpenAssignment(5);

        //Assert
        Assert.IsType<ForbidResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    /// <summary>
    /// test for opening an assignment in an invalid state
    /// </summary>
    [Fact]
    public void Open_InvalidTransition_SetErrorAndRedirectToLessonBoard()
    {
        //arrange
        var assignment = new LessonAssignment {Id = 5, TraineeId = 41, Status =LessonAssignmentStatus.Accepted};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup( s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Open)).Throws(new InvalidOperationException("Invalid status transition for assignment 5: Accepted -> Open"));
        SetUser(41);

        //Act
        var result = _controller.OpenAssignment(5);

        //Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("LessonBoard", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);
        Assert.NotNull(_controller.TempData["Error"]);

    }
}